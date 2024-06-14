using Semnox.Core.GenericUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// TransactionSearchCriteria
    /// </summary>
    public class TransactionSearchCriteria : SearchCriteria
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TransactionSearchCriteria() : base(new TransactionColumnProvider())
        {

        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="columnIdentifier"></param>
        /// <param name="operator"></param>
        /// <param name="parameters"></param>
        public TransactionSearchCriteria(Enum columnIdentifier, Operator @operator, params object[] parameters) :
            base(new TransactionColumnProvider(), columnIdentifier, @operator, parameters)
        {

        }
    }
    /// <summary>
    /// TransactionSearchCriteriaParameters enum controls the search fields
    /// </summary>
    /// 
    public enum TransactionSearchCriteriaParameters
    {
        /// <summary>
        /// TRANSACTION_ID
        /// </summary>
        TRANSACTION_ID,
        /// <summary>
        /// STATUS
        /// </summary>
        STATUS,
        /// <summary>
        /// SITE_ID
        /// </summary>
        SITE_ID,
        /// <summary>
        /// ORDER_ID
        /// </summary>
        ORDER_ID,
        /// <summary>
        /// POS_TYPE_ID
        /// </summary>
        POS_TYPE_ID,
        /// <summary>
        /// POS_MACHINE_ID
        /// </summary>
        POS_MACHINE_ID,
        /// <summary>
        /// POS_NAME
        /// </summary>
        POS_NAME,
        /// <summary>
        /// TRANSACTION_OTP
        /// </summary>
        TRANSACTION_OTP,
        /// <summary>
        /// EXTERNAL_SYSTEM_REFERENCE
        /// </summary>
        EXTERNAL_SYSTEM_REFERENCE,
        /// <summary>
        /// ORIGINAL_SYSTEM_REFERENCE
        /// </summary>
        ORIGINAL_SYSTEM_REFERENCE,
        /// <summary>
        /// CUSTOMER_ID
        /// </summary>
        CUSTOMER_ID,
        /// <summary>
        /// TRANSACTION_DATE
        /// </summary>
        TRANSACTION_DATE,
        /// <summary>
        /// LAST_UPDATE_TIME
        /// </summary>
        LAST_UPDATE_TIME,
        /// <summary>
        /// TRANSACTION_NUMBER
        /// </summary>
        TRANSACTION_NUMBER,
        /// <summary>
        /// REMARKS
        /// </summary>
        REMARKS,
        /// <summary>
        /// USER_ID
        /// </summary>
        USER_ID,
        /// <summary>
        /// TRANSACTION_GUID
        /// </summary>
        TRANSACTION_GUID,
        /// <summary>
        /// ORIGINAL_TRX_ID
        /// </summary>
        ORIGINAL_TRX_ID,
        /// <summary>
        /// HEADER_PAYMENT_MODE
        /// </summary>
        HEADER_PAYMENT_MODE,
        /// <summary>
        /// TRX_PAYMENT_MODE_ID
        /// </summary>
        TRX_PAYMENT_MODE_ID,
        /// <summary>
        /// TRX_PAYMENT_ATTRIBUTE1
        /// </summary>
        TRX_PAYMENT_ATTRIBUTE1,
        /// <summary>
        /// TRX_PAYMENT_ATTRIBUTE2
        /// </summary>
        TRX_PAYMENT_ATTRIBUTE2,
        /// <summary>
        /// TRX_PAYMENT_ATTRIBUTE3
        /// </summary>
        TRX_PAYMENT_ATTRIBUTE3,
        /// <summary>
        /// TRX_PAYMENT_ATTRIBUTE4
        /// </summary>
        TRX_PAYMENT_ATTRIBUTE4,
        /// <summary>
        /// TRX_PAYMENT_ATTRIBUTE5
        /// </summary>
        TRX_PAYMENT_ATTRIBUTE5
    }
    /// <summary>
    /// TransactionColumnProvider
    /// </summary>
    public class TransactionColumnProvider : ColumnProvider
    {
        internal TransactionColumnProvider()
        {
            columnDictionary = new Dictionary<Enum, Column>() {
                {TransactionSearchCriteriaParameters.TRANSACTION_ID, new NumberColumn("trx_header.TrxId", "Transaction Id")},
                {TransactionSearchCriteriaParameters.STATUS,  new TextColumn("trx_header.Status", "Status")},
                {TransactionSearchCriteriaParameters.SITE_ID,  new NumberColumn("trx_header.site_id", "Site Id")},
                {TransactionSearchCriteriaParameters.ORDER_ID, new NumberColumn("trx_header.OrderId", "Order Id")},
                {TransactionSearchCriteriaParameters.POS_MACHINE_ID, new NumberColumn("trx_header.POSMachineId", "POS Machine Id")},
                {TransactionSearchCriteriaParameters.POS_NAME, new TextColumn("trx_header.pos_machine", "POS Machine Name")},
                {TransactionSearchCriteriaParameters.POS_TYPE_ID, new TextColumn("trx_header.POSTypeId", "POS Type Id")},
                {TransactionSearchCriteriaParameters.HEADER_PAYMENT_MODE, new TextColumn("trx_header.Payment_Mode", "Payment Mode")},
                {TransactionSearchCriteriaParameters.TRANSACTION_OTP, new TextColumn("trx_header.TransactionOTP", "Transaction OTP")},
                {TransactionSearchCriteriaParameters.EXTERNAL_SYSTEM_REFERENCE, new TextColumn("trx_header.external_system_reference", "External System Reference")},
                {TransactionSearchCriteriaParameters.ORIGINAL_SYSTEM_REFERENCE, new TextColumn("trx_header.Original_system_reference", "Original System Reference")}, 
                {TransactionSearchCriteriaParameters.CUSTOMER_ID, new NumberColumn("trx_header.customerId", "Customer Id")}, 
                {TransactionSearchCriteriaParameters.TRANSACTION_DATE, new DateTimeColumn("trx_header.TrxDate", "Tranaction Date")}, 
                {TransactionSearchCriteriaParameters.LAST_UPDATE_TIME, new DateTimeColumn("trx_header.LastUpdateTime", "Last Update Time")},   
                {TransactionSearchCriteriaParameters.TRANSACTION_NUMBER, new TextColumn("trx_header.trx_no", "Transaction No")},
                {TransactionSearchCriteriaParameters.REMARKS, new TextColumn("trx_header.Remarks", "Remarks")},
                {TransactionSearchCriteriaParameters.USER_ID, new NumberColumn("trx_header.user_id", "User Id")}, 
                {TransactionSearchCriteriaParameters.TRANSACTION_GUID, new TextColumn("trx_header.Guid","Transaction Guid")}, 
                {TransactionSearchCriteriaParameters.ORIGINAL_TRX_ID, new NumberColumn("trx_header.OriginalTrxid","Original Transaction Id")},
                {TransactionSearchCriteriaParameters.TRX_PAYMENT_MODE_ID, new NumberColumn("tps1.PaymentModeID","Payment Mode Id")},
                {TransactionSearchCriteriaParameters.TRX_PAYMENT_ATTRIBUTE1, new NumberColumn("tps1.Attribute1","Transaction Payment Attribute1")},
                {TransactionSearchCriteriaParameters.TRX_PAYMENT_ATTRIBUTE2, new NumberColumn("tps1.Attribute2","Transaction Payment Attribute2")},
                {TransactionSearchCriteriaParameters.TRX_PAYMENT_ATTRIBUTE3, new NumberColumn("tps1.Attribute3","Transaction Payment Attribute3")},
                {TransactionSearchCriteriaParameters.TRX_PAYMENT_ATTRIBUTE4, new NumberColumn("tps1.Attribute4","Transaction Payment Attribute4")},
                {TransactionSearchCriteriaParameters.TRX_PAYMENT_ATTRIBUTE5, new NumberColumn("tps1.Attribute5","Transaction Payment Attribute5")},
            };
        }
    }
}
