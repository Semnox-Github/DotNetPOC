/********************************************************************************************
 * Project Name - Customer App Configuration                                                                     
 * Description  - DTO for Customer App configuration
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         08-May-2019   Nitin Pai            Created for Guest app
 *2.110        10-Feb-2021   Nitin Pai            Externalization enhancements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Semnox.Core.Utilities;
using Semnox.Parafait.WebCMS;

namespace Semnox.Parafait.CustomerApp
{
    public class CustomerAppConfigurationDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string splashImage;
        private string cardViewImage;
        private bool googleRegistration;
        private bool fbRegistration;
        private bool promotionLayout;
        private bool topGameLayout;
        private bool topEventLayout;
        private bool socialLayout;
        private bool storeLocatorLayout;
        private bool enableNewCard;
        private bool enableLinkCard;
        private bool enableTransferCredits;
        private bool enableRedeemCard;
        private bool enableRechargeCard;
        private bool enableLostCard;
        private bool showCreditsOnCard;
        private bool showBonusOnCard;
        private bool showTimeOnCard;
        private bool showTicketsOnCard;
        private bool showLoyaltyOnCard;
        private bool showCourtesyOnCard;
        private bool showMembershipOnCard;
        private bool showCardExpiry;
        private bool showCardIssued;
        private int idleTimeout;
        private int hqRefreshThreshold;
        private int appValidityCheckDelay;
        private string currencyCode;
        private List<LookupValuesDTO> customLinks;
        private bool enableVirtualSite;
        private int virtualSiteId;
        private bool enableGamePlay;
        private string playBoxLink;        
        private string numberFormat;
        private string promoImageFolderURL;
        private int ageOfMajority;
        private int defaultLanguage;
        private bool waiverDeactivationNeedsManagerApproval;
        private bool waiverCodeIsMandatoryToFetchCustomer;
        private bool checkWaiverRegistrationCountForTransaction;
        private bool otpCheckRequiredForWaiverRegistration;
        private int validityPeriodForWaiverRegistrationOTP;
        private bool enableWaiverSignInKiosk;
        private bool waiverOverrideNeedsManagerApproval;
        private bool allowWaiverOverride;
        private bool overrideSiteForTransaction;
        private string waiverSetSelectionOptions;
        private bool showCustomAttributesOnWaiver;
        private string promotionLayoutURL;
        private string storeLocatorURL;
        private string supportContactNumber;
        private string supportEmail;
        private string supportMessageText;
        private List<CMSSocialLinksDTO> cMSSocialLinksDTO;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomerAppConfigurationDTO()
        {
            GoogleRegistration = false;
            FbRegistration = false;
            PromotionLayout = true;
            TopGameLayout = true;
            TopEventLayout = true;
            EnableNewCard = true;
            EnableLinkCard = true;
            EnableRedeemCard = true;
            EnableRechargeCard = true;
            EnableTransferCredits = true;
            showCreditsOnCard = true;
            showBonusOnCard = true;
            showTimeOnCard = true;
            showTicketsOnCard = false;
            showLoyaltyOnCard = false;
            showCourtesyOnCard = false;
            IdleTimeout = 100;
            HQRefreshThreshold = 100;
            AppValidityCheckDelay = 100;
            enableVirtualSite = false;
            virtualSiteId = -1;
            enableGamePlay = false;
            playBoxLink = "";
            showMembershipOnCard = false;
	        waiverDeactivationNeedsManagerApproval = false;
            waiverCodeIsMandatoryToFetchCustomer = false;
            checkWaiverRegistrationCountForTransaction = false;
            otpCheckRequiredForWaiverRegistration = false;
            validityPeriodForWaiverRegistrationOTP = 5;
            enableWaiverSignInKiosk = false;
            waiverOverrideNeedsManagerApproval = false;
            allowWaiverOverride = true;
            overrideSiteForTransaction = false;
            waiverSetSelectionOptions = "";
            showCustomAttributesOnWaiver = false;
    }

        public CustomerAppConfigurationDTO(string splashImage, string cardViewImage, bool googleRegistration, bool fbRegistration,
            bool promotionLayout, bool topGameLayout, bool topEventLayout, bool enableNewCard, bool enableLinkCard,
            bool enableTransferCredits, bool enableRedeemCard, bool enableRechargeCard, bool enableLostCard, int idleTimeout, int hqRefreshThreshold, int appValidityCheckDelay, string currencyCode,
            bool showCreditsOnCard, bool showBonusOnCard, bool showTimeOnCard, bool showTicketsOnCard, bool showLoyaltyOnCard, bool showCourtesyOnCard, bool enableVirtualSite,
            int virtualSiteId, bool enableGamePlay, String playBoxLink, bool showMembershipOnCard, string numberFormat, string promoImageFolderURL,
            int ageOfMajority, int defaultLanguage, bool waiverDeactivationNeedsManagerApproval, bool waiverCodeIsMandatoryToFetchCustomer,
            bool checkWaiverRegistrationCountForTransaction, bool otpCheckRequiredForWaiverRegistration, int validityPeriodForWaiverRegistrationOTP,
            bool enableWaiverSignInKiosk, bool waiverOverrideNeedsManagerApproval, bool allowWaiverOverride, bool overrideSiteForTransaction, string waiverSetSelectionOptions, bool showCustomAttributesOnWaiver,
            string promotionLayoutURL, string storeLocatorURL, bool showCardExpiry, bool showCardIssued, bool socialLayout, bool storeLocatorLayout,
            string supportContactNumber, string supportEmail, string supportMessageText)
        {
            this.splashImage = splashImage;
            this.cardViewImage = cardViewImage;
            this.googleRegistration = googleRegistration;
            this.fbRegistration = fbRegistration;
            this.promotionLayout = promotionLayout;
            this.topGameLayout = topGameLayout;
            this.topEventLayout = topEventLayout;
            this.enableNewCard = enableNewCard;
            this.enableLinkCard = enableLinkCard;
            this.enableTransferCredits = enableTransferCredits;
            this.enableRedeemCard = enableRedeemCard;
            this.enableRechargeCard = enableRechargeCard;
            this.EnableLostCard = enableLostCard;
            this.idleTimeout = idleTimeout;
            this.hqRefreshThreshold = hqRefreshThreshold;
            this.appValidityCheckDelay = appValidityCheckDelay;
            this.currencyCode = currencyCode;
            this.showCreditsOnCard = showCreditsOnCard;
            this.showBonusOnCard = showBonusOnCard;
            this.showTimeOnCard = showTimeOnCard;
            this.showTicketsOnCard = showTicketsOnCard;
            this.showLoyaltyOnCard = showLoyaltyOnCard;
            this.showCourtesyOnCard = showCourtesyOnCard;
            this.customLinks = new List<LookupValuesDTO>();
            this.enableVirtualSite = enableVirtualSite;
            this.virtualSiteId = virtualSiteId;
            this.enableGamePlay = enableGamePlay;
            this.playBoxLink = playBoxLink;
            this.showMembershipOnCard = showMembershipOnCard;
            this.numberFormat = !String.IsNullOrWhiteSpace(numberFormat) ? numberFormat : "##0.00"; 
            this.promoImageFolderURL = promoImageFolderURL;
            this.ageOfMajority = ageOfMajority;
            this.defaultLanguage = defaultLanguage;
            this.waiverDeactivationNeedsManagerApproval = waiverDeactivationNeedsManagerApproval;
            this.waiverCodeIsMandatoryToFetchCustomer = waiverCodeIsMandatoryToFetchCustomer;
            this.checkWaiverRegistrationCountForTransaction = checkWaiverRegistrationCountForTransaction;
            this.otpCheckRequiredForWaiverRegistration = otpCheckRequiredForWaiverRegistration;
            this.validityPeriodForWaiverRegistrationOTP = validityPeriodForWaiverRegistrationOTP;
            this.enableWaiverSignInKiosk = enableWaiverSignInKiosk;
            this.waiverOverrideNeedsManagerApproval = waiverOverrideNeedsManagerApproval;
            this.allowWaiverOverride = allowWaiverOverride;
            this.overrideSiteForTransaction = overrideSiteForTransaction;
            this.waiverSetSelectionOptions = waiverSetSelectionOptions;
            this.showCustomAttributesOnWaiver = showCustomAttributesOnWaiver;
            this.promotionLayoutURL = promotionLayoutURL;
            this.storeLocatorURL = storeLocatorURL;
            this.showCardExpiry = showCardExpiry;
            this.showCardIssued = showCardIssued;
            this.socialLayout = socialLayout;
            this.storeLocatorLayout = storeLocatorLayout;
            this.supportContactNumber = supportContactNumber;
            this.supportEmail = supportEmail;
            this.supportMessageText = supportMessageText;
    }

        public string SplashImage { get { return splashImage; } set { this.IsChanged = true; splashImage = value; } }
        public string CardViewImage { get { return cardViewImage; } set { this.IsChanged = true; cardViewImage = value; } }
        public bool GoogleRegistration { get { return googleRegistration; } set { this.IsChanged = true; googleRegistration = value; } }
        public bool FbRegistration { get { return fbRegistration; } set { this.IsChanged = true; fbRegistration = value; } }
        public bool PromotionLayout { get { return promotionLayout; } set { this.IsChanged = true; promotionLayout = value; } }
        public bool TopGameLayout { get { return topGameLayout; } set { this.IsChanged = true; topGameLayout = value; } }
        public bool TopEventLayout { get { return topEventLayout; } set { this.IsChanged = true; topEventLayout = value; } }
        public bool EnableNewCard { get { return enableNewCard; } set { this.IsChanged = true; enableNewCard = value; } }
        public bool EnableLinkCard { get { return enableLinkCard; } set { this.IsChanged = true; enableLinkCard = value; } }
        public bool EnableRedeemCard { get { return enableRedeemCard; } set { this.IsChanged = true; enableRedeemCard = value; } }
        public bool EnableRechargeCard { get { return enableRechargeCard; } set { this.IsChanged = true; enableRechargeCard = value; } }
        public bool EnableTransferCredits { get { return enableTransferCredits; } set { this.IsChanged = true; enableTransferCredits = value; } }
        public bool EnableLostCard { get { return enableLostCard; } set { this.IsChanged = true; enableLostCard = value; } }
        public int IdleTimeout { get { return idleTimeout; } set { this.IsChanged = true; idleTimeout = value; } }
        public int HQRefreshThreshold { get { return hqRefreshThreshold; } set { this.IsChanged = true; hqRefreshThreshold = value; } }
        public int AppValidityCheckDelay { get { return appValidityCheckDelay; } set { this.IsChanged = true; appValidityCheckDelay = value; } }
        public string CurrencyCode { get { return currencyCode; } set { this.IsChanged = true; currencyCode = value; } }
        public bool ShowCreditsOnCard { get { return showCreditsOnCard; } set { this.IsChanged = true; showCreditsOnCard = value; } }
        public bool ShowBonusOnCard { get { return showBonusOnCard; } set { this.IsChanged = true; showBonusOnCard = value; } }
        public bool ShowTimeOnCard { get { return showTimeOnCard; } set { this.IsChanged = true; showTimeOnCard = value; } }
        public bool ShowTicketsOnCard { get { return showTicketsOnCard; } set { this.IsChanged = true; showTicketsOnCard = value; } }
        public bool ShowLoyaltyOnCard { get { return showLoyaltyOnCard; } set { this.IsChanged = true; showLoyaltyOnCard = value; } }
        public bool ShowCourtesyOnCard { get { return showCourtesyOnCard; } set { this.IsChanged = true; showCourtesyOnCard = value; } }
        public List<LookupValuesDTO> CustomLinks { get { return customLinks; } set { this.IsChanged = true; customLinks = value; } }
        public bool EnableVirtualSite { get { return enableVirtualSite; } set { this.IsChanged = true; enableVirtualSite = value; } }
        public int VirtualSiteId { get { return virtualSiteId; } set { this.IsChanged = true; virtualSiteId = value; } }
        public bool EnableGamePlay { get { return enableGamePlay; } set { this.IsChanged = true; enableGamePlay = value; } }
        public string PlayBoxLink { get { return playBoxLink; } set { this.IsChanged = true; playBoxLink = value; } }
        public bool EnableMembership { get { return showMembershipOnCard; } set { this.IsChanged = true; showMembershipOnCard = value; } }
        public string NumberFormat { get { return numberFormat; } set { this.IsChanged = true; numberFormat = value; } }
        public string PromoImageFolderURL { get { return promoImageFolderURL; } set { this.IsChanged = true; promoImageFolderURL = value; } }
        public int AgeOfMajority { get { return ageOfMajority; } set { this.IsChanged = true; ageOfMajority = value; } }
        public int DefaultLanguage { get { return defaultLanguage; } set { this.IsChanged = true; defaultLanguage = value; } }
        public bool WaiverDeactivationNeedsManagerApproval { get { return waiverDeactivationNeedsManagerApproval; } set { this.IsChanged = true; waiverDeactivationNeedsManagerApproval = value; } }
        public bool WaiverCodeIsMandatoryToFetchCustomer { get { return waiverCodeIsMandatoryToFetchCustomer; } set { this.IsChanged = true; waiverCodeIsMandatoryToFetchCustomer = value; } }
        public bool CheckWaiverRegistrationCountForTransaction { get { return checkWaiverRegistrationCountForTransaction; } set { this.IsChanged = true; checkWaiverRegistrationCountForTransaction = value; } }
        public bool OtpCheckRequiredForWaiverRegistration { get { return otpCheckRequiredForWaiverRegistration; } set { this.IsChanged = true; otpCheckRequiredForWaiverRegistration = value; } }
        public int ValidityPeriodForWaiverRegistrationOTP { get { return validityPeriodForWaiverRegistrationOTP; } set { this.IsChanged = true; validityPeriodForWaiverRegistrationOTP = value; } }
        public bool EnableWaiverSignInKiosk { get { return enableWaiverSignInKiosk; } set { this.IsChanged = true; enableWaiverSignInKiosk = value; } }
        public bool WaiverOverrideNeedsManagerApproval { get { return waiverOverrideNeedsManagerApproval; } set { this.IsChanged = true; waiverOverrideNeedsManagerApproval = value; } }
        public bool AllowWaiverOverride { get { return allowWaiverOverride; } set { this.IsChanged = true; allowWaiverOverride = value; } }
        public bool OverrideSiteForTransaction { get { return overrideSiteForTransaction; } set { this.IsChanged = true; overrideSiteForTransaction = value; } }
        public string WaiverSetSelectionOptions { get { return waiverSetSelectionOptions; } set { this.IsChanged = true; waiverSetSelectionOptions = value; } }

        public bool ShowCustomAttributesOnWaiver { get { return showCustomAttributesOnWaiver; } set { this.IsChanged = true; showCustomAttributesOnWaiver = value; } }

        public string PromotionLayoutURL { get { return promotionLayoutURL; } set { this.IsChanged = true; promotionLayoutURL = value; } }
        public bool SocialLayout { get { return socialLayout; } set { this.IsChanged = true; socialLayout = value; } }
        public bool StoreLocatorLayout { get { return storeLocatorLayout; } set { this.IsChanged = true; storeLocatorLayout = value; } }
        public string StoreLocatorURL { get { return storeLocatorURL; } set { this.IsChanged = true; storeLocatorURL = value; } }
        public bool ShowCardExpiry { get { return showCardExpiry; } set { this.IsChanged = true; showCardExpiry = value; } }
        public bool ShowCardIssued { get { return showCardIssued; } set { this.IsChanged = true; showCardIssued = value; } }
        public string SupportContactNumber { get { return supportContactNumber; } set { this.IsChanged = true; supportContactNumber = value; } }
        public string SupportEmail { get { return supportEmail; } set { this.IsChanged = true; supportEmail = value; } }
        public string SupportMessageText { get { return supportMessageText; } set { this.IsChanged = true; supportMessageText = value; } }

        public List<CMSSocialLinksDTO> CMSSocialLinksDTO { get { return cMSSocialLinksDTO; } set { this.IsChanged = true; cMSSocialLinksDTO = value; } }


        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged;
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
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.Debug("Starts-AcceptChanges() Method.");
            this.IsChanged = false;
            log.Debug("Ends-AcceptChanges() Method.");
        }
    }
}
