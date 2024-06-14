/* Project Name - TransactionParams Programs 
* Description  - Data object of the TransactionParams
* 
**************
**Version Log
**************
*Version     Date          Modified By        Remarks          
*********************************************************************************************
*1.00        15-June-2016   Rakshith           Created 
********************************************************************************************/

using Semnox.Core;
using Semnox.Parafait.Waiver;
using System;
using System.Collections.Generic;
using System.ComponentModel;



namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// This is the TransactionParams data object class.  
    /// </summary>
    public class TransactionParams
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int trxId;
        string posIdentifier;
        string loginId;
        bool shouldCommit;
        string creditCardName;
        string creditCardNumber;
        string creditCardType;
        string creditCardExpDate;
        int creditCardInvoiceNumber;
        string creditCardPaymentReference;
        string paymentReference;
        string orderRemarks;
        int customerId;
        int userId;
        int roleId;
        string paymentCardNumber;
        DateTime visitDate;
        bool autoPayDebitCard;
        int paymentModeId;
        int site_id;
        bool closeTransaction;
        string trxPaymentReference;
        bool applyOffset;
        bool forceIsCorporate;
        bool applySystemVisitDate;
        int trxLineId;
        string discountCouponCode;
        int membershipId;
        int membershipRewardsId;
        string expireWithMembership;
        string forMembershipOnly;
        List<WaiverSignatureDTO> waiverSignedDTOList;
        string waiverSigningMode;
        bool paymentProcessingCompleted;
        bool reverseTransaction;
        int ccResponseId;
        string authorizedPaymentAmount;

        /// <summary>
        /// Default constructor
        /// </summary>
        public TransactionParams()
        {
            log.LogMethodEntry();
            trxId = -1;
            posIdentifier = "";
            loginId = "";
            shouldCommit = false;
            creditCardName = "";
            creditCardNumber = "";
            creditCardType = "";
            creditCardExpDate = "";
            creditCardInvoiceNumber = -1;
            creditCardPaymentReference = "";
            customerId = -1;
            roleId = -1;
            userId = -1;
            paymentModeId = -1;
            paymentCardNumber = "";
            visitDate = DateTime.MinValue;
            autoPayDebitCard = false;
            this.site_id = -1;
            this.closeTransaction = true;
            this.trxPaymentReference = "";
            this.applyOffset = false;
            this.forceIsCorporate = true;
            this.applySystemVisitDate = false;
            this.trxLineId = -1;
            this.discountCouponCode = "";
            membershipId = -1;
            membershipRewardsId = -1;
            expireWithMembership = "N";
            forMembershipOnly = "N"; 
            this.waiverSigningMode = "";
            this.waiverSignedDTOList = new List<WaiverSignatureDTO>();
            paymentProcessingCompleted = true;
            reverseTransaction = false;
            ccResponseId = -1;
            authorizedPaymentAmount = "0";
            log.LogMethodExit();

        }

        /// <summary>
        ///  constructor with Parameter
        /// </summary>
        public TransactionParams(string posIdentifier,string loginId, bool shouldCommit, string creditCardName,string creditCardNumber,
                                        string creditCardType,string creditCardExpDate,int creditCardInvoiceNumber,string creditCardPaymentReference,
                                        string orderRemarks, int customerId, int roleId, string paymentReference , int membershipId, int membershipRewardsId, string expireWithMembership = "N", string forMembershipOnly = "N", bool paymentProcessingCompleted = true)
        {
            log.LogMethodEntry(posIdentifier, loginId, shouldCommit, creditCardName, creditCardNumber, creditCardType, creditCardExpDate, creditCardInvoiceNumber, 
                               creditCardPaymentReference, orderRemarks,  customerId,  roleId,  paymentReference,  membershipId,  membershipRewardsId,  expireWithMembership, forMembershipOnly);
            this.posIdentifier = posIdentifier;
            this.loginId = loginId;
            this.shouldCommit = shouldCommit;
            this.creditCardName = creditCardName;
            this.creditCardNumber = creditCardNumber;
            this.creditCardType = creditCardType;
            this.creditCardExpDate = creditCardExpDate;
            this.creditCardInvoiceNumber = creditCardInvoiceNumber;
            this.creditCardPaymentReference = creditCardPaymentReference;
            this.orderRemarks = orderRemarks;
            this.customerId = customerId;
            this.roleId = roleId;
            this.paymentReference = paymentReference;
            this.membershipId = membershipId;
            this.membershipRewardsId = membershipRewardsId;
            this.expireWithMembership = expireWithMembership;
            this.forMembershipOnly = forMembershipOnly;
            this.paymentProcessingCompleted = paymentProcessingCompleted;

            log.LogMethodExit();
        }


        
        
         
        //    PaymentModeId = -1, 
        /// <summary>
        ///  constructor with Parameter
        /// </summary>
        public TransactionParams(string posIdentifier, string loginId, bool shouldCommit,  string orderRemarks, int customerId, bool applyOffset, bool forceIsCorporate, bool applySystemVisitDate, bool closeTransaction, DateTime visitDate, int paymentModeId, string trxPaymentReference, int membershipId, int membershipRewardsId, int siteId, string expireWithMembership = "N", string forMembershipOnly = "N", bool paymentProcessingCompleted = true)
        {
            log.LogMethodEntry(posIdentifier, loginId, shouldCommit, orderRemarks, customerId, applyOffset, forceIsCorporate, applySystemVisitDate, closeTransaction, visitDate, 
                              paymentModeId, trxPaymentReference, membershipId, membershipRewardsId, siteId, expireWithMembership , forMembershipOnly );
            this.posIdentifier = posIdentifier;
            this.loginId = loginId;
            this.shouldCommit = shouldCommit;
            this.orderRemarks = orderRemarks;
            this.customerId = customerId;
            this.applyOffset = applyOffset;
            this.forceIsCorporate = forceIsCorporate;
            this.applySystemVisitDate = applySystemVisitDate;
            this.closeTransaction = closeTransaction;
            this.visitDate = visitDate;
            this.paymentModeId = paymentModeId;
            this.trxPaymentReference = trxPaymentReference;
            this.membershipId = membershipId;
            this.membershipRewardsId = membershipRewardsId; 
            this.expireWithMembership = expireWithMembership;
            this.forMembershipOnly = forMembershipOnly;
            this.site_id = siteId;
            this.paymentProcessingCompleted = paymentProcessingCompleted;

            log.LogMethodExit();
        }



        /// <summary>
        /// Get/Set method of the TrxId field
        /// </summary>
        [DisplayName("TrxId")]
        [DefaultValue(-1)]
        public int TrxId { get { return trxId; } set { trxId = value; } }

        /// <summary>
        /// Get/Set method of the PosIdentifier field
        /// </summary>
        [DisplayName("PosIdentifier")]
        public string PosIdentifier { get { return posIdentifier; } set { posIdentifier = value; } }

        /// <summary>
        /// Get/Set method of the Loginid field
        /// </summary>
        [DisplayName("LoginId")]
        public string LoginId { get { return loginId; } set { loginId = value; } }

        /// <summary>
        /// Get/Set method of the UserId field
        /// </summary>
        [DisplayName("UserId")]
        [DefaultValue(-1)]
        public int UserId { get { return userId; } set { userId = value; } }

        /// <summary>
        /// Get/Set method of the RoleId field
        /// </summary>
        [DisplayName("RoleId")]
        [DefaultValue(-1)]
        public int RoleId { get { return roleId; } set { roleId = value; } }

        /// <summary>
        /// Get/Set method of the ShouldCommit field
        /// </summary>
        [DisplayName("ShouldCommit")]
        public bool ShouldCommit { get { return shouldCommit; } set { shouldCommit = value; } }

        /// <summary>
        /// Get/Set method of the CreditCardName field
        /// </summary>
        [DisplayName("CreditCardName")]
        [DefaultValue("")]
        public string CreditCardName { get { return creditCardName; } set { creditCardName = value; } }


        /// <summary>
        /// Get/Set method of the CreditCardNumber field
        /// </summary>
        [DisplayName("CreditCardNumber")]
        [DefaultValue("")]
        public string CreditCardNumber { get { return creditCardNumber; } set { creditCardNumber = value; } }

        /// <summary>
        /// Get/Set method of the CreditCardType field
        /// </summary>
        [DisplayName("CreditCardType")]
        [DefaultValue("")]
        public string CreditCardType { get { return creditCardType; } set { creditCardType = value; } }


        /// <summary>
        /// Get/Set method of the CreditCardExpDate field
        /// </summary>
        [DisplayName("CreditCardExpDate")]
        [DefaultValue("")]
        public string CreditCardExpDate { get { return creditCardExpDate; } set { creditCardExpDate = value; } }

        /// <summary>
        /// Get/Set method of the CreditCardInvoiceNumber field
        /// </summary>
        [DisplayName("CreditCardInvoiceNumber")]
        [DefaultValue(-1)]
        public int CreditCardInvoiceNumber { get { return creditCardInvoiceNumber; } set { creditCardInvoiceNumber = value; } }

        /// <summary>
        /// Get/Set method of the CreditCardPaymentReference field
        /// </summary>
        [DisplayName("CreditCardPaymentReference")]
        [DefaultValue("")]
        public string CreditCardPaymentReference { get { return creditCardPaymentReference; } set { creditCardPaymentReference = value; } }


        /// <summary>
        /// Get/Set method of the TrxPaymentReference field
        /// </summary>
        [DisplayName("TrxPaymentReference")]
        [DefaultValue("")]
        public string TrxPaymentReference { get { return trxPaymentReference; } set { trxPaymentReference = value; } }

        /// <summary>
        /// Get/Set method of the PaymentReference field for Trx_Payments
        /// </summary>
        [DisplayName("PaymentReference")]
        [DefaultValue("")]
        public string PaymentReference { get { return paymentReference; } set { paymentReference = value; } }


        /// <summary>
        /// Get/Set method of the OrderRemarks field
        /// </summary>
        [DisplayName("OrderRemarks")]
        [DefaultValue("")]
        public string OrderRemarks { get { return orderRemarks; } set { orderRemarks = value; } }

        /// <summary>
        /// Get/Set method of the OrderRemarks field
        /// </summary>
        [DisplayName("PaymentCardNumber")]
        [DefaultValue("")]
        public string PaymentCardNumber { get { return paymentCardNumber; } set { paymentCardNumber = value; } }

        /// <summary>
        /// Get/Set method of the CustomerId field
        /// </summary>
        [DisplayName("CustomerId")]
        [DefaultValue(-1)]
        public int CustomerId { get { return customerId; } set { customerId = value; } }

      
        /// <summary>
        /// Get/Set method of the VisitDate field
        /// </summary>
        [DisplayName("VisitDate")]
        [DefaultValue(typeof(DateTime),"")]
        public DateTime VisitDate
        {  
            get
            {
                if (visitDate.Year < DateTime.MinValue.Year)
                {
                    visitDate = DateTime.MinValue;
                }
                return visitDate;
            }
            set { visitDate = value; }
        }


        /// <summary>
        /// Get/Set method of the AutoPayDebitCard field
        /// </summary>
        [DisplayName("AutoPayDebitCard")]
        public bool AutoPayDebitCard { get { return autoPayDebitCard; } set { autoPayDebitCard = value; } }

        /// <summary>
        /// Get/Set method of the CustomerId field
        /// </summary>
        [DisplayName("PaymentModeId")]
        [DefaultValue(-1)]
        public int PaymentModeId { get { return paymentModeId; } set { paymentModeId = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [DefaultValue(-1)]
        public int SiteId { get { return site_id; } set { site_id = value; } }


        /// <summary>
        /// Get/Set method of the CloseTransaction field
        /// </summary>
        [DisplayName("CloseTransaction")]
        public bool CloseTransaction { get { return closeTransaction; } set { closeTransaction = value; } }


        /// <summary>
        /// Get/Set method of the ApplyOffset field
        /// </summary>
        [DisplayName("ApplyOffset")]
        public bool ApplyOffset { get { return applyOffset; } set { applyOffset = value; } }

        /// <summary>
        /// Get/Set method of the ForceIsCorporate field
        /// </summary>
        [DisplayName("ForceIsCorporate")]
        [DefaultValue((bool)true)]
        public bool ForceIsCorporate { get { return forceIsCorporate; } set { forceIsCorporate = value; } }


        /// <summary>
        /// Get/Set method of the ApplySystemVisitDate field
        /// </summary>
        [DisplayName("ApplySystemVisitDate")]
        public bool ApplySystemVisitDate { get { return applySystemVisitDate; } set { applySystemVisitDate = value; } }


        /// <summary>
        /// Get/Set method of the TrxLineId field
        /// </summary>
        [DisplayName("TrxLineId")]
        [DefaultValue(-1)]
        public int TrxLineId { get { return trxLineId; } set { trxLineId = value; } }

        /// <summary>
        /// Get/Set method of the DiscountCouponCode field
        /// </summary>
        public string DiscountCouponCode
        {
            get { return discountCouponCode; }
            set { discountCouponCode = value; }
        }


        /// <summary>
        /// Get/Set method of the membershipId field
        /// </summary>
        [DisplayName("Membership Id")]
        [DefaultValue(-1)]
        public int MembershipId { get { return membershipId; } set { membershipId = value; } }

        /// <summary>
        /// Get/Set method of the membershipRewardsId field
        /// </summary>
        [DisplayName("Membership Rewards Id")]
        [DefaultValue(-1)]
        public int MembershipRewardsId { get { return membershipRewardsId; } set { membershipRewardsId = value; } }

        /// <summary>
        /// Get/Set method of the ExpireWithMembership field
        /// </summary>
        [DisplayName("Expire With Membership")]
        public string ExpireWithMembership { get { return expireWithMembership; } set { expireWithMembership = value; } }

        /// <summary>
        /// Get/Set method of the forMembershipOnly field
        /// </summary>
        [DisplayName("For Membership Only")]
        public string ForMembershipOnly { get { return forMembershipOnly; } set { forMembershipOnly = value; } }
        
        /// <summary>
        /// Get/Set method of the WaiverSigningMode field  should be passed to filter waivers assigned to specific signing Mode
        /// </summary>
        public string WaiverSigningMode
        {
            get { return waiverSigningMode; }
            set { waiverSigningMode = value; }
        }


        /// <summary>
        /// Get/Set method of the WaiversSignedDTOList field
        /// </summary>
        public List<WaiverSignatureDTO> WaiverSignedDTOList
        {
            get
            {
                return waiverSignedDTOList;
            }
            set
            {
                waiverSignedDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PaymentProcessingCompleted field
        /// </summary>
        public bool PaymentProcessingCompleted { get { return paymentProcessingCompleted; } set { paymentProcessingCompleted = value; } }

        /// <summary>
        /// Get/Set method of the ReverseTransaction field
        /// </summary>
        [DisplayName("ReverseTransaction")]
        public bool ReverseTransaction { get { return reverseTransaction; } set { reverseTransaction = value; } }


        /// <summary>
        /// Get/Set method of the CCResponseId field for Trx_Payments
        /// </summary>
        [DisplayName("CCResponseId")]
        [DefaultValue(-1)]
        public int CCResponseId { get { return ccResponseId; } set { ccResponseId = value; } }


        /// <summary>
        /// Get/Set method of the AuthorizedPaymentAmount field
        /// </summary>
        [DisplayName("AuthorizedPaymentAmount")]
        [DefaultValue("")]
        public string AuthorizedPaymentAmount { get { return authorizedPaymentAmount; } set { authorizedPaymentAmount = value; } }
    }
}
