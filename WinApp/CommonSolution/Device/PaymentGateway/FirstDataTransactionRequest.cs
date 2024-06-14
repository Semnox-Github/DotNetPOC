using System;
using System.Collections.Generic;
using System.Text;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// FirstData Transaction Request
    /// </summary>
    public class FirstDataTransactionRequest
    {
        /// <summary>
        /// double TransactionAmount
        /// </summary>
        public double TransactionAmount;
        /// <summary>
        ///  double TipAmount
        /// </summary>
        public double TipAmount;
        /// <summary>
        /// string STAN
        /// </summary>
        public string STAN;
        /// <summary>
        /// string ReferenceNo
        /// </summary>
        public string ReferenceNo;
        /// <summary>
        /// string LocalDateTime
        /// </summary>
        public string LocalDateTime;
        /// <summary>
        /// string GMTTranDateTime
        /// </summary>
        public string GMTTranDateTime;
        /// <summary>
        /// string OrigResponseCode
        /// </summary>
        public string OrigResponseCode;
        /// <summary>
        /// string OrigAuthID
        /// </summary>
        public string OrigAuthID;
        /// <summary>
        /// string OrigDatetime
        /// </summary>
        public string OrigDatetime;
        /// <summary>
        /// string OrigGMTDatetime
        /// </summary>
        public string OrigGMTDatetime;
        /// <summary>
        ///  string OrigSTAN
        /// </summary>
        public string OrigSTAN;
        /// <summary>
        /// string OrigRef
        /// </summary>
        public string OrigRef;
        /// <summary>
        /// string OrigAccNo
        /// </summary>
        public string OrigAccNo;
        /// <summary>
        /// string OrigTransactionType
        /// </summary>
        public string OrigTransactionType;
        /// <summary>
        /// string OrigToken
        /// </summary>
        public string OrigToken;
        /// <summary>
        /// string OrigCardName
        /// </summary>
        public string OrigCardName;
        /// <summary>
        /// string OrigGroupData
        /// </summary>
        public string OrigGroupData;
        /// <summary>
        ///  string OrigCardExpiryDate
        /// </summary>
        public string OrigCardExpiryDate;
        /// <summary>
        /// double OrigTransactionAmount
        /// </summary>
        public double OrigTransactionAmount;
        /// <summary>
        ///  int OrigPaymentId
        /// </summary>
        public int OrigPaymentId;
        /// <summary>
        /// string TransactionType
        /// </summary>
        public string TransactionType;
        /// <summary>
        ///  bool isCredit
        /// </summary>
        public bool isCredit = false;
        /// <summary>
        /// bool isPrintReceipt
        /// </summary>
        public bool isPrintReceipt = false;
        /// <summary>
        ///  bool wasOrigSwiped
        /// </summary>
        public bool wasOrigSwiped = false;
        
    }
}
