using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace Semnox.Parafait.Device.PaymentGateway
{
    class NCRApiFunctions
    {
        private const string DLLNAME = @"mtx_pos.dll";

        #region GenericFunctions
        /// <summary>
        /// Calling MTX_POS_BeginOrder will clear the interval message and open the Pin Pad.The transaction order is performed here.
        /// </summary>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_BeginOrder")]
        public static extern void BeginOrder();
        // MTX_POS_BeginOrder()

        /// <summary>
        /// When the cashier signs off the POST it should de-activate the MTX_POS.DLL by calling
        /// MTX_POS_CheckerSignOff.
        /// </summary>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_CheckerSignOff")]
        public static extern void CheckerSignOff();
        // MTX_POS_CheckerSignOff()

        /// <summary>
        /// Method used for signOn and this maethod is called only once when the cashier login.
        /// </summary>
        /// <param name="cashierID"> The Cashier id is the login id of the cashier who logged in.
        /// Since a cashier is not actually signed on yet, the POS must set a ‘dummy’ cashier number such at ‘1’. </param>
        /// <param name="laneNumber">OpenEPS only supports lane numbers from 1 to 99. 
        /// The lane number field is for numeric entry only. Do not pad the field with spaces or other non-numeric characters.</param>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_CheckerSignOn")]
        public static extern void CheckerSignOn(string cashierID, string laneNumber);
        // MTX_POS_CheckerSignOn(CashierID: string[9], LaneNumber: string[2])

        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_DataEntryContinue")]
        public static extern void DataEntryContinue();
        // MTX_POS_DataEntryContinue()

        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_DisplayIntervalMessage")]
        public static extern void DisplayIntervalMessage(string line1, string line2);
        // MTX_POS_DisplayIntervalMessage(Line1: Array[0..24] of char; Line2: Array[0..24] of char)

        /// <summary>
        /// MTX_POS_EndOrder contains no passed variables, and is used to indicate the finish of a specific order.
        /// </summary>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_EndOrder")]
        public static extern void EndOrder();
        // MTX_POS_EndOrder()

        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GenerateMTXSeqNum")]
        public static extern void GenerateMTXSeqNumber(ref int resultCode, [MarshalAs(UnmanagedType.LPStr)] StringBuilder mtxSeqNumber);
        // MTX_POS_GenerateMTXSeqNum(ResultCode: Int,SeqNum: String[6])

        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GetData")]
        public static extern void GetData([MarshalAs(UnmanagedType.LPStr)] StringBuilder bitmap);
        // MTX_POS_GetData(FieldToQuery:string[32])

        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_Keyboard")]
        public static extern void Keyboard(string line1, string line2, char inputDataType, string minInputLength, string maxInputLength);
        // MTX_POS_Keyboard(TextLineOne [String 29]; TextLineTwo [String 29]; InputDataType [String 1; see chart]; MinImpLeng [String 2]; MaxImpLeng [String 2])

        /// <summary>
        /// Locks the card – Used if the card appears on the ‘hot list’
        /// </summary>
        
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_Lock_WIC_ICC")]
        public static extern void Lock_WIC_ICC();
        // MTX_POS_Lock_WIC_ICC(LockDate [String 8; yyyymmdd]; LockTime [String 6; hhmmss]; LockReason [String 4])

        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_RedoSignatureCapture")]
        public static extern void RedoSignatureCapture();
        // MTX_POS_RedoSignatureCapture()

        /// <summary>
        /// Ends the smart card session and prompts the customer to remove the smart card
        /// </summary>
        /// CompletionTextString of length 2
        /// 01 - (No display)
        /// 02 - Thank You
        /// 03 - No Allowable WIC Items
        /// 04 - No Current WIC.
        /// 05 - Not Enough WIC Quantity
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_Remove_WIC_ICC")]
        public static extern void WIC_ICC([MarshalAs(UnmanagedType.LPStr)] StringBuilder bitmap);
        // MTX_POS_Remove_WIC_ICC(CompletionText [String 2])

        /// <summary>
        /// The MTX_POS_Reset function resets the OpenEPS DLL, clearing out all variables except the CashierID and LaneNumber.
        /// </summary>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_Reset")]
        public static extern void ResetClear();
        // MTX_POS_Reset();

        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_Reset_NonF")]
        public static extern void Reset_NonF();
        // MTX_POS_Reset_NonF();

        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_Reset_WOResetSCAT")]
        public static extern void ResetWithoutSCAT();
        // MTX_POS_Reset_WOResetSCAT();

        /// <summary>
        /// MTX_POS_SendTransaction is called at the end of a transaction to request OpenEPS send the transaction for Host approval. 
        /// The POST timer for transaction timeout should start right after calling MTX_POS_SendTransaction.
        /// </summary>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SendTransaction")]
        public static extern void SendTransaction();
        // MTX_POS_SendTransaction();

        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SendTransaction_BarCode")]
        public static extern void SendTransaction_BarCode();
        // MTX_POS_SendTransaction_BarCode();

        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SendTransaction_NonF")]
        public static extern void SendTransaction_NonF();
        // MTX_POS_SendTransaction_NonF();

        /// <summary>
        /// Starts the SmartWIC session and prompts the customer to insert their card.
        /// </summary>
        /// aMode
        /// 0 - Normal Processing
        /// 1 - Training
        /// 2 - Certification
        /// 3 - Certification/Training
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_START_WIC_Session")]
        public static extern void START_WIC_Session(int wicMode);
        // MTX_POS_START_WIC_Session(aMode: integer)

        /// <summary>
        /// Receives data from the pinpad.
        /// </summary>
        /// The pinpad data will be recievedin this field.
        /// <param name="receiveData"></param>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SW_Receive")]
        public static extern void SmartWIC_Receive(ref char[] receiveData);
        // MTX_POS_SW_Receive(SWReceiveData: array[0..500] of char)

        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_InitDLL")]
        public static extern void InitializeDLL(string laneNumber);
        // MTX_POS_InitDLL(LaneNumber [String 4])

        /// <summary>
        /// When MTX_POS_GET_SCATStatus returns SCATStatus=1 (Done),2 (Manual) or 6 (Done-Manual Not Allowed), 
        /// the POST will need to determine what data fields are still required by MTX_POS.DLL to complete the transaction.
        /// </summary>
        /// <param name="validationPassed">If additional information is required, this field will be set to False, 
        /// and the FieldsMissing string will specify which field is required.
        /// If additional information is not required, this field will return True, 
        /// and the FieldsMissing string will return all zeros.</param>
        /// <param name="fieldsMissing">FieldsMissing is a 32 character Fields* variable which describes the bitmap of data fields 
        /// that the POST will have to SET in order to complete the transaction.</param>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_ValidateData")]
        public static extern void ValidateData(ref bool validationPassed, [MarshalAs(UnmanagedType.LPStr)] StringBuilder fieldsMissing);
        // MTX_POS_ValidateData(ValidateData: boolean, FieldsMissing: string[32])

        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_TransactionComplete")]
        public static extern void TransactionComplete();
        // MTX_POS_TransactionComplete()

        /// <summary>
        /// Writes the updated prescription information back to the card.
        /// This is the data received from the Get_Prescription command modified to decrement the items purchased with the card.
        /// </summary>
        /// <param name="Update_Prescription">The Prescription informatio to update to the card.</param>
        [DllImport(@"mtx_pos.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_Update_Prescription")]
        public static extern void Update_Prescription(string Update_Prescription);
        //MTX_POS_Update_Prescription(string Update_Prescription);        
        #endregion

        #region SetFunctions
        public static class Set
        {
            #region BitAssociations
            // Bit 001
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_ResponseCode")]
            public static extern void ResponseCode(string responseCode);
            // MTX_POS_SET_ResponseCode (ResponseCode: string[4]);

            // Bit 002
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_LaneNumber")]
            public static extern void LaneNumber(string laneNumber);
            // MTX_POS_GET_LaneNumber (LaneNumber: string[4])

            // Bit 003
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_CashierID")]
            public static extern void CashierID(string cashierID);
            // MTX_POS_SET_CashierID (CashierID: string[4])

            // Bit 004
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_PurchaseAmount")]
            public static extern void PurchaseAmount(int purchaseAmount);
            // MTX_POS_SET_PurchaseAmount (PurchaseAmount: integer);

            // Bit 005
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_CashBackAmount")]
            public static extern void CashBackAmount(int cashBackAmount);
            // MTX_POS_SET_CashBackAmount (CashBackAmount: integer);

            // Bit 006
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_FeeAmount")]
            public static extern void FeeAmount(int feeAmount);
            // MTX_POS_SET_FeeAmount (FeeAmount: integer);

            // Bit 007
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_AccountType")]
            public static extern void AccountType(string accountType);
            // MTX_POS_SET_AccountType (Version: string[4]);

            // Bit 008
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_AccountBalance")]
            public static extern void AccountBalance(int accountBalance);
            // MTX_POS_SET_AccountBalance (AccountBalance: integer);

            // Bit 009
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_PersonalAccountNumber")]
            public static extern void PersonalAccountNumber(string personalAccountNumber);
            // MTX_POS_SET_PersonalAccountNumber(PersonalAccountNumber: string[22]);

            // Bit 010
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_ExpirationDate")]
            public static extern void ExpirationDate(string expirationDate);
            // MTX_POS_SET_ExpirationDate(ExpirationDate: string[4]);

            // Bit 011
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_EBTCashBalance")]
            public static extern void EBTCashBalance(int ebtCashBalance);
            // MTX_POS_SET_EBTCashBalance (EBTCashBalance: integer);

            // Bit 012
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_PrintEBTCashBalance")]
            public static extern void PrintEBTCashBalance(bool printEBTCashBalance);
            // MTX_POS_SET_PrintEBTCashBalance(PrintEBTCashBalance: Boolean);

            // Bit 013
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_EBTFoodStampBalance")]
            public static extern void EBTFoodStampBalance(int ebtFoodStampBalance);
            // MTX_POS_SET_EBTFoodStampBalance (EBTFoodStampBalance: integer);

            // Bit 014
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_PrintEBTFoodStampBalance")]
            public static extern void PrintEBTFoodStampBalance(bool printEBTFoodStampBalance);
            // MTX_POS_SET_PrintEBTFoodStampBalance(PrintEBTFoodStampBalance: Boolean);

            // Bit 015
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_VoucherNumber")]
            public static extern void VoucherNumber(string voucherNumber);
            // MTX_POS_SET_VoucherNumber (VoucherNumber: string[15]);

            // Bit 016
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_CustomerName")]
            public static extern void CustomerName(string customerName);
            // MTX_POS_SET_CustomerName (CustomerName: string[40]);

            // Bit 017
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_Track1Data")]
            public static extern void Track1Data([MarshalAs(UnmanagedType.LPStr)] StringBuilder track1Data);
            // MTX_POS_SET_Track1Data (Track1Data: string[80]);

            // Bit 018
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_Track2Data")]
            public static extern void Track2Data([MarshalAs(UnmanagedType.LPStr)] StringBuilder track2Data);
            // MTX_POS_SET_Track2Data (Track2Data: string[80]);

            // Bit 019
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_ManagerID")]
            public static extern void ManagerID(string managerID);
            // MTX_POS_SET_ManagerID (ManagerID: string[9]);

            /// <summary>
            /// This function call is only used if the transaction has already been sent to the host, 
            /// and it received a ‘Soft’ or Overridable decline
            /// </summary>
            /// <param name="overrideFlag"></param>
            // Bit 020
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_OverrideFlag")]
            public static extern void OverrideFlag(bool overrideFlag);
            // MTX_POS_SET_OverrideFlag(OverrideFlag: Boolean);

            // Bit 021
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_MTXSequenceNumber")]
            public static extern void MTXSequenceNumber(string mtxSequenceNumber);
            // MTX_POS_SET_MTXSequenceNumber (MTXSequenceNumber: string[6]);

            // Bit 022
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_HostRetrievalReferenceNumber")]
            public static extern void HostRetrievalReferenceNumber(string hostRetrievalReferenceNumber);
            // MTX_POS_SET_HostRetrievalReferenceNumber (HostRetrievalReferenceNumber: string[12]);

            // Bit 023
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_HostReferenceNumber")]
            public static extern void HostReferenceNumber(string hostReferenceNumber);
            // MTX_POS_SET_HostReferenceNumber (HostReferenceNumber: string[10]);

            // Bit 024
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_MerchantIDNumber")]
            public static extern void MerchantIDNumber(string merchantIDNumber);
            // MTX_POS_SET_MerchantIDNumber (MerchantIDNumber: string[15]);

            // Bit 025
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_AuthorizationNumber")]
            public static extern void AuthorizationNumber(string authorizationNumber);
            // MTX_POS_SET_AuthorizationNumber (AuthorizationNumber: string[8]);

            // Bit 026
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_HostDeclineMessage")]
            public static extern void HostDeclineMessage(string postTransactionNumber);
            // MTX_POS_SET_HostDeclineMessage (HostDeclineMessage: string[16]);

            // Bit 027
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_CheckType")]
            public static extern void CheckType(string checkType);
            // MTX_POS_SET_CheckType (CheckType: string[4]);

            // Bit 028
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_CheckTransitRoutingNumber")]
            public static extern void CheckTransitRoutingNumber(string checkTransitRoutingNumber);
            // MTX_POS_SET_CheckTransitRoutingNumber (CheckTransitRoutingNumber: string[9]);

            // Bit 029
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_CheckAccountNumber")]
            public static extern void CheckAccountNumber(string checkAccountNumber);
            // MTX_POS_SET_CheckAccountNumber (CheckAccountNumber: string[22]);

            // Bit 030
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_CheckNumber")]
            public static extern void CheckNumber(string checkNumber);
            // MTX_POS_SET_CheckNumber (CheckNumber: string[8]);

            // Bit 031
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_PrimaryIDType")]
            public static extern void PrimaryIDType(string primaryIDType);
            // MTX_POS_SET_PrimaryIDType (PrimaryIDType: string[4]);

            // Bit 032
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_PrimaryID")]
            public static extern void PrimaryID(string primaryID);
            // MTX_POS_SET_PrimaryID (PrimaryID: string[40]);

            // Bit 033
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_StateCode")]
            public static extern void StateCode(string stateCode);
            // MTX_POS_SET_StateCode (StateCode: string[4]);

            // Bit 034
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_DateOfBirth")]
            public static extern void DateOfBirth(string dateOfBirth);
            // MTX_POS_SET_DateOfBirth (DateOfBirth: string[6]);

            // Bit 035
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_PIN")]
            public static extern void PIN(string pin);
            // MTX_POS_SET_PIN (PIN: string[16]);

            /// <summary>
            /// To set a value, the POST must call the specific function associated with the variable and bit returned 
            /// by the ValidateData function, and pass the MTX_POS_SET_VariableName function the value to be set.
            /// </summary>
            /// <param name="VariableNameData"></param>
            [DllImport(@"mtx_pos.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_VariableName")]
            public static extern void VariableName(string VariableNameData);
            //MTX_POS_SET_VariableName(string VariableNameData)

            // Bit 036
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_NewPIN")]
            public static extern void NewPIN(string newPIN);
            // MTX_POS_SET_NewPIN (NewPIN: string[16]);

            /// <summary>
            /// MTX_POS_SET_PostTransactionNumber must be called immediately after the MTX_POS_BeginOrder, with no other calls in between.
            /// </summary>
            /// <param name="postTransactionNumber">Setting the PostTransactionNumber is mandatory if the POS system is supporting 
            /// Biometrics. If the POS is not supporting Biometrics, setting PostTransactionNumber is optional, but recommended.</param>
            // Bit 037
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_PostTransactionNumber")]
            public static extern void POSTTransactionNumber(string postTransactionNumber);
            // MTX_POS_SET_PostTransactionNumber (PostTransactionNumber: string[22]);

            // Bit 038
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_ManualEntryMICRFlag")]
            public static extern void ManualEntryMICRFlag(bool manualEntryMICRFlag);
            // MTX_POS_SET_ManualEntryMICRFlag(ManualEntryMICRFlag: Boolean);

            // Bit 039
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_ManualEntryTrack2Flag")]
            public static extern void ManualEntryTrack2Flag(bool manualEntryTrack2Flag);
            // MTX_POS_SET_ManualEntryTrack2Flag(ManualEntryTrack2Flag: Boolean);

            // Bit 040
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_ManualEntryIDFlag")]
            public static extern void ManualEntryIDFlag(bool manualEntryIDFlag);
            // MTX_POS_SET_ManualEntryIDFlag(ManualEntryIDFlag: Boolean);

            // Bit 041
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_VelocityLinesCount")]
            public static extern void VelocityLinesCount(int velocityLinesCount);
            // MTX_POS_SET_VelocityLinesCount(VelocityLinesCount: integer);

            // Bit 042
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_VelocityLine")]
            public static extern void VelocityLine(string velocityLine);
            // MTX_POS_SET_VelocityLine(VelocityLine: string[40])

            // Bit 043
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_ReceiptCustomerLinesCount")]
            public static extern void ReceiptCustomerLinesCount(int receiptCustomerLinesCount);
            // MTX_POS_SET_ReceiptCustomerLinesCount(ReceiptCustomerLinesCount: integer);

            // Bit 044
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_ReceiptCustomerLine")]
            public static extern void ReceiptCustomerLine(int receiptCutomerLineNo, string receiptCutomerLine);
            // MTX_POS_SET_ReceiptCustomerLine(ReceiptCutomerLineNo: integer, ReceiptCutomerLine: string[40])

            // Bit 045
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_ReceiptDrawerLinesCount")]
            public static extern void ReceiptDrawerLinesCount(int receiptDrawerLinesCount);
            // MTX_POS_SET_ReceiptDrawerLinesCount(ReceiptDrawerLinesCount: integer);

            // Bit 046
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_ReceiptDrawerLine")]
            public static extern void ReceiptDrawerLine(int receiptDrawerLineNo, string receiptDrawerLine);
            // MTX_POS_SET_ReceiptDrawerLine(ReceiptDrawerLineNo: integer, ReceiptDrawerLine: string[40])

            // Bit 047
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_UPCCode")]
            public static extern void UPCCode(string upcCode);
            // MTX_POS_SET_UPCCode(UPCCode: string[14])

            // Bit 048
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_PONumber")]
            public static extern void PONumber(string poNumber);
            // MTX_POS_SET_PONumber (PONumber: string[12]);

            // Bit 049
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_CashierDisplay1")]
            public static extern void CashierDisplay1(string cashierDisplay1);
            // MTX_POS_SET_CashierDisplay1 (CashierDisplay1: string[20]);

            // Bit 050
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_CashierDisplay2")]
            public static extern void CashierDisplay2(string cashierDisplay2);
            // MTX_POS_SET_CashierDisplay2 (CashierDisplay2: string[20]);

            // Bit 051
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_CustomerDisplay1")]
            public static extern void CustomerDisplay1(string customerDisplay1);
            // MTX_POS_SET_CustomerDisplay1 (CustomerDisplay1: string[20]);

            // Bit 052
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_CustomerDisplay2")]
            public static extern void CustomerDisplay2(string customerDisplay2);
            // MTX_POS_SET_CustomerDisplay2 (CustomerDisplay2: string[20]);

            // Bit 053
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_TransactionType")]
            public static extern void TransactionType(int transactionType);
            // MTX_POS_SET_TransactionType(TransactionType: integer);

            // Bit 054
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_TransactionDate")]
            public static extern void TransactionDate(string transactionDate);
            // MTX_POS_SET_TransactionDate (TransactionDate: string[6]);

            // Bit 055
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_TransactionTime")]
            public static extern void TransactionTime(string transactionTime);
            // MTX_POS_SET_TransactionTime (TransactionTime: string[6]);

            // it 056
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_TrainingTransaction")]
            public static extern void TrainingTransaction(bool trainingTransaction);
            // MTX_POS_SET_TrainingTransaction(TrainingTransaction: Boolean);

            // Bit 057
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_ReceiptRequired")]
            public static extern void ReceiptRequired(bool receiptRequired);
            // MTX_POS_SET_ReceiptRequired(ReceiptRequired: Boolean);

            // Bit 058
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_UserField3")]
            public static extern void UserField3(string userField3);
            // MTX_POS_SET_UserField3(UserField3: string[10])

            // Bit 059
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_UserField4")]
            public static extern void UserField4(string userField4);
            // MTX_POS_SET_UserField4(UserField4: string[8])

            // Bit 060
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_DiagnosticLinesCount")]
            public static extern void DiagnosticLinesCount(int diagnosticLinesCount);
            // MTX_POS_SET_DiagnosticLinesCount(DiagnosticLinesCount: integer);

            // Bit 061
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_DiagnosticLine")]
            public static extern void DiagnosticLine(string diagnosticLine);
            // MTX_POS_SET_DiagnosticLine(DiagnosticLine: string[20])

            // Bit 062
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_SecondaryIDType")]
            public static extern void SecondaryIDType(string secondaryIDType);
            // MTX_POS_SET_SecondaryIDType(SecondaryIDType: string[4])

            // Bit 063
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_SecondaryID")]
            public static extern void SecondaryID(string secondaryID);
            // MTX_POS_SET_SecondaryID(SecondaryID: string[40])

            // Bit 064
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_CashierOKCashback")]
            public static extern void CashierOKCashback(bool cashierOKCashback);
            // MTX_POS_SET_CashierOKCashback(CashierOKCashback: Boolean);

            // Bit 065
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_FleetData")]
            public static extern void FleetData(string fleetData);
            // MTX_POS_SET_FleetData(FleetData: string[500])

            // Bit 066
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_Odometer")]
            public static extern void Odometer(string odometer);
            // MTX_POS_SET_Odometer(Odometer: string[10])

            // Bit 067
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_VehicleID")]
            public static extern void VehicleID(string vehicleID);
            // MTX_POS_SET_VehicleID(FleetData: string[20])

            // Bit 068
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_DriverID")]
            public static extern void DriverID(string driverID);
            // MTX_POS_SET_DriverID(DriverID: string[20])

            // Bit 069
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_ProductRestrictionCode")]
            public static extern void ProductRestrictionCode(int productRestrictionCode);
            // MTX_POS_SET_ProductRestrictionCode(ProductRestrictionCode: integer);

            // Bit 070
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_POSEnteredRestrictionCode")]
            public static extern void POSEnteredRestrictionCode(string posEnteredRestrictionCode);
            // MTX_POS_SET_POSEnteredRestrictionCode(POSEnteredRestrictionCode: string[20])

            // Bit 071
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_MaxFleetProducts")]
            public static extern void MaxFleetProducts(int maxFuelItems, int maxNonFuelItems, int maxTotalItems);
            // MTX_POS_SET_MaxFleetProducts(MaxFuelItems: integer; MaxNonFuelItems: integer; MaxTotalItems: integer);

            // Bit 072
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_ItemExpDate")]
            public static extern void ItemExpDate(string itemExpDate);
            // MTX_POS_SET_ItemExpDate(ItemExpDate: string[6])

            // Bit 073
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_Account2Balance")]
            public static extern void Account2Balance(int account2Balance);
            // MTX_POS_SET_Account2Balance (Account2Balance: integer);

            // Bit 074
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_PhoneNumber")]
            public static extern void PhoneNumber(string phoneNumber);
            // MTX_POS_SET_PhoneNumber(PhoneNumber: string[32])

            // Bit 075
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_SignatureCapture")]
            public static extern void SignatureCapture(string signatureCaptureData, int dataLength);
            // MTX_POS_SET_SignatureCapture(SignatureCaptureData: string[500], DataLength: integer)

            // Bit 076
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_DukptKeySerialNumber")]
            public static extern void DukptKeySerialNumber(string dukptKeySerialNumber);
            // MTX_POS_SET_DukptKeySerialNumber(DukptKeySerialNumber: string[20])

            // Bit 077
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_SCATCustomerDisplay")]
            public static extern void MTX_POS_SET_SCATCustomerDisplay(string line0, string line1, string line2, string line3);
            // MTX_POS_SET_SCATCustomerDisplay(Line0: string[45]; Line1: string[45]; Line2: string[45]; Line3: string[45]);

            // Bit 078
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_ERCRequired")]
            public static extern void ERCRequired(int ercRequired);
            // MTX_POS_SET_ERCRequired(ERCRequired: integer);

            // Bit 079
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_ReceiptHeaderCount")]
            public static extern void ReceiptHeaderCount(int receiptHeaderCount);
            // MTX_POS_SET_ReceiptHeaderCount(ReceiptHeaderCount: integer);

            // Bit 080
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_ReceiptHeaderLine")]
            public static extern void ReceiptHeaderLine(int lineId, string line);
            // MTX_POS_SET_ReceiptHeaderLine(LineID: Integer; ReceiptHeaderLine: string[40])

            // Bit 081
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_ReceiptTrailCount")]
            public static extern void ReceiptTrailCount(int receiptTrailCount);
            // MTX_POS_SET_ReceiptTrailCount(ReceiptTrailCount: integer);

            // Bit 082
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_ReceiptTrailLine")]
            public static extern void ReceiptTrailLine(int lineId, string line);
            // MTX_POS_SET_ReceiptTrailLine(LineID: Integer; ReceiptTrailLine: string[40])

            // Bit 083
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_MICRFields")]
            public static extern void MICRFields(string routingNumber, string accountNumber, string checkNumber);
            // MTX_POS_SET_MICRFields(RoutingNumber: string[9], AccountNumber: string[22], CheckNumber: string[8])

            // Bit 084
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_TaxAmount")]
            public static extern void TaxAmount(int taxAmount);
            // MTX_POS_SET_TaxAmount(TaxAmount: integer);

            // Bit 085
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_RawMICR")]
            public static extern void RawMICR(byte[] rawMICR, int micrLength);
            // MTX_POS_SET_RawMICR(ARawMICR : Array100; Asize : Integer)

            // Bit 086 - ***RESERVED FOR FUTURE USE***
            // [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_CheckImage")]
            // public static extern void checkImage(byte[] checkImage, int checkImageLength);
            // MTX_POS_SET_CheckImage(AcheckImage : Array500; Asize : Integer)

            // Bit 087
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_SignatureCaptureOPOS")]
            public static extern void SignatureCaptureOPOS(byte[] signatureCaptureData, int signatureCaptureDataLength);
            // MTX_POS_SET_SignatureCaptureOPOS(SignatureCaptureData :  Array2000; Asize : Integer)

            /// <summary>
            /// If the POS is able to accept an approval for an amount other than the amount initially submitted, then the
            /// POST should call MTX_POS_SET_AmountChangeAllowed, setting it to true.
            /// </summary>
            /// <param name="amountChangeAllowed">If AmountChangeAllowed is set to True, the POS may accept approvals 
            /// for lesser amounts.</param>
            // Bit 088
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_AmountChangeAllowed")]
            public static extern void AmountChangeAllowed(bool amountChangeAllowed);
            // MTX_POS_SET_AmountChangeAllowed(AmountChangeAllowed: Boolean);

            // Bit 089
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_ApprovedAmount")]
            public static extern void ApprovedAmount(int approvedAmount);
            // MTX_POS_SET_ApprovedAmount(ApprovedAmount: integer);

            // Bit 090
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_POSVerifyCard")]
            public static extern void POSVerifyCard(bool posVerifyCard);
            // MTX_POS_SET_POSVerifyCard(VerifyCard: Boolean);

            // Bit 091
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_BioDOB")]
            public static extern void BioDOB(string bioDOB);
            // MTX_POS_SET_BioDOB(BioDOB: string[8])

            // Bit 091
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_BioFreqShop")]
            public static extern void BioFreqShop(string bioFreqShop);
            // MTX_POS_SET_BioFreqShop(BioFreqShop: string[40])

            // Bit 091
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_ZipCode")]
            public static extern void ZipCode(string zipCode);
            // MTX_POS_SET_ZipCode(ZipCode: string[10])

            // Bit 092
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_FuelAmount")]
            public static extern void FuelAmount(int fuelAmount);
            // MTX_POS_SET_FuelAmount(FuelAmount: integer);

            // Bit 093
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_CVV2")]
            public static extern void CVV2(string cvv2);
            // MTX_POS_SET_CVV2(CVV2: string[9])

            // Bit 094
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_ControlNumber")]
            public static extern void ControlNumber(string controlNumber);
            // MTX_POS_SET_ControlNumber(ControlNumber: string[20])

            // Bit 095
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_WirelessPIN")]
            public static extern void WirelessPIN(string wirelessPIN);
            // MTX_POS_SET_WirelessPIN(WirelessPIN: string[20])

            // Bit 096
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_POSVerifyCustomerName")]
            public static extern void POSVerifyCustomerName(bool posVerifyCustomerName);
            // MTX_POS_SET_POSVerifyCustomerName(VerifyCustomerName: Boolean);

            // Bit 100
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_IDVerificationNumber")]
            public static extern void IDVerificationNumber(string idVerificationNumber);
            // MTX_POS_SET_IDVerificationNumber(VerificationNumber: string[4])

            // Bit 102
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_FsaAmount")]
            public static extern void FSAAmount(int fsaAmount);
            // MTX_POS_SET_FsaAmount(FsaAmount: integer);

            // Bit 103
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_RxAmount")]
            public static extern void RxAmount(int rxAmount);
            // MTX_POS_SET_RxAmount(RxAmount: integer);

            // Bit 104
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_DentalAmount")]
            public static extern void DentalAmount(int dentalAmount);
            // MTX_POS_SET_DentalAmount(DentalAmount: integer);

            // Bit 105
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_MedicalAmount")]
            public static extern void MedicalAmount(int medicalAmount);
            // MTX_POS_SET_MedicalAmount(MedicalAmount: integer);

            // Bit 106
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_VisionAmount")]
            public static extern void VisionAmount(int visionAmount);
            // MTX_POS_SET_VisionAmount(VisionAmount: integer);

            // Bit 107
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_ManagerIDOrSecondaryID")]
            public static extern void ManagerIDOrSecondaryID(int managerIDOrSecondaryID);
            // MTX_POS_SET_ManagerIDOrSecondaryID(ManagerIDOrSecondaryID: integer);

            // Bit 108
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_eWICRx")]
            public static extern void eWicRx(int rxLength, string rxData, string merchantDiscount, string stateCode);
            // MTX_POS_SET_eWICRx(RxLength: integer, RxData: string [3000], MerchantDiscount: string[20], StateCode: string [2]);

            // Bit 109
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_POSVerifyCardLast4")]
            public static extern void POSVerifyCardLast4(string verifyDigits);
            // MTX_POS_SET_POSVerifyCardLast4(VerifyCardLast4: string[4])

            // Bit 114
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_SVSAuthorizationNumber")]
            public static extern void SVSAuthorizationNumber(string svsAuthorizationNumber);
            // MTX_POS_SET_SVSAuthorizationNumber(SVSAuthorizationNumber: string[8])

            // Bit 115
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_ServerInformation")]
            public static extern void ServerInformation(string serverInformation);
            // MTX_POS_SET_ServerInformation(ServerInformation: string[1000])

            // Bit 117
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_SocialSecurityNumber")]
            public static extern void SocialSecurityNumber(string socialSecurityNumber);
            // MTX_POS_SET_SocialSecurityNumber(SSN: string[9])

            // Bit 118
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_PayrollCheckIssueDate")]
            public static extern void PayrollCheckIssueDate(string payrollCheckIssueDate);
            // MTX_POS_SET_PayrollCheckIssueDate(IssueDate: string[8])

            // Bit 119
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_CustomerNumber")]
            public static extern void CustomerNumber(string payrollCheckIssueDate);
            // MTX_POS_SET_CustomerNumber(CustomerNumber: string[40])
            #endregion

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_BackgroundProcess")]
            public static extern void BackgroundProcess(string transaction, ref int result);
            // MTX_POS_SET_BackgroundProcess(aTransaction :String,  var Result :integer)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_BarCodeActivation")]
            public static extern void BarCodeActivation(string transaction, ref int result);
            // MTX_POS_SET_BarCodeActivation(aTransaction :String,  var Result :integer)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_BioCustomerName")]
            public static extern void BioCustomerName(string bioCustomerName);
            // MTX_POS_SET_BioCustomerName(BioCustomerName: string[40])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_BioDriversLicNumber")]
            public static extern void BioDriversLicNumber(string bioDriversLicNumber);
            // MTX_POS_SET_BioDriversLicNumber(BioDriversLicNumber: string[40])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_BioDriversLicStateCode")]
            public static extern void BioDriversLicStateCode(string bioDriversLicStateCode);
            // MTX_POS_SET_BioDriversLicStateCode(BioDriversLicStateCode: string[4])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_BioMetricsTran")]
            public static extern void BioMetricsTran(bool bioMetricsTran);
            // MTX_POS_SET_BioMetricsTran(aBioTran: Boolean)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_CurrentWorkingKey0")]
            public static extern void CurrentWorkingKey0(string currentWorkingKey0);
            // MTX_POS_SET_CurrentWorkingKey0(WorkingKey: string[16])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_CustomerInformation")]
            public static extern void CustomerInformation(string customerName, string customerAddress, string customerCity, string customerState, string customerZip, string customerPhoneNumber, string customerEmail, string promotionCode);
            // MTX_POS_SET_CustomerInformation(CustomerName: string[40]; CustomerAddress: string[40]; CustomerCity: string[40]; CustomerState: string[4]; CustomerZip: string[10]; CustomerPhoneNumber: string[20]; CustomerEMail: string[40]; PromotionCode: string[10])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_CustomerOK")]
            public static extern void CustomerOK(int customerOK);
            // MTX_POS_SET_CustomerOK (CustomerOK: Integer);

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_DataEntry")]
            public static extern void DataEntry(int entryMode, string prompt1, string prompt2, int minLength, int maxLength);
            // MTX_POS_SET_DataEntry (EntryMode: integer, Prompt1: string[20], Prompt2: string[20], MinLength: integer, MaxLength: integer);

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_DepartmentNumber")]
            public static extern void DepartmentNumber(string departmentNumber);
            // MTX_POS_SET_DepartmentNumber(DepartmentNumber: string[6])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_ExtendedCashierPrompts")]
            public static extern void ExtendedCashierPrompts(string[] extendedCashierPrompts);
            // MTX_POS_SET_ExtendedCashierPrompts(line1,line2,line3,line4,line5: array[40])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_Extended_DataEntry")]
            public static extern void ExtendedDataEntry(string prompt, bool signatureRequired);
            // MTX_POS_SET_Extended_DataEntry(Prompt: string[1000], SignatureRequired:Boolean)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_FieldsRequired")]
            public static extern void FieldsRequired(string fieldsRequired);
            // MTX_POS_SET_FieldsRequired(FieldsRequired: string[32])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_FieldsRequiredi")]
            public static extern void FieldsRequiredi(int fieldsRequiredi);
            // MTX_POS_SET_FieldsRequiredi(FieldsRequiredi: integer[4])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_HOSTStatus")]
            public static extern void HostStatus(int hostStatus);
            // MTX_POS_SET_HOSTStatus(HOSTStatus: integer[4])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_LanguageID")]
            public static extern void LanguageID(string languageID);
            // MTX_POS_SET_LanguageID(LanguageID: string[1])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_MICRStatus")]
            public static extern void MICRStatus(int micrStatus);
            // MTX_POS_SET_MICRStatus(MICRStatus: integer)

            /// <summary>
            /// It is recommended that the POST call MTX_POS_SET_POSTVersion and set the name and version of the POS software
            /// after the sign on. The version field is a string and so can contain both the name and the specific version number 
            /// of the POS software. This data is logged in the journal and can be useful for troubleshooting.
            /// </summary>
            /// <param name="postVersion">Is of 45 length and in "POS Software Name: XXX.XXX.XXX.XXX" format. Sofware name and Version is saparated ":"</param>
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_POSTVersion")]
            public static extern void POSTVersion(string postVersion);
            // MTX_POS_SET_POSTVersion(POSTVersion: string[45])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_SCATFieldPending")]
            public static extern void SCATFieldPending(string scatFieldPending);
            // MTX_POS_SET_SCATFieldPending(MTX_POS_SET_SCATFieldPending: string[32])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_SCATFieldPendingi")]
            public static extern void SCATFieldPendingi(int scatFieldPendingi);
            // MTX_POS_SET_SCATFieldPendingi(SCATFieldPendingi: integer[4])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_SCATStatus")]
            public static extern void SCATStatus(int scatStatus);
            // MTX_POS_SET_SCATStatus(SCATStatus: integer)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_StoreNumber")]
            public static extern void StoreNumber(string storeNumber);
            // MTX_POS_SET_StoreNumber(MTX_POS_SET_StoreNumber: string[8])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_TenderTypeMTX")]
            public static extern void TenderTypeMTX(StringBuilder tenderTypeMTX);
            // MTX_POS_SET_TenderTypeMTX(TenderTypeMTX: string[32])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_TenderTypeMTXi")]
            public static extern void TenderTypeMTXi(int tenderTypeMHXi);
            // MTX_POS_SET_TenderTypeMTXi(TenderTypeMTXi: int[4])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_TenderType_NonF")]
            public static extern void TenderType_NonF(int TenderType_NonF);
            // MTX_POS_SET_TenderType_NonF(TenderType_NonF: int)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_TenderTypePOS")]
            public static extern void TenderTypePOS(string tenderTypePOS);
            // MTX_POS_SET_TenderTypePOS(TenderTypePOS: string[32])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_TenderTypePOSi")]
            public static extern void TenderTypePOSi(int tenderTypePOSi);
            // MTX_POS_SET_TenderTypePOSi(TenderTypePOSi: int[4])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_TenderTypeStatus")]
            public static extern void TenderTypeStatus(int tenderTypeStatus);
            // MTX_POS_SET_TenderTypeStatus(TenderTypeStatus: int)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_TicketLookupSearchWindow")]
            public static extern void TicketLookupSearchWindow(string ticketLookupSearchWindow);
            // MTX_POS_SET_TicketLookupSearchWindow(DateRange: string[17])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_TransactionType_NonF")]
            public static extern void TransactionType_NonF(int transactionType_NonF);
            // MTX_POS_SET_TransactionType_NonF(TranType: int)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_UserID")]
            public static extern void UserID(string userID);
            // MTX_POS_SET_UserID(UserID: string[40])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_VirtualReceiptLines")]
            public static extern void VirtualReceiptLines(string[] lines);
            // MTX_POS_SET_VirtualReceiptLines(Line1:string[45],Line2:string[45],Line3:string[45],Line4:string[45])
            // 4-element array consisting of strings with a length no greater than 45.

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_VisaTranID")]
            public static extern void VisaTranID(string visaTranID);
            // MTX_POS_SET_VisaTranID(VisaTranID: string[15])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_WinEPSCardType")]
            public static extern void WinEPSCardType(string winEPSCardType);
            // MTX_POS_SET_WinEPSCardType(WinEPSCardType: string[18])
        }
        #endregion

        #region GetFunctions
        public class Get
        {
            #region BitAssociations
            // Bit 001
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ResponseCode")]
            public static extern void ResponseCode([MarshalAs(UnmanagedType.LPStr)] StringBuilder responseCode);
            // MTX_POS_GET_ResponseCode (ResponseCode: string[4]);

            // Bit 002
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_LaneNumber")]
            public static extern void LaneNumber([MarshalAs(UnmanagedType.LPStr)] StringBuilder laneNumber);
            // MTX_POS_GET_LaneNumber (LaneNumber: string[4])

            // Bit 003
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_CashierID")]
            public static extern void CashierID([MarshalAs(UnmanagedType.LPStr)] StringBuilder cashierID);
            // MTX_POS_GET_CashierID (CashierID: string[4])

            // Bit 004
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_PurchaseAmount")]
            public static extern void PurchaseAmount(ref int purchaseAmount);
            // MTX_POS_GET_PurchaseAmount (PurchaseAmount: integer);

            /// <summary>
            /// The POS should call the MTX_POS_GET_CashBackAmount to get the final approved cashback amount.
            /// </summary>
            /// <param name="cashBackAmount">The cash back amount is an integer with an assumed decimal place for cents.
            /// For example, a value of 123 indicates $1.23.</param>
            // Bit 005
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_CashBackAmount")]
            public static extern void CashBackAmount(ref int cashBackAmount);
            // MTX_POS_GET_CashBackAmount (CashBackAmount: integer);

            // Bit 006
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_FeeAmount")]
            public static extern void FeeAmount(ref int feeAmount);
            // MTX_POS_GET_FeeAmount (FeeAmount: integer);

            // Bit 007
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_AccountType")]
            public static extern void AccountType([MarshalAs(UnmanagedType.LPStr)] StringBuilder accountType);
            // MTX_POS_GET_AccountType (Version: string[4]);

            // Bit 008
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_AccountBalance")]
            public static extern void AccountBalance(ref int accountBalance);
            // MTX_POS_GET_AccountBalance (AccountBalance: integer);

            // Bit 009
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_PersonalAccountNumber")]
            public static extern void PersonalAccountNumber([MarshalAs(UnmanagedType.LPStr)] StringBuilder personalAccountNumber);
            // MTX_POS_GET_PersonalAccountNumber(PersonalAccountNumber: string[22]);

            // Bit 010
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ExpirationDate")]
            public static extern void ExpirationDate([MarshalAs(UnmanagedType.LPStr)] StringBuilder expirationDate);
            // MTX_POS_GET_ExpirationDate(ExpirationDate: string[22]);

            // Bit 011
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_EBTCashBalance")]
            public static extern void EBTCashBalance(ref int ebtCashBalance);
            // MTX_POS_GET_EBTCashBalance (EBTCashBalance: integer);

            // Bit 012
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_PrintEBTCashBalance")]
            public static extern void PrintEBTCashBalance(ref bool printEBTCashBalance);
            // MTX_POS_GET_PrintEBTCashBalance(PrintEBTCashBalance: Boolean);

            // Bit 013
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_EBTFoodStampBalance")]
            public static extern void EBTFoodStampBalance(ref int ebtFoodStampBalance);
            // MTX_POS_GET_EBTFoodStampBalance (EBTFoodStampBalance: integer);

            // Bit 014
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_PrintEBTFoodStampBalance")]
            public static extern void PrintEBTFoodStampBalance(ref bool printEBTFoodStampBalance);
            // MTX_POS_GET_PrintEBTFoodStampBalance(PrintEBTFoodStampBalance: Boolean);

            // Bit 015
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_VoucherNumber")]
            public static extern void VoucherNumber([MarshalAs(UnmanagedType.LPStr)] StringBuilder voucherNumber);
            // MTX_POS_GET_VoucherNumber (VoucherNumber: string[15]);

            // Bit 016
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_CustomerName")]
            public static extern void CustomerName([MarshalAs(UnmanagedType.LPStr)] StringBuilder customerName);
            // MTX_POS_GET_CustomerName (CustomerName: string[40]);

            // Bit 017
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_Track1Data")]
            public static extern void Track1Data([MarshalAs(UnmanagedType.LPStr)] StringBuilder track1Data);
            // MTX_POS_GET_Track1Data (Track1Data: string[80]);

            // Bit 018
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_Track2Data")]
            public static extern void Track2Data([MarshalAs(UnmanagedType.LPStr)] StringBuilder track2Data);
            // MTX_POS_GET_Track2Data (Track2Data: string[80]);

            // Bit 019
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ManagerID")]
            public static extern void ManagerID([MarshalAs(UnmanagedType.LPStr)] StringBuilder managerID);
            // MTX_POS_GET_ManagerID (ManagerID: string[9]);

            // Bit 020
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_OverrideFlag")]
            public static extern void OverrideFlag(ref bool overrideFlag);
            // MTX_POS_GET_OverrideFlag(OverrideFlag: Boolean);

            // Bit 021
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_MTXSequenceNumber")]
            public static extern void MTXSequenceNumber([MarshalAs(UnmanagedType.LPStr)] StringBuilder mtxSequenceNumber);
            // MTX_POS_GET_MTXSequenceNumber (MTXSequenceNumber: string[6]);

            // Bit 022
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_HostRetrievalReferenceNumber")]
            public static extern void HostRetrievalReferenceNumber([MarshalAs(UnmanagedType.LPStr)] StringBuilder hostRetrievalReferenceNumber);
            // MTX_POS_GET_HostRetrievalReferenceNumber (HostRetrievalReferenceNumber: string[12]);

            // Bit 023
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_HostReferenceNumber")]
            public static extern void HostReferenceNumber([MarshalAs(UnmanagedType.LPStr)] StringBuilder hostReferenceNumber);
            // MTX_POS_GET_HostReferenceNumber (HostReferenceNumber: string[10]);

            // Bit 024
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_MerchantIDNumber")]
            public static extern void MerchantIDNumber([MarshalAs(UnmanagedType.LPStr)] StringBuilder merchantIDNumber);
            // MTX_POS_GET_MerchantIDNumber (MerchantIDNumber: string[15]);

            // Bit 025
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_AuthorizationNumber")]
            public static extern void AuthorizationNumber([MarshalAs(UnmanagedType.LPStr)] StringBuilder authorizationNumber);
            // MTX_POS_GET_AuthorizationNumber (AuthorizationNumber: string[8]);

            // Bit 026
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_HostDeclineMessage")]
            public static extern void HostDeclineMessage([MarshalAs(UnmanagedType.LPStr)] StringBuilder postTransactionNumber);
            // MTX_POS_GET_HostDeclineMessage (HostDeclineMessage: string[16]);

            // Bit 027
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_CheckType")]
            public static extern void CheckType([MarshalAs(UnmanagedType.LPStr)] StringBuilder checkType);
            // MTX_POS_GET_CheckType (CheckType: string[4]);

            // Bit 028
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_CheckTransitRoutingNumber")]
            public static extern void CheckTransitRoutingNumber([MarshalAs(UnmanagedType.LPStr)] StringBuilder checkTransitRoutingNumber);
            // MTX_POS_GET_CheckTransitRoutingNumber (CheckTransitRoutingNumber: string[9]);

            // Bit 029
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_CheckAccountNumber")]
            public static extern void CheckAccountNumber([MarshalAs(UnmanagedType.LPStr)] StringBuilder checkAccountNumber);
            // MTX_POS_GET_CheckAccountNumber (CheckAccountNumber: string[22]);

            // Bit 030
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_CheckNumber")]
            public static extern void CheckNumber([MarshalAs(UnmanagedType.LPStr)] StringBuilder checkNumber);
            // MTX_POS_GET_CheckNumber (CheckNumber: string[8]);

            // Bit 031
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_PrimaryIDType")]
            public static extern void PrimaryIDType([MarshalAs(UnmanagedType.LPStr)] StringBuilder primaryIDType);
            // MTX_POS_GET_PrimaryIDType (PrimaryIDType: string[4]);

            // Bit 032
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_PrimaryID")]
            public static extern void PrimaryID([MarshalAs(UnmanagedType.LPStr)] StringBuilder primaryID);
            // MTX_POS_GET_PrimaryID (PrimaryID: string[40]);

            // Bit 033
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_StateCode")]
            public static extern void StateCode([MarshalAs(UnmanagedType.LPStr)] StringBuilder stateCode);
            // MTX_POS_GET_StateCode (StateCode: string[4]);

            // Bit 034
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_DateOfBirth")]
            public static extern void DateOfBirth([MarshalAs(UnmanagedType.LPStr)] StringBuilder dateOfBirth);
            // MTX_POS_GET_DateOfBirth (DateOfBirth: string[6]);

            // Bit 035
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_PIN")]
            public static extern void PIN([MarshalAs(UnmanagedType.LPStr)] StringBuilder pin);
            // MTX_POS_GET_PIN (PIN: string[16]);

            // Bit 036
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_NewPIN")]
            public static extern void NewPin([MarshalAs(UnmanagedType.LPStr)] StringBuilder newPin);
            // MTX_POS_GET_NewPIN (NewPIN: string[16]);

            // Bit 037
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_PostTransactionNumber")]
            public static extern void PostTransactionNumber([MarshalAs(UnmanagedType.LPStr)] StringBuilder postTransactionNumber);
            // MTX_POS_GET_PostTransactionNumber (PostTransactionNumber: string[22]);

            // Bit 038
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ManualEntryMICRFlag")]
            public static extern void ManualEntryMICRFlag(ref bool manualEntryMICRFlag);
            // MTX_POS_GET_ManualEntryMICRFlag(ManualEntryMICRFlag: Boolean);

            // Bit 039
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ManualEntryTrack2Flag")]
            public static extern void ManualEntryTrack2Flag(ref bool manualEntryTrack2Flag);
            // MTX_POS_GET_ManualEntryTrack2Flag(ManualEntryTrack2Flag: Boolean);

            // Bit 040
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ManualEntryIDFlag")]
            public static extern void ManualEntryIDFlag(ref bool manualEntryIDFlag);
            // MTX_POS_GET_ManualEntryIDFlag(ManualEntryIDFlag: Boolean);

            // Bit 041
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_VelocityLinesCount")]
            public static extern void VelocityLinesCount(ref int velocityLinesCount);
            // MTX_POS_GET_VelocityLinesCount(VelocityLinesCount: integer);

            // Bit 042
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_VelocityLine")]
            public static extern void VelocityLine([MarshalAs(UnmanagedType.LPStr)] StringBuilder velocityLine);
            // MTX_POS_GET_VelocityLine(VelocityLine: string[40])

            // Bit 043
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ReceiptCustomerLinesCount")]
            public static extern void ReceiptCustomerLinesCount(ref int receiptCustomerLinesCount);
            // MTX_POS_GET_ReceiptCustomerLinesCount(ReceiptCustomerLinesCount: integer);


            // Bit 044
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ReceiptCustomerLine")]
            public static extern void ReceiptCustomerLine(int receiptCutomerLineNo, [MarshalAs(UnmanagedType.LPStr)] StringBuilder receiptCutomerLine);
            // MTX_POS_GET_ReceiptCustomerLine(ReceiptCutomerLineNo: integer, ReceiptCutomerLine: string[40])

            // Bit 045
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ReceiptDrawerLinesCount")]
            public static extern void ReceiptDrawerLinesCount(ref int receiptDrawerLinesCount);
            // MTX_POS_GET_ReceiptDrawerLinesCount(ReceiptDrawerLinesCount: integer);

            // Bit 046
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ReceiptDrawerLine")]
            public static extern void ReceiptDrawerLine(int receiptDrawerLineNo, [MarshalAs(UnmanagedType.LPStr)] StringBuilder receiptDrawerLine);
            // MTX_POS_GET_ReceiptDrawerLine(ReceiptDrawerLineNo: integer, ReceiptDrawerLine: string[40])

            // Bit 047
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_UPCCode")]
            public static extern void UPCCode([MarshalAs(UnmanagedType.LPStr)] StringBuilder upcCode);
            // MTX_POS_GET_UPCCode(UPCCode: string[14])

            // Bit 048
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_PONumber")]
            public static extern void PONumber([MarshalAs(UnmanagedType.LPStr)] StringBuilder poNumber);
            // MTX_POS_GET_PONumber (PONumber: string[12]);

            // Bit 049
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_CashierDisplay1")]
            public static extern void CashierDisplay1([MarshalAs(UnmanagedType.LPStr)] StringBuilder cashierDisplay1);
            // MTX_POS_GET_CashierDisplay1 (CashierDisplay1: string[20]);

            // Bit 050
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_CashierDisplay2")]
            public static extern void CashierDisplay2([MarshalAs(UnmanagedType.LPStr)] StringBuilder cashierDisplay2);
            // MTX_POS_GET_CashierDisplay2 (CashierDisplay2: string[20]);

            // Bit 051
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_CustomerDisplay1")]
            public static extern void CustomerDisplay1([MarshalAs(UnmanagedType.LPStr)] StringBuilder customerDisplay1);
            // MTX_POS_GET_CustomerDisplay1 (CustomerDisplay1: string[20]);

            // Bit 052
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_CustomerDisplay2")]
            public static extern void CustomerDisplay2([MarshalAs(UnmanagedType.LPStr)] StringBuilder customerDisplay2);
            // MTX_POS_GET_CustomerDisplay2 (CustomerDisplay2: string[20]);

            // Bit 053
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_TransactionType")]
            public static extern void TransactionType(ref int transactionType);
            // MTX_POS_GET_TransactionType(TransactionType: integer);

            // Bit 054
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_TransactionDate")]
            public static extern void TransactionDate([MarshalAs(UnmanagedType.LPStr)] StringBuilder transactionDate);
            // MTX_POS_GET_TransactionDate (TransactionDate: string[6]);

            // Bit 055
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_TransactionTime")]
            public static extern void TransactionTime([MarshalAs(UnmanagedType.LPStr)] StringBuilder transactionTime);
            // MTX_POS_GET_TransactionTime (TransactionTime: string[6]);

            // it 056
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_TrainingTransaction")]
            public static extern void TrainingTransaction(ref bool trainingTransaction);
            // MTX_POS_GET_TrainingTransaction(TrainingTransaction: Boolean);

            /// <summary>
            /// True is returned, the POST MUST print a receipt. 
            /// Receipt requirements are described in the Receipt Addendum or the POST 
            /// can use the MTXEPS generated receipts by using the appropriate function calls.
            /// </summary>
            // Bit 057
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ReceiptRequired")]
            public static extern void ReceiptRequired(ref bool receiptRequired);
            // MTX_POS_GET_ReceiptRequired(ReceiptRequired: Boolean);

            // Bit 058
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_UserField3")]
            public static extern void UserField3([MarshalAs(UnmanagedType.LPStr)] StringBuilder userField3);
            // MTX_POS_GET_UserField3(UserField3: string[10])

            // Bit 059
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_UserField4")]
            public static extern void UserField4([MarshalAs(UnmanagedType.LPStr)] StringBuilder userField4);
            // MTX_POS_GET_UserField4(UserField4: string[8])

            // Bit 060
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_DiagnosticLinesCount")]
            public static extern void DiagnosticLinesCount(ref int diagnosticLinesCount);
            // MTX_POS_GET_DiagnosticLinesCount(DiagnosticLinesCount: integer);

            // Bit 061
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_DiagnosticLine")]
            public static extern void DiagnosticLine([MarshalAs(UnmanagedType.LPStr)] StringBuilder diagnosticLine);
            // MTX_POS_GET_DiagnosticLine(DiagnosticLine: string[20])

            // Bit 062
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_SecondaryIDType")]
            public static extern void SecondaryIDType([MarshalAs(UnmanagedType.LPStr)] StringBuilder secondaryIDType);
            // MTX_POS_GET_SecondaryIDType(SecondaryIDType: string[4])

            // Bit 063
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_SecondaryID")]
            public static extern void SecondaryID([MarshalAs(UnmanagedType.LPStr)] StringBuilder secondaryID);
            // MTX_POS_GET_SecondaryID(SecondaryID: string[40])

            // Bit 064
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_CashierOKCashback")]
            public static extern void CashierOKCashback(ref bool cashierOKCashback);
            // MTX_POS_GET_CashierOKCashback(CashierOKCashback: Boolean);

            // Bit 065
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_FleetData")]
            public static extern void FleetData([MarshalAs(UnmanagedType.LPStr)] StringBuilder fleetData);
            // MTX_POS_GET_FleetData(FleetData: string[500])

            // Bit 066
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_Odometer")]
            public static extern void Odometer([MarshalAs(UnmanagedType.LPStr)] StringBuilder odometer);
            // MTX_POS_GET_Odometer(Odometer: string[10])

            // Bit 067
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_VehicleID")]
            public static extern void VehicleID([MarshalAs(UnmanagedType.LPStr)] StringBuilder vehicleID);
            // MTX_POS_GET_VehicleID(FleetData: string[20])

            // Bit 068
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_DriverID")]
            public static extern void DriverID([MarshalAs(UnmanagedType.LPStr)] StringBuilder driverID);
            // MTX_POS_GET_DriverID(DriverID: string[20])

            // Bit 069
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ProductRestrictionCode")]
            public static extern void ProductRestrictionCode(ref int productRestrictionCode);
            // MTX_POS_GET_ProductRestrictionCode(ProductRestrictionCode: integer);

            // Bit 070
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_POSEnteredRestrictionCode")]
            public static extern void POSEnteredRestrictionCode([MarshalAs(UnmanagedType.LPStr)] StringBuilder posEnteredRestrictionCode);
            // MTX_POS_GET_POSEnteredRestrictionCode(POSEnteredRestrictionCode: string[20])

            // Bit 071
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_MaxFleetProducts")]
            public static extern void MaxFleetProducts(ref int maxFuelItems, int maxNonFuelItems, int maxTotalItems);
            // MTX_POS_GET_MaxFleetProducts(MaxFuelItems: integer; MaxNonFuelItems: integer; MaxTotalItems: integer);

            // Bit 072
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ItemExpDate")]
            public static extern void ItemExpDate([MarshalAs(UnmanagedType.LPStr)] StringBuilder itemExpDate);
            // MTX_POS_GET_ItemExpDate(ItemExpDate: string[6])

            // Bit 073
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_Account2Balance")]
            public static extern void Account2Balance(ref int account2Balance);
            // MTX_POS_GET_Account2Balance (Account2Balance: integer);

            // Bit 074
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_PhoneNumber")]
            public static extern void PhoneNumber([MarshalAs(UnmanagedType.LPStr)] StringBuilder phoneNumber);
            // MTX_POS_GET_PhoneNumber(PhoneNumber: string[32])

            // Bit 075
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_SignatureCapture")]
            public static extern void SignatureCapture([MarshalAs(UnmanagedType.LPStr)] StringBuilder signatureCaptureData, int dataLength);
            // MTX_POS_GET_SignatureCapture(SignatureCaptureData: string[500], DataLength: integer)

            // Bit 078
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ERCRequired")]
            public static extern void ERCRequired(ref int ercRequired);
            // MTX_POS_GET_ERCRequired(ERCRequired: integer);

            // Bit 079
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ReceiptHeaderCount")]
            public static extern void ReceiptHeaderCount(ref int receiptHeaderCount);
            // MTX_POS_GET_ReceiptHeaderCount(ReceiptHeaderCount: integer);

            // Bit 080
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ReceiptHeaderLine")]
            public static extern void ReceiptHeaderLine(int lineId, [MarshalAs(UnmanagedType.LPStr)] StringBuilder line);
            // MTX_POS_GET_ReceiptHeaderLine(LineID: Integer; ReceiptHeaderLine: string[40])

            // Bit 081
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ReceiptTrailCount")]
            public static extern void ReceiptTrailCount(ref int receiptTrailCount);
            // MTX_POS_GET_ReceiptTrailCount(ReceiptTrailCount: integer);

            // Bit 082
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ReceiptTrailLine")]
            public static extern void ReceiptTrailLine(int lineId, [MarshalAs(UnmanagedType.LPStr)] StringBuilder line);
            // MTX_POS_GET_ReceiptTrailLine(LineID: Integer; ReceiptTrailLine: string[40])

            // Bit 083
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_MICRFields")]
            public static extern void MICRFields([MarshalAs(UnmanagedType.LPStr)] StringBuilder routingNumber, string accountNumber, string checkNumber);
            // MTX_POS_GET_MICRFields(RoutingNumber: string[9], AccountNumber: string[22], CheckNumber: string[8])

            // Bit 084
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_TaxAmount")]
            public static extern void TaxAmount(ref int taxAmount);
            // MTX_POS_GET_TaxAmount(TaxAmount: integer);

            // Bit 085
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_RawMICR")]
            public static extern void RawMICR(byte[] rawMICR, int micrLength);
            // MTX_POS_GET_RawMICR(ARawMICR : Array100; Asize : Integer)

            // Bit 086 - ***RESERVED FOR FUTURE USE***
            // [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_CheckImage")]
            // public static extern void checkImage(byte[] checkImage, int checkImageLength);
            // MTX_POS_GET_CheckImage(AcheckImage : Array500; Asize : Integer)

            // Bit 087
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_SignatureCaptureOPOS")]
            public static extern void SignatureCaptureOPOS(byte[] signatureCaptureData, int signatureCaptureDataLength);
            // MTX_POS_GET_SignatureCaptureOPOS(SignatureCaptureData :  Array2000; Asize : Integer)

            // Bit 088
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_AmountChangeAllowed")]
            public static extern void AmountChangeAllowed(ref bool amountChangeAllowed);
            // MTX_POS_GET_AmountChangeAllowed(AmountChangeAllowed: Boolean);

            /// <summary>
            /// The MTX_POS_GET_ApprovedAmount function is used by the POS to determine the actual value 
            /// approved by the host for the current transaction.
            /// </summary>
            /// <param name="approvedAmount">The approved amount is an integer with an assumed decimal place for cents.
            /// For example, a value of 123 indicates $1.23.</param>
            // Bit 089
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ApprovedAmount")]
            public static extern void ApprovedAmount(ref int approvedAmount);
            // MTX_POS_GET_ApprovedAmount(ApprovedAmount: integer);

            // Bit 090
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_POSVerifyCard")]
            public static extern void POSVerifyCard(ref bool posVerifyCard);
            // MTX_POS_GET_POSVerifyCard(VerifyCard: Boolean);

            // Bit 091
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_BioDOB")]
            public static extern void BioDOB([MarshalAs(UnmanagedType.LPStr)] StringBuilder bioDOB);
            // MTX_POS_GET_BioDOB(BioDOB: string[8])

            // Bit 091
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_BioFreqShop")]
            public static extern void BioFreqShop([MarshalAs(UnmanagedType.LPStr)] StringBuilder bioFreqShop);
            // MTX_POS_GET_BioFreqShop(BioFreqShop: string[40])

            // Bit 091
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ZipCode")]
            public static extern void ZipCode([MarshalAs(UnmanagedType.LPStr)] StringBuilder zipCode);
            // MTX_POS_GET_ZipCode(ZipCode: string[10])

            // Bit 092
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_FuelAmount")]
            public static extern void FuelAmount(ref int fuelAmount);
            // MTX_POS_GET_FuelAmount(FuelAmount: integer);

            // Bit 093
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_CVV2")]
            public static extern void CVV2([MarshalAs(UnmanagedType.LPStr)] StringBuilder cvv2);
            // MTX_POS_GET_CVV2(CVV2: string[9])

            // Bit 094
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ControlNumber")]
            public static extern void ControlNumber([MarshalAs(UnmanagedType.LPStr)] StringBuilder controlNumber);
            // MTX_POS_GET_ControlNumber(ControlNumber: string[20])

            // Bit 095
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_WirelessPIN")]
            public static extern void WirelessPIN([MarshalAs(UnmanagedType.LPStr)] StringBuilder wirelessPIN);
            // MTX_POS_GET_WirelessPIN(WirelessPIN: string[20])

            // Bit 096
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_POSVerifyCustomerName")]
            public static extern void PosVerifyCustomerName(ref bool posVerifyCustomerName);
            // MTX_POS_GET_POSVerifyCustomerName(VerifyCustomerName: Boolean);

            // Bit 108
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_eWICRx")]
            public static extern void eWicRx(ref int rxLength, string rxData, string merchantDiscount, string stateCode);
            // MTX_POS_GET_eWICRx(RxLength: integer, RxData: string [3000], MerchantDiscount: string[20], StateCode: string [2]);

            // Bit 109
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_POSVerifyCardLast4")]
            public static extern void PosVerifyCardLast4([MarshalAs(UnmanagedType.LPStr)] StringBuilder verifyDigits);
            // MTX_POS_GET_POSVerifyCardLast4(VerifyCardLast4: string[4])

            // Bit 115
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ServerInformation")]
            public static extern void ServerInformation([MarshalAs(UnmanagedType.LPStr)] StringBuilder serverInformation);
            // MTX_POS_GET_ServerInformation(ServerInformation: string[1000])

            // Bit 117
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_SocialSecurityNumber")]
            public static extern void SocialSecurityNumber([MarshalAs(UnmanagedType.LPStr)] StringBuilder socialSecurityNumber);
            // MTX_POS_GET_SocialSecurityNumber(SSN: string[9])

            // Bit 118
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_PayrollCheckIssueDate")]
            public static extern void PayrollCheckIssueDate([MarshalAs(UnmanagedType.LPStr)] StringBuilder payrollCheckIssueDate);
            // MTX_POS_GET_PayrollCheckIssueDate(IssueDate: string[8])

            // Bit 119
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_CustomerNumber")]
            public static extern void CustomerNumber([MarshalAs(UnmanagedType.LPStr)] StringBuilder payrollCheckIssueDate);
            // MTX_POS_GET_CustomerNumber(CustomerNumber: string[40])
            #endregion

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_AccountBalanceAvailable")]
            public static extern void AccountBalanceAvailable(ref bool accountBalanceAvailable);
            // MTX_POS_GET_AccountBalanceAvailable(AccountBalanceAvailable: Boolean)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_BioCustomerName")]
            public static extern void BioCustomerName([MarshalAs(UnmanagedType.LPStr)] StringBuilder bioCustomerName);
            // MTX_POS_GET_BioCustomerName(BioCustomerName: string[40])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_BioDriversLicNumber")]
            public static extern void BioDriversLicNumber([MarshalAs(UnmanagedType.LPStr)] StringBuilder bioDriversLicNumber);
            // MTX_POS_GET_BioDriversLicNumber(BioDriversLicNumber: string[40])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_BioDriversLicStateCode")]
            public static extern void BioDriversLicStateCode([MarshalAs(UnmanagedType.LPStr)] StringBuilder bioDriversLicStateCode);
            // MTX_POS_GET_BioDriversLicStateCode(BioDriversLicStateCode: string[4])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_BioMetricsTran")]
            public static extern void BioMetricsTran(ref bool bioMetricsTran);
            // MTX_POS_GET_BioMetricsTran(aBioTran: Boolean)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_CardEntryType")]
            public static extern void CardEntryType([MarshalAs(UnmanagedType.LPStr)] StringBuilder cardEntryType);
            // MTX_POS_GET_CardEntryType(EntryType: string[4])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ComboCard")]
            public static extern void ComboCard(ref bool comboCard);
            // MTX_POS_GET_ComboCard(ComboCard: Boolean)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_CurrentWorkingKey0")]
            public static extern void CurrentWorkingKey0([MarshalAs(UnmanagedType.LPStr)] StringBuilder currentWorkingKey0);
            // MTX_POS_GET_CurrentWorkingKey0(WorkingKey: string[16])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_CustomerInformation")]
            public static extern void CustomerInformation([MarshalAs(UnmanagedType.LPStr)] StringBuilder customerName, string customerAddress, string customerCity, string customerState, string customerZip, string customerPhoneNumber, string customerEmail, string promotionCode);
            // MTX_POS_GET_CustomerInformation(CustomerName: string[40]; CustomerAddress: string[40]; CustomerCity: string[40]; CustomerState: string[4]; CustomerZip: string[10]; CustomerPhoneNumber: string[20]; CustomerEMail: string[40]; PromotionCode: string[10])

            /// <summary>
            /// Used to determine if the customer has approved the transaction
            /// </summary>
            /// <param name="customerOK">
            /// 0 - LOOP
            /// 1 - YES
            /// 2 - No
            /// 3 - CANCEL
            /// 4 - ERROR</param>
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_CustomerOK")]
            public static extern void CustomerOK(ref int customerOK);
            // MTX_POS_GET_CustomerOK (CustomerOK: Integer);

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_DataEntryStatus")]
            public static extern void DataEntryStatus(ref int dataEntryStatus);
            // MTX_POS_GET_DataEntryStatus(Status: Integer);

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_DataEntryType")]
            public static extern void DataEntryType(ref int dataEntryType);
            // MTX_POS_GET_DataEntryType(Status: Integer);

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_DataEntryValue")]
            public static extern void DataEntryValue([MarshalAs(UnmanagedType.LPStr)] StringBuilder dataEntryValue);
            // MTX_POS_GET_DataEntryValue(Value: string[40])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_DataEntryValue2")]
            public static extern void DataEntryValue2([MarshalAs(UnmanagedType.LPStr)] StringBuilder dataEntryValue);
            // MTX_POS_GET_DataEntryValue2(Value: string[80])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_SET_DiscountAmount")]
            public static extern void DiscountAmount([MarshalAs(UnmanagedType.LPStr)] StringBuilder discountAmount);
            // MTX_POS_SET_DiscountAmount(TypeAndAmount: string[10])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_EPSConnectionStatus")]
            public static extern void EPSConnectionStatus(ref int epsConnectionStatus);
            // MTX_POS_GET_EPSConnectionStatus(Status: Integer);

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_EPSVersionNo")]
            public static extern void EPSVersionNumber([MarshalAs(UnmanagedType.LPStr)] StringBuilder epsVersionNumber);
            // MTX_POS_GET_EPSVersionNo(EPSVerNumber: string[14])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ExpirationDateMasked")]
            public static extern void ExpirationDateMasked([MarshalAs(UnmanagedType.LPStr)] StringBuilder expirationDateMasked);
            // MTX_POS_GET_ExpirationDateMasked(ExpirationDate: string[4])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ExtendedCashierPrompts")]
            public static extern void ExtendedCashierPrompts(string[] extendedCashierPrompts);
            // MTX_POS_GET_ExtendedCashierPrompts(line1,line2,line3,line4,line5: array[40])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_Extended_DataEntryValue")]
            public static extern void DataEntryValue(ref int rxAck, ref int signatureLength, [MarshalAs(UnmanagedType.LPStr)] StringBuilder signatureData);
            // MTX_POS_GET_Extended_DataEntryValue(RxACK: integer, SignatureLength: integer, SignatureData: string[2000])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_FieldPending_NonF")]
            public static extern void FieldPending_NonF([MarshalAs(UnmanagedType.LPStr)] StringBuilder fieldsRequired);
            // MTX_POS_GET_FieldPending_NonF(FieldPending_NonF: string[32])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_FieldPendingi_NonF")]
            public static extern void FieldPendingi_NonF(ref int fieldPendingi_NonF);
            // MTX_POS_GET_FieldPendingi_NonF(FieldPending_NonF: string[4])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_FieldsRequired")]
            public static extern void FieldsRequired([MarshalAs(UnmanagedType.LPStr)] StringBuilder fieldsRequired);
            // MTX_POS_GET_FieldsRequired(FieldsRequired: string[32])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_FieldsRequiredi")]
            public static extern void FieldsRequiredi(ref int fieldsRequiredi);
            // MTX_POS_GET_FieldsRequiredi(FieldsRequiredi: integer[4])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_FrequentShopperData")]
            public static extern void FrequentShopperData([MarshalAs(UnmanagedType.LPStr)] StringBuilder frequentShopperData);
            // MTX_POS_GET_FrequentShopperData(Value: string[40])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_HostSlotNumber")]
            public static extern void HostSlotNumber([MarshalAs(UnmanagedType.LPStr)] StringBuilder hostSlotNumber);
            // MTX_POS_GET_HostSlotNumber(HostSlotNumber: string[3])

            /// <summary>
            /// HostResponseCode is the code sent to WinEPS by the host (payments processor) 
            /// that indicates an approval or decline of a transaction.
            /// </summary>
            /// <param name="hostResponseCode"></param>
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_HostResponseCode")]
            public static extern void HostResponseCode([MarshalAs(UnmanagedType.LPStr)] StringBuilder hostResponseCode);
            // MTX_POS_GET_HostResponseCode(HostResponseCode: string[6])

            /// <summary>
            /// To determine when the host has returned a transaction response, the POST must loop on
            /// MTX_POS_GET_HOSTStatus. While looping the POS should call this function every 250ms.When
            /// MTX_POS_GET_HOSTStatus returns Done, the POST may move on.
            /// </summary>
            /// <param name="hostStatus">0 - LOOP
            /// 1 - Done</param>
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_HOSTStatus")]
            public static extern void HostStatus(ref int hostStatus);
            // MTX_POS_GET_HOSTStatus(HOSTStatus: integer[4])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_HOSTStatus_NonF")]
            public static extern void HostStatus_NonF(ref int hostStatus_NonF);
            // MTX_POS_GET_HOSTStatus_NonF(HOSTStatus: integer)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_IsCardFSA")]
            public static extern void IsCardFSA(ref bool isCardFSA);
            // MTX_POS_GET_IsCardFSA(IsCardFSA: Boolean)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_IsOTC")]
            public static extern void IsOTC(ref bool IsOTC);
            // MTX_POS_GET_IsOTC(IsOTC: Boolean)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_LaneStandInAllowed")]
            public static extern void LaneStandInAllowed(ref bool laneStandInAllowed);
            // MTX_POS_GET_LaneStandInAllowed(aAllowed: Boolean)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_LanguageID")]
            public static extern void LanguageID([MarshalAs(UnmanagedType.LPStr)] StringBuilder languageID);
            // MTX_POS_GET_LanguageID(LanguageID: string[1])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_MICRStatus")]
            public static extern void MICRStatus(ref int micrStatus);
            // MTX_POS_GET_MICRStatus(MICRStatus: integer)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_P2PCallBackUsed")]
            public static extern void P2PCallBackUsed(ref bool p2pCallBackUsed);
            // MTX_POS_GET_P2PCallBackUsed(P2PCallBackUsed: Boolean)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_P2PDLL")]
            public static extern void IsP2PDll(ref bool isP2PDll);
            // MTX_POS_GET_P2PDLL(P2PStatus: Boolean)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_P2PEncrypted")]
            public static extern void IsP2PEncrypted(ref bool isP2PEncrypted);
            // MTX_POS_GET_P2PEncrypted(P2PTransactionStatus: Boolean)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_PanHash")]
            public static extern void PANHash([MarshalAs(UnmanagedType.LPStr)] StringBuilder panHash);
            // MTX_POS_GET_PANHash(AHashAcct: string[40])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_PanHashSHA256")]
            public static extern void PANHashSHA256([MarshalAs(UnmanagedType.LPStr)] StringBuilder panHashSHA256);
            // MTX_POS_GET_PANHashSHA256(AHashAcct: string[64])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_PersonalAccountNumberMasked")]
            public static extern void PersonalAccountNumberMasked([MarshalAs(UnmanagedType.LPStr)] StringBuilder personalAccountNumberMasked);
            // MTX_POS_GET_PersonalAccountNumberMasked(AccountNumber: string[22])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_POSTVersion")]
            public static extern void POSTVersion([MarshalAs(UnmanagedType.LPStr)] StringBuilder postVersion);
            // MTX_POS_GET_POSTVersion(POSTVersion: string[45])

            /// <summary>
            /// Gets the WIC prescription from the card to tell POS what items are allowed for WIC
            /// </summary>
            /// <param name="date"> yyyymmdd format</param>
            /// <param name="time">hhmmss format</param>
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_Prescription")]
            public static extern void Prescription([MarshalAs(UnmanagedType.LPStr)] StringBuilder date, [MarshalAs(UnmanagedType.LPStr)] StringBuilder time);
            // MTX_POS_GET_Prescription(Date [String 8; yyyymmdd], Time [String 6; hhmmss])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ProgramID")]
            public static extern void ProgramID([MarshalAs(UnmanagedType.LPStr)] StringBuilder programID);
            // MTX_POS_GET_ProgramID(ProgramID: string[5])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ReadyStatus_NonF")]
            public static extern void ReadyStatus_NonF(ref int readyStatus_NonF);
            // MTX_POS_GET_ReadyStatus_NonF(ReadyStatus: integer[4])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ReceiptRequired_NonF")]
            public static extern void ReceiptRequired_NonF(ref bool receiptRequired_NonF);
            // MTX_POS_GET_ReceiptRequired_NonF(ReceiptRequired: Boolean)

            /// <summary>
            /// The outcome of the transaction is determined by the ResponseCode field.
            /// </summary>
            /// <param name="responseCode_NonF">Response codes are broadly divided into Approvals and Declines. 
            /// Response Codes that begin with an ‘A’ are approvals and response codes that begin with an ‘N’ are declines. 
            /// The declines are further divided into ‘Hard’ declines and ‘Soft’ declines.</param>
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ResponseCode_NonF")]
            public static extern void ResponseCode_NonF([MarshalAs(UnmanagedType.LPStr)] StringBuilder responseCode_NonF);
            // MTX_POS_GET_ResponseCode_NonF(ResponseCode: string[4])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_ReturnCheckNote")]
            public static extern void ReturnCheckNote([MarshalAs(UnmanagedType.LPStr)] StringBuilder returnCheckNote);
            // MTX_POS_GET_ReturnCheckNote(ReturnCheckNote: string[100])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_RFID")]
            public static extern void RFID([MarshalAs(UnmanagedType.LPStr)] StringBuilder rfid);
            // MTX_POS_GET_RFID(TrackDataSource: string[1])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_SCATFieldPending")]
            public static extern void ScatFieldPending([MarshalAs(UnmanagedType.LPStr)] StringBuilder scatFieldPending);
            // MTX_POS_GET_SCATFieldPending(MTX_POS_GET_SCATFieldPending: string[32])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_SCATFieldPendingi")]
            public static extern void ScatFieldPendingi(ref int scatFieldPendingi);
            // MTX_POS_GET_SCATFieldPendingi(SCATFieldPendingi: integer[4])

            /// <summary>
            /// The POST must call MTX_POS_GET_SCATReady to determine when OpenEPS is ready. The POST
            /// should loop on this call each time it returns “Loop” (Value: 0); when the function returns “Ready” (Value:1) 
            /// the POS should move onto the next function call. While looping the POS should call this function every
            /// 250ms.If SCATReady does not return Ready (Value: 1) prior to the point where the POS is ready to tender, the
            /// POS must wait until Ready (Value: 1) before proceeding on with the Transaction Sequence.
            /// </summary>
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_SCATReady")]
            public static extern void SCATReady(ref int scatReady);
            // MTX_POS_GET_SCATReady(SCATReady: integer)

            /// <summary>
            /// Once the transaction type has been set, the MTX_POS.DLL will complete its acquisition of information
            /// from the SCAT. During this time the POST should loop on MTX_POS_GET_SCATStatus. While looping
            /// the POS should call this function every 250ms.
            /// </summary>
            /// <param name="scatStatus">0-WAIT,
            /// 1 - DONE,
            /// 2 - MANUAL
            /// 3 - CANCEL
            /// 4 - FATAL ERROR
            /// 5 - CHECK MICR STATUS
            /// 6 - DONE-MANUAL NOT ALLOWED</param>
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_SCATStatus")]
            public static extern void SCATStatus(ref int scatStatus);
            // MTX_POS_GET_SCATStatus(SCATStatus: integer)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_SCATStatus_NonF")]
            public static extern void SCATStatus_NonF(ref int scatStatus_NonF);
            // MTX_POS_GET_SCATStatus_NonF(SCATStatus: integer)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_SCATTrackStatus")]
            public static extern void MTX_POS_GET_SCATTrackStatus(ref int scatTrackStatus);
            // MTX_POS_GET_SCATTrackStatus(TrackStatus: integer)

            /// <summary>
            /// The function to check the card is removed or not.
            /// </summary>
            /// <param name="swCardRemoved">If the card is removed then the value will be true.</param>
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_SWCardRemoved")]
            public static extern void SWCardRemoved(ref bool swCardRemoved);
            // MTX_POS_GET_SWCardRemoved(CardRemoved: Boolean)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_TenderTypeMTX")]
            public static extern void TenderTypeMtx([MarshalAs(UnmanagedType.LPStr)] StringBuilder tenderTypeMtx);
            // MTX_POS_GET_TenderTypeMTX(TenderTypeMTX: string[32])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_TenderTypeMTXi")]
            public static extern void TenderTypeMtxi(ref int tenderTypeMtxi);
            // MTX_POS_GET_TenderTypeMTXi(TenderTypeMTXi: integer[4])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_TenderType_NonF")]
            public static extern void TenderType_NonF(ref int TenderType_NonF);
            // MTX_POS_GET_TenderType_NonF(TenderType_NonF: integer)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_TenderTypePOS")]
            public static extern void TenderTypePOS([MarshalAs(UnmanagedType.LPStr)] StringBuilder tenderTypePos);
            // MTX_POS_GET_TenderTypePOS(TenderTypePOS: string[32])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_TenderTypePOSi")]
            public static extern void TenderTypePOSi(ref int tenderTypePOSi);
            // MTX_POS_GET_TenderTypePOSi(TenderTypePOSi: integer[4])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_TenderTypeStatus")]
            public static extern void TenderTypeStatus(ref int tenderTypeStatus);
            // MTX_POS_GET_TenderTypeStatus(TenderTypeStatus: integer)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_TenderTypesUsed_NonF")]
            public static extern void TenderTypesUsed_NonF([MarshalAs(UnmanagedType.LPStr)] StringBuilder TenderTypesUsed_NonF);
            // MTX_POS_GET_TenderTypesUsed_NonF(Tender types used: string[32])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_TenderTypesUsedi_NonF")]
            public static extern void TenderTypesUsedi_NonF(ref int tenderTypesUsedi_NonF);
            // MTX_POS_GET_TenderTypesUsedi_NonF(Tender types used: integer[4])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_TerminalId")]
            public static extern void TerminalID([MarshalAs(UnmanagedType.LPStr)] StringBuilder terminalID);
            // MTX_POS_GET_TerminalId(TerminalId: string[16])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_TicketLookupResponse")]
            public static extern void TicketLookupResponse([MarshalAs(UnmanagedType.LPStr)] StringBuilder terminalID);
            // MTX_POS_GET_TicketLookupResponse(ResponseXMS: string35000)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_Track1DataMasked")]
            public static extern void Track1DataMasked([MarshalAs(UnmanagedType.LPStr)] StringBuilder track1DataMasked);
            // MTX_POS_GET_Track1DataMasked(Track1Data: string[80])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_Track2DataMasked")]
            public static extern void Track2DataMasked([MarshalAs(UnmanagedType.LPStr)] StringBuilder track2DataMasked);
            // MTX_POS_GET_Track2DataMasked(Track2Data: string[44])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_TransactionGUID")]
            public static extern void TransactionGUID([MarshalAs(UnmanagedType.LPStr)] StringBuilder transactionGUID);
            // MTX_POS_GET_TransactionGUID(GUID: string[32])

            /// <summary>
            /// This function allows the POS to get the value OpenEPS is using for a timeout so the 
            /// POS can set its own timeout accordingly instead of relying on a hard coded value.
            /// </summary>
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_TransactionTimeout")]
            public static extern void TransactionTimeout(ref int transactionTimeout);
            // MTX_POS_GET_TransactionTimeout(TimeoutSecs: integer)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_TransactionType_NonF")]
            public static extern void TransactionType_NonF(ref int transactionType_NonF);
            // MTX_POS_GET_TransactionType_NonF(TranType: integer)

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_TransactionTypesUsed_NonF")]
            public static extern void TransactionTypesUsed_NonF(ref int tenderType, [MarshalAs(UnmanagedType.LPStr)] StringBuilder nonFinancialTypes);
            // MTX_POS_GET_TransactionTypesUsed_NonF(TenderType: integer; NonFinancialTypes: string[32])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_TransactionTypesUsedi_NonF")]
            public static extern void TransactionTypesUsedi_NonF(ref int tenderType, ref int nonFinancialTypes);
            // MTX_POS_GET_TransactionTypesUsedi_NonF(TenderType: integer; NonFinancialTypes: integer[4])

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_VisaTranID")]
            public static extern void VisaTranId([MarshalAs(UnmanagedType.LPStr)] StringBuilder disaTranId);
            // MTX_POS_GET_VisaTranID(VisaTranID: string[15])

            /// <summary>
            /// Asks the customer to enter their PIN and validates the PIN against security information stored on the card.
            /// </summary>
            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_WIC_Pin")]
            public static extern void WIC_PIN();
            // MTX_POS_GET_WIC_Pin()

            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_WinEPSCardType")]
            public static extern void WinEpsCardType([MarshalAs(UnmanagedType.LPStr)] StringBuilder winEpsCardType);
            // MTX_POS_GET_WinEPSCardType(WinEPSCardType: string[18])


            [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "MTX_POS_GET_TokenData")]
            public static extern void TokenData([MarshalAs(UnmanagedType.LPStr)] StringBuilder tokenData);
            // MTX_POS_GET_TokenData(TokenData: string[500])
        }
        #endregion
    }
}
