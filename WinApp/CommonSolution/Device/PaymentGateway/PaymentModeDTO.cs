/********************************************************************************************
 * Project Name - PaymentModeDTO Programs
 * Description  - Data object of PaymentMode DTO  
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        26-Nov-2016   Rakshith          Created 
 *2.50.0      05-Dec-2018   Mathew Ninan       Transaction re-design. Added Gateway Lookup
 *2.60        10-Apr-2019   Mushahid Faizan    Added DiscountCouponsDTO List & PaymentModeChannelsDTO List.
 *2.70.2        10-Jul-2019   Girish kundar    Modified :Added constructor for required fields .
 *                                                      Added List<PaymentModeChanelDTO> and  Missed Who columns. 
 *            26-Jul-2019   Mushahid Faizan    Added isActive
 *2.90        15-Jul-2020   Girish Kundar      Modified : Added PAYMENT_MODE_ID_LIST as search parameter
 *2.130.0     21-May-2021   Girish Kundar      Modified: Added Attribue1  to 5 columns to the table as part of Xero integration
 *2.130.7     13-Apr-2022   Guru S A           Payment mode OTP validation changes
 *2.140.0     07-Sep-2021   Fiona              Added PAYMENT_CHANNEL_NAME search parameter
                                               Added copy Constructor
                                               Added CompatiablePaymentModesDTOList
 *2.150.1     22-Feb-2023   Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Discounts;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    ///  PaymentModeDTO Class
    /// </summary>
    public class PaymentModeDTO
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by PAYMENT_MODE_ID
            /// </summary>
            PAYMENT_MODE_ID,
            /// <summary>
            /// Search by PAYMENT_MODE_GUID
            /// </summary>
            PAYMENT_MODE_GUID,
            /// <summary>
            /// Search by PAYMENT_MODE
            /// </summary>
            PAYMENT_MODE,
            /// <summary>
            /// Search by SITE_ID
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by ISCash
            /// </summary>
            ISCASH,
            /// <summary>
            /// Search by IsCreditCard
            /// </summary>
            ISCREDITCARD,
            /// <summary>
            /// Search by ISDebitCard
            /// </summary>
            ISDEBITCARD,
            /// <summary>
            /// Search by ISRoundOff
            /// </summary>
            ISROUNDOFF,
            /// <summary>
            /// Search by Master Entity Id field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by PAYMENT MODE ID LIST field
            /// </summary>
            PAYMENT_MODE_ID_LIST,
            /// <summary>
            /// Search by PAYMENT_CHANNEL_NAME
            /// </summary>
            PAYMENT_CHANNEL_NAME ,
           
            /// <summary>
            /// Search by OTPValidation field
            /// </summary>
            OTP_VALIDATION_REQUIRED

        }

        private int paymentModeId;
        private string paymentMode;
        private bool isCreditCard;
        private decimal? creditCardSurchargePercentage;
        private bool synchStatus;
        private int site_id;
        private bool isCash;
        private bool isDebitCard;
        private int gateway;
        private PaymentGateways gatewayLookUp;
        private bool managerApprovalRequired;
        private bool isRoundOff;
        private bool pOSAvailable;
        private int displayOrder;
        private int masterEntityId;
        private string guid;
        private string imageFileName;
        private char isQRCode;//ChinaICBC changes
        private bool paymentReferenceMandatory;
        private bool isActive;
        private LookupValuesDTO paymentGateway;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private List<PaymentModeChannelsDTO> paymentModeChannelsDTOList;
        private List<DiscountCouponsDTO> discountCouponsDTOList;
        private List<CompatiablePaymentModesDTO> compatiablePaymentModesDTOList;
        private List<PaymentModeDisplayGroupsDTO> paymentModeDisplayGroupsDTOList;
        private string attribute1;
        private string attribute2;
        private string attribute3;
        private string attribute4;
        private string attribute5;
        private bool otpValidation;
        private bool notifyingObjectIsChanged;
        private  readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>  
        public PaymentModeDTO()
        {
            log.LogMethodEntry();
            this.paymentModeId = -1;
            this.site_id = -1;
            this.paymentMode = string.Empty;
            this.gatewayLookUp = PaymentGateways.None;
            this.gateway = -1;
            paymentModeChannelsDTOList = new List<PaymentModeChannelsDTO>();
            discountCouponsDTOList = new List<DiscountCouponsDTO>();
            compatiablePaymentModesDTOList = new List<CompatiablePaymentModesDTO>();
            paymentModeDisplayGroupsDTOList = new List<PaymentModeDisplayGroupsDTO>();
            this.imageFileName = string.Empty;
            masterEntityId = -1;
            otpValidation = false;
            log.LogMethodExit();
        }


        /// <summary>
        ///  constructor with Required Parameter
        /// </summary>
        public PaymentModeDTO(int paymentModeId, string paymentMode, bool isCreditCard, decimal? creditCardSurchargePercentage,
                               bool isCash, bool isDebitCard, int gateway, bool managerApprovalRequired,
                               bool isRoundOff, bool pOSAvailable, int displayOrder, string imageFileName, char isQRCode,//ChinaICBC changes
                               bool PaymentReferenceMandatory,string attribute1, string attribute2,
                               string attribute3, string attribute4, string attribute5, bool otpValidation)
            :this()
        {
            log.LogMethodEntry( paymentModeId,  paymentMode,  isCreditCard,  creditCardSurchargePercentage,
                                isCash,  isDebitCard,  gateway,  managerApprovalRequired,
                                isRoundOff,  pOSAvailable,  displayOrder,  imageFileName,  isQRCode,//ChinaICBC changes
                                PaymentReferenceMandatory, attribute1, attribute2, attribute3, attribute4, attribute5, 
                               otpValidation);
            this.paymentModeId = paymentModeId;
            this.paymentMode = paymentMode;
            this.isCreditCard = isCreditCard;
            this.creditCardSurchargePercentage = creditCardSurchargePercentage;
            this.isCash = isCash;
            this.isDebitCard = isDebitCard;
            this.gateway = gateway;
            this.managerApprovalRequired = managerApprovalRequired;
            this.isRoundOff = isRoundOff;
            this.pOSAvailable = pOSAvailable;
            this.displayOrder = displayOrder;
            this.imageFileName = imageFileName;
            this.isQRCode = isQRCode;//ChinaICBC changes
            this.paymentReferenceMandatory = PaymentReferenceMandatory;
            this.attribute1 = attribute1;
            this.attribute2 = attribute2;
            this.attribute3 = attribute3;
            this.attribute4 = attribute4;
            this.attribute5 = attribute5;
            this.otpValidation = otpValidation;
            log.LogMethodExit();
        }



        /// <summary>
        ///  constructor with Parameter
        /// </summary>
        public PaymentModeDTO(int paymentModeId, string paymentMode, bool isCreditCard, decimal? creditCardSurchargePercentage, string guid,
                               bool synchStatus, int site_id, bool isCash, bool isDebitCard, int gateway, bool managerApprovalRequired,
                               bool isRoundOff, bool pOSAvailable, int displayOrder, int masterEntityId, string imageFileName, char isQRCode,//ChinaICBC changes
                               bool PaymentReferenceMandatory, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,
                               string attribute1, string attribute2, string attribute3, string attribute4, string attribute5, 
                               bool otpValidation)
            :this(paymentModeId, paymentMode, isCreditCard, creditCardSurchargePercentage,
                  isCash, isDebitCard, gateway, managerApprovalRequired,
                  isRoundOff, pOSAvailable, displayOrder, imageFileName, isQRCode, PaymentReferenceMandatory,
                   attribute1, attribute2, attribute3, attribute4, attribute5, otpValidation)
        {
           log.LogMethodEntry(paymentModeId, paymentMode, isCreditCard, creditCardSurchargePercentage,
                                isCash, isDebitCard, gateway, managerApprovalRequired,
                                isRoundOff, pOSAvailable, displayOrder, imageFileName, isQRCode,
                                PaymentReferenceMandatory, createdBy, creationDate, lastUpdatedBy, lastUpdateDate,
                                 attribute1, attribute2, attribute3, attribute4, attribute5, otpValidation);
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.site_id = site_id;
            this.masterEntityId = masterEntityId;
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.lastUpdatedBy = lastUpdatedBy;
            this.creationDate = creationDate;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }
        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="parameterPaymentModeDTO"></param>
        public PaymentModeDTO(PaymentModeDTO parameterPaymentModeDTO)
            :this(parameterPaymentModeDTO.paymentModeId, parameterPaymentModeDTO.paymentMode, parameterPaymentModeDTO.isCreditCard, parameterPaymentModeDTO.creditCardSurchargePercentage,
                  parameterPaymentModeDTO.guid, parameterPaymentModeDTO.synchStatus, parameterPaymentModeDTO.site_id, parameterPaymentModeDTO.isCash, parameterPaymentModeDTO.isDebitCard,
                  parameterPaymentModeDTO.gateway, parameterPaymentModeDTO.managerApprovalRequired, parameterPaymentModeDTO.isRoundOff, parameterPaymentModeDTO.pOSAvailable,
                  parameterPaymentModeDTO.displayOrder, parameterPaymentModeDTO.masterEntityId, parameterPaymentModeDTO.imageFileName, parameterPaymentModeDTO.isQRCode,
                  parameterPaymentModeDTO.PaymentReferenceMandatory, parameterPaymentModeDTO.isActive, parameterPaymentModeDTO.createdBy, parameterPaymentModeDTO.creationDate,
                  parameterPaymentModeDTO.lastUpdatedBy, parameterPaymentModeDTO.lastUpdateDate, parameterPaymentModeDTO.attribute1, parameterPaymentModeDTO.attribute2,
                  parameterPaymentModeDTO.attribute3, parameterPaymentModeDTO.attribute4, parameterPaymentModeDTO.attribute5, parameterPaymentModeDTO.otpValidation)
        {
            log.LogMethodEntry();
            if (parameterPaymentModeDTO.paymentGateway != null)
            {
                this.paymentGateway = new LookupValuesDTO(parameterPaymentModeDTO.paymentGateway);
            }
            if (parameterPaymentModeDTO.paymentModeChannelsDTOList != null && parameterPaymentModeDTO.paymentModeChannelsDTOList.Any())
            {
                for (int i = 0; i < parameterPaymentModeDTO.paymentModeChannelsDTOList.Count; i++)
                {
                    this.paymentModeChannelsDTOList.Add(new PaymentModeChannelsDTO(parameterPaymentModeDTO.paymentModeChannelsDTOList[i]));
                }
            }
            if (parameterPaymentModeDTO.compatiablePaymentModesDTOList != null && parameterPaymentModeDTO.compatiablePaymentModesDTOList.Any())
            {
                for (int i = 0; i < parameterPaymentModeDTO.compatiablePaymentModesDTOList.Count; i++)
                {
                    this.compatiablePaymentModesDTOList.Add(new CompatiablePaymentModesDTO(parameterPaymentModeDTO.compatiablePaymentModesDTOList[i]));
                }
            }
            if (parameterPaymentModeDTO.paymentModeDisplayGroupsDTOList != null && parameterPaymentModeDTO.paymentModeDisplayGroupsDTOList.Any())
            {
                for (int i = 0; i < parameterPaymentModeDTO.paymentModeDisplayGroupsDTOList.Count; i++)
                {
                    this.paymentModeDisplayGroupsDTOList.Add(new PaymentModeDisplayGroupsDTO(parameterPaymentModeDTO.paymentModeDisplayGroupsDTOList[i]));
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the PaymentModeId field
        /// </summary>
        [DisplayName("PaymentModeId")]
        public int PaymentModeId { get { return paymentModeId; } set { paymentModeId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PaymentMode field
        /// </summary>
        [DisplayName("PaymentMode")]
        public string PaymentMode { get { return paymentMode; } set { paymentMode = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsCreditCard field
        /// </summary>
        [DisplayName("IsCreditCard")]
        public bool IsCreditCard { get { return isCreditCard; } set { isCreditCard = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreditCardSurchargePercentage field
        /// </summary>
        [DisplayName("CreditCardSurchargePercentage")]
        public decimal? CreditCardSurchargePercentage { get { return creditCardSurchargePercentage ;} set { creditCardSurchargePercentage = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }

        /// <summary>
        /// Get/Set method of the Site_id field
        /// </summary>
        [DisplayName("Site_id")]
        public int Site_id { get { return site_id; } set { site_id = value; } }

        /// <summary>
        /// Get/Set method of the IsCash field
        /// </summary>
        [DisplayName("IsCash")]
        public bool IsCash { get { return isCash; } set { isCash = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsDebitCard field
        /// </summary>
        [DisplayName("IsDebitCard")]
        public bool IsDebitCard { get { return isDebitCard; } set { isDebitCard = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Gateway field
        /// </summary>
        [DisplayName("Gateway")]
        public int Gateway { get { return gateway; } set { gateway = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Gateway Lookup field
        /// </summary>
        [DisplayName("GatewayLookUp")]
        public PaymentGateways GatewayLookUp { get { return gatewayLookUp; } set { gatewayLookUp = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ManagerApprovalRequired field
        /// </summary>
        [DisplayName("ManagerApprovalRequired")]
        public bool ManagerApprovalRequired { get { return managerApprovalRequired; } set { managerApprovalRequired = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsRoundOff field
        /// </summary>
        [DisplayName("IsRoundOff")]
        public bool IsRoundOff { get { return isRoundOff; } set { isRoundOff = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the POSAvailable field
        /// </summary>
        [DisplayName("POSAvailable")]
        public bool POSAvailable { get { return pOSAvailable; } set { pOSAvailable = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DisplayOrder field
        /// </summary>
        [DisplayName("DisplayOrder")]
        public int DisplayOrder { get { return displayOrder; } set { displayOrder = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ImageFileName field
        /// </summary>
        [DisplayName("ImageFileName")]
        [DefaultValue("")]
        public string ImageFileName { get { return imageFileName; } set { imageFileName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PaymentGateway field
        /// </summary>
        [DisplayName("PaymentGateway")]
        public LookupValuesDTO PaymentGateway { get { return paymentGateway; } set { paymentGateway = value; } }

        /// <summary>//Starts:ChinaICBC changes
        /// Get/Set method of the IsQRCode field
        /// </summary>
        [DisplayName("IsQRCode")]
        public char IsQRCode { get { return isQRCode; } set { isQRCode = value; this.IsChanged = true; } }//Ends:ChinaICBC changes

        /// <summary>
        /// Get/Set method of the PaymentReferenceMandatory field
        /// </summary>
        [DisplayName("Payment Reference Mandatory")]
        public bool PaymentReferenceMandatory { get { return paymentReferenceMandatory; } set { paymentReferenceMandatory = value; this.IsChanged = true; } }//Ends:ChinaICBC changes

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }

        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; }
        }

        /// <summary>
        /// Get/Set methods for DiscountCouponsDTOList 
        /// </summary>
        public List<DiscountCouponsDTO> DiscountCouponsDTOList
        {
            get
            {
                return discountCouponsDTOList;
            }

            set
            {
                discountCouponsDTOList = value;
            }
        }
        /// <summary>
        /// Get/Set method of the PaymentModeChannelDTOList field
        /// </summary>
        public List<PaymentModeChannelsDTO> PaymentModeChannelsDTOList
        {
            get { return paymentModeChannelsDTOList; }
            set { paymentModeChannelsDTOList = value; }
        }
        /// <summary>
        ///  Get/Set method of the CompatiablePaymentModesDTOList field
        /// </summary>
        public List<CompatiablePaymentModesDTO> CompatiablePaymentModesDTOList
        {
            get { return compatiablePaymentModesDTOList; }
            set { compatiablePaymentModesDTOList = value; }
        }
        /// <summary>
        ///  Get/Set method of the PaymentModeDisplayGroupsDTOList field
        /// </summary>
        public List<PaymentModeDisplayGroupsDTO> PaymentModeDisplayGroupsDTOList
        {
            get { return paymentModeDisplayGroupsDTOList; }
            set { paymentModeDisplayGroupsDTOList = value; }
        }
        /// <summary>
        /// Attribute1
        /// </summary>
        public string Attribute1
        {
            get { return attribute1; }
            set { this.IsChanged = true; attribute1 = value; }
        }

        /// <summary>
        /// Attribute2
        /// </summary>
        public string Attribute2
        {
            get { return attribute2; }
            set { this.IsChanged = true; attribute2 = value; }
        }

        /// <summary>
        /// Attribute3
        /// </summary>
        public string Attribute3
        {
            get { return attribute3; }
            set { this.IsChanged = true; attribute3 = value; }
        }

        /// <summary>
        /// Attribute4
        /// </summary>
        public string Attribute4
        {
            get { return attribute4; }
            set { this.IsChanged = true; attribute4 = value; }
        }

        /// <summary>
        /// Attribute5
        /// </summary>
        public string Attribute5
        {
            get { return attribute5; }
            set { this.IsChanged = true; attribute5 = value; }
        }

        /// <summary>
        /// Get/Set method of the OTPValidation field
        /// </summary>
        public bool OTPValidation { get { return otpValidation; } set { otpValidation = value; this.IsChanged = true; } }
 

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || paymentModeId < 0;
                }
            }

            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }

        /// <summary>
        /// Returns whether the PaymentModeDTO is changed or any of its  children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (paymentModeChannelsDTOList != null &&
                    paymentModeChannelsDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (discountCouponsDTOList != null &&
                    discountCouponsDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if(compatiablePaymentModesDTOList!=null &&
                    compatiablePaymentModesDTOList.Any(x=>x.IsChanged))
                {
                    return true;
                }
                if (paymentModeDisplayGroupsDTOList != null &&
                    paymentModeDisplayGroupsDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            IsChanged = false;
            log.LogMethodExit();
        }

    }
}
