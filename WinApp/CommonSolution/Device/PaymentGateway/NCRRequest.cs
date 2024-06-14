using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// class NCRRequest
    /// </summary>
    public class NCRRequest
    {
        /// <summary>
        /// Amount should multiplied by 100. That $1.23 sent as 123
        /// </summary>
        public int Amount;
        /// <summary>
        /// Please select the transaction type.
        /// </summary>
        public eTransactionType TransactionType;        
        /// <summary>
        /// Unique number to identify the transaction. Its recomandade not mandatory.
        /// </summary>
        public string PostTransactionNumber;
        /// <summary>
        /// Unique number to identify the transaction. This is assigned only during the refund and void.
        /// </summary>
        public StringBuilder SequenceNo = new StringBuilder();
        /// <summary>
        /// If the POS is able to accept an approval for an amount other than the amount initially submitted.
        /// </summary>
        public bool IsAmountChangeAllowed;
        /// <summary>
        /// Personal Account Number, the values will be set during the manual entry.
        /// </summary>
        public StringBuilder PersonalAccountNumber=new StringBuilder();
        /// <summary>
        /// Assigned by the HOST for each approved transaction. Prompted for on Voice/Force Transacitons
        /// </summary>
        public string AuthorizationNumber;
        /// <summary>
        /// Expiration Date, the values will be set during the manual entry.
        /// </summary>
        public string ExpirationDate;
        /// <summary>
        /// Voucher Number, the values will be set during the EBT.
        /// </summary>
        public string VoucherNumber;
        /// <summary>
        /// Slide Card Only, required by ValidateData if POS has attached card reader configured.
        /// </summary>
        public StringBuilder Track2Data;
        /// <summary>
        /// Manager ID of the cashier.
        /// </summary>
        public string ManagerID;        
        /// <summary>
        /// UPC is currently used for Gift Cards. This field should typically be sent with 10 or 11 digits: the 10 digit UPC number.
        /// </summary>
        public string UPCCode;
        /// <summary>
        /// This function is called by the POS to set the SecondaryIDType
        /// </summary>
        public string SecondaryIDType;
        /// <summary>
        /// This function is called by the POS to set the SecondaryID for Check Authorization
        /// </summary>
        public string SecondaryID;
        /// <summary>
        /// This function is called by the POS to set the SecondaryID for Check Authorization
        /// </summary>
        public int TaxAmount;
        /// <summary>
        /// SiteId of the site
        /// </summary>
        public int SiteId;
        /// <summary>
        /// SiteId of the site
        /// </summary>
        public int ccResponseId;
    }
}
