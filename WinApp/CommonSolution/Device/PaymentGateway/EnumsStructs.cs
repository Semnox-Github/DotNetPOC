using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// Enum Variables for Command Response
    /// </summary>
    public enum enumCmdResponse
    {
        /// <summary>
        /// Approved
        /// </summary>
        Approved,
        /// <summary>
        /// Declined
        /// </summary>
        Declined,
        /// <summary>
        /// Success
        /// </summary>
        Success,
        /// <summary>
        /// Error
        /// </summary>
        Error
    }

    /// <summary>
    /// Enum Variables for Card Type
    /// </summary>
    public enum CardType
    {
        /// <summary>
        /// Credit
        /// </summary>
        Credit,
        /// <summary>
        /// Debit
        /// </summary>
        Debit
    }

    /// <summary>
    /// Enum variables for SimState
    /// </summary>
    public enum SimState
    {
        /// <summary>
        /// SelectCreditDebit
        /// </summary>
        SelectCreditDebit,
        /// <summary>
        /// ConfirmAmount
        /// </summary>
        ConfirmAmount,
        /// <summary>
        /// DisplayTransResult
        /// </summary>
        DisplayTransResult,
        /// <summary>
        /// DisplayThankYou
        /// </summary>
        DisplayThankYou,
        /// <summary>
        /// DisplayHandsOff
        /// </summary>
        DisplayHandsOff,
        /// <summary>
        /// ManualEntry
        /// </summary>
        ManualEntry,
        /// <summary>
        /// SimComplete
        /// </summary>
        SimComplete
    };

    /// <summary>
    /// Enum variables for command Status
    /// </summary>
    public enum enumCommandStatus
    {
        /// <summary>
        /// PGWSSuccessMessage
        /// </summary>
        PGWSSuccessMessage,
        /// <summary>
        ///PGSuccess 
        /// </summary>
        PGSuccess,
        /// <summary>
        ///PGWSorInternetError 
        /// </summary>
        PGWSorInternetError,
        /// <summary>
        /// Approved 
        /// </summary>
        Approved,
        /// <summary>
        /// Declined
        /// </summary>
        Declined,
        /// <summary>
        /// Success
        /// </summary>
        Success,
        /// <summary>
        /// Error
        /// </summary>
        Error,
        /// <summary>
        /// NewRequest
        /// </summary>
        NewRequest,
        /// <summary>
        /// WSerror
        /// </summary>
        WSError,
        /// <summary>
        /// DBRequestError
        /// </summary>
        DBRequestError,
        /// <summary>
        /// DBResponseError
        /// </summary>
        DBResponseError,
        /// <summary>
        /// DBDuplicateRecord
        /// </summary>
        DBDuplicateRecord,
        /// <summary>
        /// ErrorGettingInvoiceAmounts
        /// </summary>
        ErrorGettingInvoiceAmounts,
        /// <summary>
        /// ErrorUpdateStatus
        /// </summary>
        ErrorUpdateStatus,
        /// <summary>
        /// CardNotRead
        /// </summary>
        CardNotRead,
        /// <summary>
        /// EmptyListVoidSale
        /// </summary>
        EmptyListVoidSale,
        /// <summary>
        /// Return
        /// </summary>
        Return,
        /// <summary>
        /// InValidPrice
        /// </summary>
        InValidPrice,
        /// <summary>
        /// MissingFields
        /// </summary>
        MissingFields,
        /// <summary>
        /// IPADCancel
        /// </summary>
        IPADCancel,
        /// <summary>
        /// KBSecurity
        /// </summary>
        KBSecurity,
        /// <summary>
        /// SimDone
        /// </summary>
        SimDone,
        /// <summary>
        /// None
        /// </summary>
        None,
        /// <summary>
        /// DeviceNotConnected
        /// </summary>
        DeviceNotConnected
    }

    /// <summary>
    /// PGSEMessages Struct
    /// </summary>
    public struct PGSEMessages
    {
        /// <summary>
        /// PGWSSuccessMessage
        /// </summary>
        public static string PGWSSuccessMessage = "Payment Gateway Successful";
        /// <summary>
        /// PGApproved
        /// </summary>
        public static string PGApproved = "Approved";
        /// <summary>
        /// PGDeclined
        /// </summary>
        public static string PGDeclined = "Declined";
        /// <summary>
        /// PGSuccess
        /// </summary>
        public static string PGSuccess = "Success";
        /// <summary>
        /// PGError
        /// </summary>
        public static string PGError = "Error";
        /// <summary>
        /// PGWSError
        /// </summary>
        public static string PGWSError = "Web service or Internet is down";
        /// <summary>
        /// PGDBRequestError
        /// </summary>
        public static string PGDBRequestError = "DB Request Message Error";
        /// <summary>
        /// PGDBResponseError
        /// </summary>
        public static string PGDBResponseError = "DB Response Message Error";
        /// <summary>
        /// PGdefault
        /// </summary>
        public static string PGdefault = "default message";
        /// <summary>
        /// PGDBReqMsgInsert
        /// </summary>
        public static string PGDBReqMsgInsert = "Request Message is getting inserted into the DB";
        /// <summary>
        /// PGDBResMsgInsert
        /// </summary>
        public static string PGDBResMsgInsert = "Request Message is getting inserted into the DB";
        /// <summary>
        /// PGDBReqMsgNotAttNotset
        /// </summary>
        public static string PGDBReqMsgNotAttNotset = "Request Message attributes are not set properly, please try once again";
        /// <summary>
        /// PGDeviceRemoved
        /// </summary>
        public static string PGDeviceRemoved = "Device is removed";
        /// <summary>
        /// PGDeviceConnected
        /// </summary>
        public static string PGDeviceConnected = "Device is connected";
        /// <summary>
        /// PGDeviceNotConnected
        /// </summary>
        public static string PGDeviceNotConnected = "Device is Not connected";
        /// <summary>
        /// PGDeviceNotAuthenticated
        /// </summary>
        public static string PGDeviceNotAuthenticated = "Not Authenticated";
        /// <summary>
        /// PGDeviceAuthenticated
        /// </summary>
        public static string PGDeviceAuthenticated = "Authenticated";
        /// <summary>
        /// PGDeviceTamper
        /// </summary>
        public static string PGDeviceTamper = "Device Tamper";
        /// <summary>
        /// PGDeviceNoChanges
        /// </summary>
        public static string PGDeviceNoChanges = "No Changes";
        /// <summary>
        /// PGDeviceReady
        /// </summary>
        public static string PGDeviceReady = "Device is ready, please swipe card";
        /// <summary>
        /// PGDeviceBusy
        /// </summary>
        public static string PGDeviceBusy = "Device is busy, please see the status message on the device or Restart the device";
        /// <summary>
        /// PGDeviceRestart
        /// </summary>
        public static string PGDeviceRestart = "Device is getting restarted";
        /// <summary>
        /// PGDeviceRestarted
        /// </summary>
        public static string PGDeviceRestarted = "Device is restarted";
        /// <summary>
        /// PGCardTypeConfirm
        /// </summary>
        public static string PGCardTypeConfirm = "Confirm Card Type";
        /// <summary>
        /// PGCardTypeSelected
        /// </summary>
        public static string PGCardTypeSelected = "Confirmed card type";
        /// <summary>
        /// PGCardCancelled
        /// </summary>
        public static string PGCardCancelled = "Cancelled";
        /// <summary>
        /// PGCardReading
        /// </summary>
        public static string PGCardReading = "Reading card data";
        /// <summary>
        /// PGCardReadingError
        /// </summary>
        public static string PGCardReadingError = "Error in reading card data, please swipe again";
        /// <summary>
        /// PGCardConfirmAmount
        /// </summary>
        public static string PGCardConfirmAmount = "Confirm Amount";
        /// <summary>
        /// PGWSInvoke
        /// </summary>
        public static string PGWSInvoke = "Invoking Payment gateway";
        /// <summary>
        /// PGDeviceRestartShutdownConfirm
        /// </summary>
        public static string PGDeviceRestartShutdownConfirm = "Are you sure you want to Restart the device ? ";
        /// <summary>
        /// PGDeviceDisconnected
        /// </summary>
        public static string PGDeviceDisconnected = "Device is disconnected";
        //public static string PGPurAmountExcInvAmt = " Purchase amount exceeds the Balance amount. Are you sure you want to continue the transaction ? ";
        /// <summary>
        /// PGDBDuplicateInvoice
        /// </summary>
        public static string PGDBDuplicateInvoice = " Already Record Exists with the same Invoice Number";
        /// <summary>
        /// PGRegInvoiceExMessage
        /// </summary>
        public static string PGRegInvoiceExMessage = " Enter only Integers";
        /// <summary>
        /// PGVDInvoiceAmount
        /// </summary>
        public static string PGVDInvoiceAmount = " Invoice and Amount cannot be null or zero";
        /// <summary>
        /// PGValAmountBlank
        /// </summary>
        public static string PGValAmountBlank = " Amounts cannot be Blank ";
        /// <summary>
        /// PGRequiredFieldsBlank
        /// </summary>
        public static string PGRequiredFieldsBlank = " Required fields are missing ";
        /// <summary>
        /// PGValPurAmtformat
        /// </summary>
        public static string PGValPurAmtformat = " Purchase amount is not in the proper format. It should be in the form of : 00.00";
        /// <summary>
        /// VoidSaleTransactionStarted
        /// </summary>
        public static string VoidSaleTransactionStarted = " VoidSale transaction started ";
        /// <summary>
        /// VoidSaleoneTransucces
        /// </summary>
        public static string VoidSaleoneTransucces= " VoidSale transaction is successful ";
        /// <summary>
        /// VoidSaleNoTransactions
        /// </summary>
        public static string VoidSaleNoTransactions = " One or more VoidSale transactions are Unsuccessful ";
        /// <summary>
        /// VoidSaleNoTransactionsEmptyList
        /// </summary>
        public static string VoidSaleNoTransactionsEmptyList = " No sale transactions ";
        /// <summary>
        /// NewInvoice
        /// </summary>
        public static string NewInvoice = " New Invoice number Generated ";
        /// <summary>
        /// DebitReturn
        /// </summary>
        /// 
        public static string DebitReturn = " Please highlight the row which you want to Return the Debit transaction ";
        /// <summary>
        /// UseVoidsale
        /// </summary>
        public static string UseVoidsale = " Use VoidSale Button ";
        /// <summary>
        /// DebitRowsNotinGrid
        /// </summary>
        public static string DebitRowsNotinGrid = " No Debit transactions in the grid";
        /// <summary>
        /// MsgCustomCardSwipeReturn
        /// </summary>
        public static string MsgCustomCardSwipeReturn= " Return -  Please swipe the card : {0} ";
        /// <summary>
        /// MsgCustomCardSwipeVoidSale
        /// </summary>
        public static string MsgCustomCardSwipeVoidSale = " VoidSale - for the card : {0} ";
        /// <summary>
        /// PartialApprovalBlocked
        /// </summary>
        public static string PartialApprovalBlocked = " Since Partial Approvals are not enabled,the transaction is declined. ";//Begin Modification on 09-Nov-2015:Partial approval
        /// <summary>
        /// PartiallyApproved
        /// </summary>
        public static string PartiallyApproved = "Partially Approved";//End Modification on 09-Nov-2015:Partial approval
    }

    /// <summary>
    /// structStoredProcs Struct
    /// </summary>
    public struct structStoredProcs
    {
        /// <summary>
        /// SPInsertRequestMessageDetails
        /// </summary>
        public static string SPInsertRequestMessageDetails = "SPInsertRequestMesage";
        /// <summary>
        /// SPInsertResponseMessageDetails
        /// </summary>
        public static string SPInsertResponseMessageDetails = "SPInsertResponseMesage";
        /// <summary>
        /// SPGetInvoiceAppovedAmounts
        /// </summary>
        public static string SPGetInvoiceAppovedAmounts = "SPGetInvoiceAppovedAmounts";
        /// <summary>
        /// SPGetVoidSaleReturnInvoiceDetails
        /// </summary>
        public static string SPGetVoidSaleReturnInvoiceDetails = "SPGetVoidSaleReturnInvoiceDetails";
    }
}
