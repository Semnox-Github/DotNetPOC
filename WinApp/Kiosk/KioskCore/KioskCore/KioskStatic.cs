/********************************************************************************************
* Project Name - Semnox.Parafait.KioskCore -KioskStatic
* Description  - KioskStatic 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.4.0       28-Sep-2018      Guru S A           Modified for Validty msg changes & Tax display
*2.4.0       25-Nov-2018      Raghuveera         LoadPromotionModeDropDown generic method added for all kiosk
* 2.60.0     13-Mar-2019      Raghuveera         Enable transfer config migrated from boolean to dropdown 
* 2.60.0     30-Apr-2019      Nitin Pai          Adding Face Value\Deposit to transactions
*2.70.2      3-Sep-2019       Deeksha            Added logger methods
*2.70.2      3-Dec-2019       Guru S A           Waiver phase 2 changes
*2.70.2      06-Nov-2019      Girish Kundar      Ticket printer integration enhancement
*2.70.3      11-Feb-2020      Deeksha            Invariant culture-Font Issue Fix
*2.80.1      02-Feb-2021      Deeksha            Theme changes to support customized Images/Font
*2.80.1      10-Nov-2020      Deeksha            Modified to display entitlement information message for payment mode / transaction screen
*2.90.0      30-Jun-2020      Dakshakh raj       Dynamic Payment Modes based on set up
*2.100.0     05-Aug-2020      Guru S A           Kiosk activity log changes
*2.110       21-Dec-2020      Jinto Thomas       Modified: As part of WristBand printer changes
*2.120       17-Apr-2021      Guru S A           Wristband printing flow enhancements
*2.130.0     01-Jul-2021      Dakshak            Theme changes to support customized Font ForeColor
*2.130.1     18-Sep-2021      Sathyavathi        Check-In Check-Out feature in Kiosk
*2.140.0     18-Sep-2021      Sathyavathi        Check-In Check-Out feature in Kiosk
*2.140.0     22-Oct-2021      Sathyavathi        CEC enhancement - Fund Raiser and Donations
*2.140.2     11-Mar-2022      Girish Kundar      Modified :  RFID  STIMA, BOCA printer - Factory implementation
*2.130.4     11-Mar-2022      Girish Kundar      Modified :  SHOW_VIRTUAL_POINTS configuration added to decide whether Virtual points to shown or not
*2.140.2     13-Jun-2022      Sathyavathi        VCAT integration in Kiosk reconciled from Fireball08
*2.130.11    13-Oct-2022      Vignesh Bhat       Ability to display background images for display group 
*2.150.0.0   23-Sep-2022      Sathyavathi        Check-In feature Phase-2
*2.150.0.0   10-Oct-2022      Sathyavathi        Mask card number
*2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
*2.155.0     20-Jun-2023      Sathyavathi        Attrcation Sale in Kiosk
*2.150.6     10-Nov-2023      Sathyavathi        Customer Lookup Enhancement
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;
using System.Collections.Generic;
using System.ComponentModel;//playpass:starts,Ends
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using System.Xml;
using System.Configuration;
using System.Linq;
using Semnox.Parafait.POS;
using Semnox.Parafait.Device;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Printer.WristBandPrinters.Boca;
using Semnox.Parafait.Device.Printer.WristBandPrinters;
using Semnox.Parafait.Device.Printer.FiscalPrint;
using System.Security.Permissions;
using Semnox.Parafait.logger;
using Semnox.Parafait.Transaction.TransactionFunctions;

namespace Semnox.Parafait.KioskCore
{
    public static class KioskStatic
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string shut_code;
        public static string shut_click;
        public static bool receipt = true;
        public static SqlDataReader rsshutcode;
        public static Semnox.Parafait.KioskCore.CoinAcceptor.CoinAcceptor.Models CoinAcceptorModel;
        public static Semnox.Parafait.KioskCore.CardDispenser.CardDispenser.Models CardDispenserModel;
        public static Semnox.Parafait.KioskCore.BillAcceptor.BillAcceptor.Models BillAcceptorModel;
        public static bool debugMode = false;
        public const string KIOSKMODEISCEC = "cec";
        public static System.IO.Ports.SerialPort spBillAcceptor;
        public static System.IO.Ports.SerialPort spCardDispenser;
        public static System.IO.Ports.SerialPort spReceiptPrinter;
        //public static System.IO.Ports.SerialPort spCoinAcceptor;
        public delegate void ReceiveAction();
        public static ReceiveAction cdReceiveAction = null;
        public static ReceiveAction baReceiveAction = null;
        public static ReceiveAction caReceiveAction = null;
        public static byte[] cardDispenserRec = new byte[22];
        public static byte[] billAcceptorRec = new byte[22];
        public static byte[] coinAcceptor_rec = new byte[22];
        public static bool billAcceptorDatareceived = false;
        public static bool coinAcceptorDatareceived = false;
        public static int MONEY_SCREEN_TIMEOUT;
        public static bool ENABLE_CLOSE_IN_READ_CARD_SCREEN = false;
        public static bool DISABLE_PURCHASE_ON_CARD_LOW_LEVEL = false;
        public static string CustomerScreenMessage1;
        public static string CustomerScreenMessage2;
        public static string VIPTerm;
        public static string ProductScreenGreeting;
        public static bool isUSBPrinter = false;
        public static bool AllowRegisterWithoutCard = false;
        public static DeviceClass DispenserReaderDevice;
        public static DeviceClass TopUpReaderDevice;
        public static Semnox.Parafait.KioskCore.CardAcceptor.CardAcceptor cardAcceptor;
        static string LogDirectory = Application.StartupPath + "\\log\\";
        public static Utilities Utilities;
        public static MSCPL_SDK.MicrocoinSDK microcoinSDK;
        public static ElementPSAdaper ElementPSAdapter;
        public static SCR200 SCR200;
        public static bool CCReversalNeededOnStartUp = false;
        public static bool PlayKioskAudio = false;
        public static string SiteHeading = "";
        public static bool ShowBonus = true;
        public static bool ShowGames = true;
        public static bool ShowCourtesy = true;
        public static bool ShowTickets = true;
        public static bool ShowLoyaltyPoints = true;
        public static bool ShowTime = false;
        public static bool ShowVirtualPoints = false;
        public static bool AllowOverPay = true;
        public static bool EnableTransfer = false;
        public static bool RegisterBeforePurchase = false;
        public static bool ShowRegistrationTAndC = false;
        public static bool ShowCheckInTAndC = false;
        public static bool EnableRedeemTokens = false;
        public static bool EnableFAQ = false;
        public static bool DisablePurchase = false;
        public static bool DisableNewCard = false;
        public static bool RegistrationAllowed = false;
        public static bool DisableCustomerRegistration = false;
        public static bool AllowPointsToTimeConversion = false;
        public static bool EnablePauseCard = false;
        public static bool EnableWaiverSignInKiosk = false;
        public static bool EnablePlaygroundEntry = true;

        static Utilities _utilities;//Quest:Start
        static object lastTrxId = null;

        public static bool questStatus = true;//Quest:Ends
        public static string showActivityDuration;//25-06-2015: changes for admin activity log option        
        public static ExternalPOSInterface.AlohaCheck alohaCheck = null;
        public static bool IS_ALOHA_ENV = false;
        public static bool IGNORE_THIRD_PARTY_SYNCH_ERROR = false;
        public static bool SPLIT_AND_MAP_VARIABLE_PRODUCT = true; // Added 08-Jun-2015 for Aloha update
        public static string KIOSK_CARD_VALUE_FORMAT;
        public static int CardDispenserAddress = 8;
        public static bool ALLOW_DECIMALS_IN_VARIABLE_RECHARGE = false;
        public static string TABLETOP_KIOSK_TYPE = "TABLETOP";
        public static int CreditCardReaderPortNo;
        public static double RegistrationBonusAmount = -1;
        public static double MAX_VARIABLE_RECHARGE_AMOUNT = 100;
        public static bool REVERSE_KIOSK_TOPUP_CARD_NUMBER = true;
        public static double TIME_IN_MINUTES_PER_CREDIT;
        private static ProductsDTO DepositProduct;
        public static List<PaymentModeDTO> paymentModeDTOList = null;
        public static string DateMonthFormat;
        public static bool enableDaySelection = true;
        public static bool enableYearSelection = true;
        public static bool enableMonthSelection = true;
        public static RichContentDTO RegistrationRichContentDTO;
        public static RichContentDTO CheckInRichContentDTO;
        private static SequencesDTO kioskTransactionSequenceDTO = null;
        public static string PAYMENT_GATEWAY_COMPONENT_RESTART_ERROR = "PAYMENT_GATEWAY_COMPONENT_RESTART_ERROR";
        public static string FUND_RAISER_AND_DONATIONS_LOOKUP = "FUND_RAISER_AND_DONATIONS";
        public static string FUND_RAISER_LOOKUP_VALUE = "Fund Raiser";
        public static string DONATIONS_LOOKUP_VALUE = "Donations";
        public static string TOPLEFT = "TopLeft";
        public static string TOPCENTER = "TopCenter";
        public static string TOPRIGHT = "TopRight";
        public static string MIDDLELEFT = "MiddleLeft";
        public static string MIDDLECENTER = "MiddleCenter";
        public static string MIDDLERIGHT = "MiddleRight";
        public static string BOTTOMLEFT = "BottomLeft";
        public static string BOTTOMCENTER = "BottomCenter";
        public static string BOTTOMRIGHT = "BottomRight";
        public static string OPT1 = "OPT1";
        public static string OPT2 = "OPT2";
        public static bool MASKCARDNUMBER = true;
        public static int LASTXDIGITSTOSHOW = 4;
        public static decimal AGE_LOWER_LIMIT = 0;  //as per productContainerDTO definition
        public static decimal AGE_UPPER_LIMIT = 999;  //as per productContainerDTO definition
        public static string NOTE = "N";
        public static string COIN = "C";
        public static string TOKEN = "T";

        private static List<MasterScheduleBL> masterScheduleBLList = null;
        public static List<MasterScheduleBL> MasterScheduleBLList { get { return masterScheduleBLList; } set { masterScheduleBLList = value; } }

        public enum ApplicationContentModule
        {
            REGISTRATION = 1,
            CHECKIN = 2
        };

        public static ProductsDTO GetDepositProduct()
        {
            log.LogMethodEntry();
            if (DepositProduct == null)
            {
                Products depositProductBL = new Products(Utilities.ParafaitEnv.CardDepositProductId);
                DepositProduct = depositProductBL.GetProductsDTO;
            }
            log.LogMethodExit(DepositProduct);
            return DepositProduct;
        }

        public static POSMachineDTO POSMachineDTO;
        public static List<POSPaymentModeInclusionDTO> pOSPaymentModeInclusionDTOList = new List<POSPaymentModeInclusionDTO>();
        public static List<POSPaymentModeInclusionDTO> errorPayentModeDTOList = new List<POSPaymentModeInclusionDTO>();

        public static Device.Printer.FiscalPrint.FiscalPrinter fiscalPrinter;
        public static string PdfViewSettings = "#zoom=100&toolbar=0&statusbar=0&messages=0&navpanes=0";

        public static string[] CustomizedTagList = {
                                            "HomeScreenOptionsTextForeColor",
                                            "HomeScreenBtnLanguageTextForeColor",
                                            "HomeScreenLanguageListForeColor",
                                            "HomeScreenBtnTermsTextForeColor",
                                            "HomeScreenFooterTextForeColor",
                                            "HomeScreenBtnRegisterTextForeColor",
                                            "HomeScreenBtnCheckBalanceTextForeColor",
                                            "HomeScreenBtnNewCardTextForeColor",
                                            "HomeScreenBtnPurchaseTextForeColor",
                                            "HomeScreenBtnRechargeTextForeColor",
                                            "HomeScreenBtnFoodAndBeveragesTextForeColor",
                                            "HomeScreenBtnTransferTextForeColor",
                                            "HomeScreenBtnRedeemTokensTextForeColor",
                                            "HomeScreenBtnPauseTimeTextForeColor",
                                            "HomeScreenBtnPointsToTimeTextForeColor",
                                            "HomeScreenBtnHomeTextForeColor",
                                            "HomeScreenBtnSignWaiverTextForeColor",
                                            "HomeScreenBtnPlaygroundEntryTextForeColor",
                                            "HomeScreenBtnAttractionsTextForeColor",

                                            "DisplayGroupBtnTextForeColor",
                                            "DisplayGroupCancelBtnTextForeColor",
                                            "DisplayGroupBackBtnTextForeColor",
                                            "DisplayGroupFooterTextForeColor",
                                            "DisplayGroupGreetingsTextForeColor",

                                            "ChooseProductsBtnTextForeColor",
                                            "ChooseProductsCancelBtnTextForeColor",
                                            "ChooseProductVariableBtnTextForeColor",
                                            "ChooseProductsBackBtnTextForeColor",
                                            "ChooseProductsVariableBtnTextForeColor",
                                            "ChooseProductsFooterTextForeColor",
                                            "ChooseProductsGreetingsTextForeColor",
                                            "ChooseProductsBtnHomeTextForeColor",

                                            "TapCardScreenHeaderTextForeColor",
                                            "TapCardScreenNewCardTextForeColor",
                                            "TapCardScreenYesBtnTextForeColor",
                                            "TapCardScreenCloseBtnTextForeColor",
                                            "TapCardScreenNoBtnTextForeColor",
                                            "TapCardScreenButton1TextForeColor",
                                            "TapCardScreenNoteTextForeColor",
                                            "TapCardScreenLblMessageTextForeColor",

                                            "PaymentModeBtnTextForeColor",
                                            "PaymentModeSummaryHeaderTextForeColor",
                                            "PaymentModeDepositeHeaderTextForeColor",
                                            "PaymentModeDiscountHeaderTextForeColor",
                                            "PaymentModeSummaryInfoTextForeColor",
                                            "PaymentModeDepositeInfoTextForeColor",
                                            "PaymentModeDiscountInfoTextForeColor",
                                            "PaymentModeTotalToPayHeaderTextForeColor",
                                            "PaymentModeTotalToPayInfoTextForeColor",
                                            "PaymentModeDiscountBtnTextForeColor",
                                            "PaymentModeCPValidityTextForeColor",
                                            "PaymentModeBackButtonTextForeColor",
                                            "PaymentModeCancelButtonTextForeColor",
                                            "PaymentModeButtonsTextForeColor",
                                            "PaymentModeLblHeadingTextForeColor",
                                            "PaymentModeLblCardnumberTextForeColor",
                                            "PaymentModeLblPackageTextForeColor",
                                            "PaymentModeDiscountLabelTextForeColor",
                                            "PaymentModeLblPriceOfCardDepositTextForeColor",
                                            "PaymentModeTextMessageTextForeColor",
                                            "PaymentModeBtnHomeTextForeColor",
                                            "PaymentModeLblPassportCouponTextForeColor",

                                            "YesNoScreenMessageTextForeColor",
                                            "YesNoScreenBtnYesTextForeColor",
                                            "YesNoScreenBtnNoTextForeColor",
                                            "YesNoScreenLblAdditionalMessageTextForeColor",

                                            "TransferFormTapMessageTextForeColor",
                                            "TransferFormFooterTextForeColor",
                                            "TransferFormBtnPrevTextForeColor",
                                            "TransferFormBtnNextTextForeColor",
                                            "TransferFormCardDetailsHeaderTextForeColor",
                                            "TransferFormCardDetailsInfoTextForeColor",
                                            "TransferFormTimeRemainingTextForeColor",

                                            "FAQScreenHeaderTextForeColor",
                                            "FAQScreenBtnTermsTextForeColor",
                                            "FAQScreenBtnBackTextForeColor",

                                            "PauseTimeLblMessageTextForeColor",
                                            "PauseTimeCardNumberHeaderTextForeColor",
                                            "PauseTimeCardNumberInfoTextForeColor",
                                            "PauseTimeTimeHeaderTextForeColor",
                                            "PauseTimeTimeInfoTextForeColor",
                                            "PauseTimeETicketHeaderTextForeColor",
                                            "PauseTimeETicketInfoTextForeColor",
                                            "PauseTimeBackBtnTextForeColor",
                                            "PauseTimeOkBtnTextForeColor",

                                            "CardCountScreenHeader1TextForeColor",
                                            "CardCountScreenHeader2TextForeColor",
                                            "CardCountScreenCardBtnsTextForeColor",
                                            "CardCountScreenBackBtnTextForeColor",
                                            "CardCountScreenCancelBtnTextForeColor",

                                            "AdminScreenBtnCancelTextForeColor",
                                            "AdminScreenBtnExitTextForeColor",
                                            "AdminScreenBtnSetupTextForeColor",
                                            "AdminScreenBtnLoadBonusTextForeColor",
                                            "AdminScreenBtnPrintSummaryTextForeColor",
                                            "AdminScreenBtnKioskActivityTextForeColor",
                                            "AdminScreenBtnRebootComputerTextForeColor",
                                            "AdminScreenBtnTrxViewTextForeColor",

                                            "CashInsertScreenHeader1TextForeColor",
                                            "CashInsertScreenHeader2TextForeColor",
                                            "CashInsertScreenCashInsertedInfoTextForeColor",
                                            "CashInsertScreenBtnNewCardTextForeColor",
                                            "CashInsertScreenBtnRechargeTextForeColor",
                                            "CashInsertScreenHeader3TextForeColor",
                                            "CashInsertScreenDenominationHeaderTextForeColor",
                                            "CashInsertScreenQuantityHeaderTextForeColor",
                                            "CashInsertScreenPointsHeaderTextForeColor",
                                            "CashInsertScreenCancelButtonTextForeColor",

                                            "AgeGateScreenHeader1TextForeColor",
                                            "AgeGateScreenHeader2TextForeColor",
                                            "AgeGateScreenHeader3TextForeColor",
                                            "AgeGateScreenMonthTextBoxTextForeColor",
                                            "AgeGateScreenDateTextBoxTextForeColor",
                                            "AgeGateScreenYearTextBoxTextForeColor",
                                            "AgeGateScreenDateFormatTextForeColor",
                                            "AgeGateScreenReadConfirmTextForeColor",
                                            "AgeGateScreenlnkTermsTextForeColor",
                                            "AgeGateScreenBtnCancelTextForeColor",
                                            "AgeGateScreenBtnNextTextForeColor",

                                            "CustomerScreenHeader1TextForeColor",
                                            "CustomerScreenHeader2TextForeColor",
                                            "CustomerScreenDetailsHeaderTextForeColor",
                                            "CustomerScreenInfoTextForeColor",
                                            "CustomerScreenOptInTextForeColor",
                                            "CustomerScreenTermsAndConditionsTextForeColor",
                                            "CustomerScreenPrivacyTextForeColor",
                                            "CustomerScreenInkTermsTextForeColor",
                                            "CustomerScreenBtnPrevTextForeColor",
                                            "CustomerScreenBtnSaveTextForeColor",
                                            "CustomerScreenFooterTextForeColor",
                                            "CustomerScreenLblTimeRemainingTextForeColor",
                                            "CustomerScreenBtnHomeTextForeColor",
                                            "CustomerScreenDgvLinkedRelationsTextForeColor",
                                            "CustomerScreenLblLinkedRelationsTextForeColor",
                                            "CustomerScreenBtnAddRelationTextForeColor",
                                            "CustomerScreenDgvLinkedRelationsHeaderTextForeColor",
                                            "CustomerScreenWhatsAppOptOutTextForeColor",
                                            "CustomerScreenPhotoTextForeColor",
                                            "CustomerScreenDgvLinkedRelationsInfoTextForeColor",

                                            "CheckBalanceHeader1TextForeColor",
                                            "CheckBalanceCardNumerHeaderTextForeColor",
                                            "CheckBalanceCardNumberInfoTextForeColor",
                                            "CheckBalanceTicketModeHeaderTextForeColor",
                                            "CheckBalanceTicketModeInfoTextForeColor",
                                            "CheckBalanceBtnChangeTextForeColor",
                                            "CheckBalancePlayValueHeaderTextForeColor",
                                            "CheckBalancePlayValueInfoTextForeColor",
                                            "CheckBalanceBonusHeaderTextForeColor",
                                            "CheckBalanceBonusInfoTextForeColor",
                                            "CheckBalanceVirtualPointInfoTextForeColor",
                                            "CheckBalanceVirtualPointHeaderTextForeColor",
                                            "CheckBalanceCourtesyHeaderTextForeColor",
                                            "CheckBalanceCourtesyInfoTextForeColor",
                                            "CheckBalanceTimeHeaderTextForeColor",
                                            "CheckBalanceTimeInfoTextForeColor",
                                            "CheckBalanceLPHeaderTextForeColor",
                                            "CheckBalanceLPInfoTextForeColor",
                                            "CheckBalanceGameHeaderTextForeColor",
                                            "CheckBalanceGameInfoTextForeColor",
                                            "CheckBalanceETicketHeaderTextForeColor",
                                            "CheckBalanceETicketInfoTextForeColor",
                                            "CheckBalanceBackButtonTextForeColor",
                                            "CheckBalanceTopUpButtonTextForeColor",
                                            "CheckBalanceActivityButtonTextForeColor",
                                            "CheckBalanceFooterTextForeColor",
                                            "CheckBalanceTimeOutTextForeColor",
                                            "CheckBalanceBtnViewSignedWaiversTextForeColor",
                                            "CheckBalanceBtnHomeTextForeColor",

                                            "CustomerDashboardCardNumberTextForeColor",
                                            "CustomerDashboardCardNumberInfoTextForeColor",
                                            "CustomerDashboardTicketModeTextForeColor",
                                            "CustomerDashboardTicketModeInfoTextForeColor",
                                            "CustomerDashboardChangeButtonTextForeColor",
                                            "CustomerDashboardHeader1TextForeColor",
                                            "CustomerDashboardHeader1DetailsTextForeColor",
                                            "CustomerDashboardHeader2TextForeColor",
                                            "CustomerDashboardHeader2DetailsTextForeColor",
                                            "CustomerDashboardBackBtnTextForeColor",
                                            "CustomerDashboardTopUpBtnTextForeColor",
                                            "CustomerDashboardRegisterBtnTextForeColor",
                                            "CustomerDashboardButton1TextForeColor",
                                            "CustomerDashboardLblPlayValueLabelTextForeColor",
                                            "CustomerDashboardLblCreditsTextForeColor",
                                            "CustomerDashboardLblTimeOutTextForeColor",
                                            "CustomerDashboardBtnHomeTextForeColor",
                                            "CustomerDashboardInfoTextForeColor",

                                            "CreditsToTimeHeaderTextForeColor",
                                            "CreditsToTimeDetailsHeaderTextForeColor",
                                            "CreditsToTimeDetailsInfoTextForeColor",
                                            "CreditsToTimeBtnPrevTextForeColor",
                                            "CreditsToTimeBtnSaveTextForeColor",
                                            "CreditsToTimeAfterSaveHeaderTextForeColor",
                                            "CreditsToTimeAfterSaveInfoTextForeColor",
                                            "CreditsToTimeFooterTextForeColor",
                                            "CreditsToTimeTimeRemainingTextForeColor",
                                            "CreditsToTimeBtnHomeTextForeColor",

                                            "RedeemTokensScreenHeaderTextForeColor",
                                            "RedeemTokensScreenTokenInsertedTextForeColor",
                                            "RedeemTokensScreenAvialableTokensTextForeColor",
                                            "RedeemTokensScreenBtnNewCardTextForeColor",
                                            "RedeemTokensScreenBtnLoadTextForeColor",
                                            "RedeemTokensScreenBtnBackTextForeColor",
                                            "RedeemTokensScreenFooterTextForeColor",
                                            "RedeemTokensScreenDenominationTextForeColor",
                                            "RedeemTokensScreenQuantityTextForeColor",
                                            "RedeemTokensScreenPointsTextForeColor",
                                            "RedeemTokensScreenTimeOutTextForeColor",
                                            "RedeemTokensScreenButton1extForeColor",
                                            "RedeemTokensScreenBtnHomeTextForeColor",

                                            "TransferToScreenHeaderTextForeColor",
                                            "TransferToScreenTimeRemainingTextForeColor",
                                            "TransferToScreenCardsHeaderTextForeColor",
                                            "TransferToScreenCreditsHeaderTextForeColor",
                                            "TransferToScreenPointsHeaderTextForeColor",
                                            "TransferToScreenCardsInfoTextForeColor",
                                            "TransferToScreenAvlTokensInfoTextForeColor",
                                            "TransferToScreenTransTokensInfoTextForeColor",
                                            "TransferToScreenBtnPrevTextForeColor",
                                            "TransferToScreenBtnNextTextForeColor",
                                            "TransferToScreenTransfererHeaderTextForeColor",
                                            "TransferToScreenTransfereeHeaderTextForeColor",
                                            "TransferToScreenCardsHeader2TextForeColor",
                                            "TransferToScreenCreditsHeader2TextForeColor",
                                            "TransferToScreenCardsInfo2TextForeColor",
                                            "TransferToScreenCreditsInfo2TextForeColor",
                                            "TransferToScreenFooterTextForeColor",

                                            "CardGamesPrevTextForeColor",
                                            "CardGamesdgvCardGamesForeColor",
                                            "CardGamesdgvCardGamesHeaderForeColor",
                                            "CardGamesBackColor",

                                            "RegisterTnCHeaderTextForeColor",
                                            "RegisterTnCBtnYesTextForeColor",
                                            "RegisterTnCBtnCancelTextForeColor",
                                            "RegisterTnCBtnPrevTextForeColor",

                                            "TicketTypeHeaderTextForeColor",
                                            "TicketTypeOption1TextForeColor",
                                            "TicketTypeOption2TextForeColor",
                                            "TicketTypeBtnOkTextForeColor",
                                            "TicketTypeBtnCancelTextForeColor",

                                            "CardCountBasicsHeader1TextForeColor",
                                            "CardCountBasicsHeader2TextForeColor",
                                            "CardCountBasicsHeader3TextForeColor",
                                            "CardCountBasicsBtnOneTextForeColor",
                                            "CardCountBasicsBtnTwoTextForeColor",
                                            "CardCountBasicsBtnThreeTextForeColor",
                                            "CardCountBasicsBtnPrevTextForeColor",

                                            "KioskActivityHeaderTextForeColor",
                                            "KioskActivityDetailsHeaderTextForeColor",
                                            "KioskActivityBtnPrevTextForeColor",
                                            "KioskActivityDgvKioskActivityTextForeColor",
                                            "KioskActivityTxtMessageTextForeColor",
                                            "KioskActivityDgvKioskActivityHeaderTextForeColor",
                                            "KioskActivityDgvKioskActivityInfoTextForeColor",

                                            "CardTransactionTimeOutTextForeColor",
                                            "CardTransactionSummaryHeaderTextForeColor",
                                            "CardTransactionSummaryInfoTextForeColor",
                                            "CardTransactionDepositeHeaderTextForeColor",
                                            "CardTransactionDepositeInfoTextForeColor",
                                            "CardTransactionDiscountHeaderTextForeColor",
                                            "CardTransactionDiscountInfoTextForeColor",
                                            "CardTransactionFundHeaderTextForeColor",
                                            "CardTransactionFundInfoTextForeColor",
                                            "CardTransactionDonationHeaderTextForeColor",
                                            "CardTransactionDonationInfoTextForeColor",
                                            "CardTransactionTotalToPayHeaderTextForeColor",
                                            "CardTransactionTotalToPayInfoTextForeColor",
                                            "CardTransactionAmountPaidHeaderTextForeColor",
                                            "CardTransactionAmountPaidInfoTextForeColor",
                                            "CardTransactionBalanceToPayHeaderTextForeColor",
                                            "CardTransactionBalanceToPayInfoTextForeColor",
                                            "CardTransactionCPVlaiditTextForeColor",
                                            "CardTransactionCancelButtonTextForeColor",
                                            "CardTransactionBtnCreditTextForeColor",
                                            "CardTransactionBtnDebitTextForeColor",
                                            "CardTransactionFooterTextForeColor",
                                            "CardTransactionBtnHomeTextForeColor",
                                            "CardTransactionPaymentButtonsTextForeColor",
                                            "CardTransactionLblTimeOutTextForeColor",

                                            "LoadBonusHeaderTextForeColor",
                                            "LoadBonusCardHeaderTextForeColor",
                                            "LoadBonusCreditsHeaderTextForeColor",
                                            "LoadBonusBonusHeaderTextForeColor",
                                            "LoadBonusCardInfoTextForeColor",
                                            "LoadBonusCreditsInfoTextForeColor",
                                            "LoadBonusBonusInfoTextForeColor",
                                            "LoadBonusFooterTextForeColor",
                                            "LoadBonusBtnSaveTextForeColor",
                                            "LoadBonusBtnPrevTextForeColor",

                                            "ThankYouScreenHeader1TextForeColor",
                                            "ThankYouScreenHeader2TextForeColor",
                                            "ThankYouScreenBtnPrevTextForeColor",

                                            "UpsellProductScreenTimeOutTextForeColor",
                                            "UpsellProductScreenHeader1TextForeColor",
                                            "UpsellProductScreenHeader2TextForeColor",
                                            "UpsellProductScreenDesc1TextForeColor",
                                            "UpsellProductScreenDesc2TextForeColor",
                                            "UpsellProductScreenDesc3TextForeColor",
                                            "UpsellProductScreenBtnYesTextForeColor",
                                            "UpsellProductScreenBtnNoTextForeColor",
                                            "UpsellProductScreenFooterTextForeColor",
                                            "UpsellProductScreenBtnHomeTextForeColor",

                                            "ChooseEntitlementHeaderTextForeColor",
                                            "ChooseEntitlementBtnTimeTextForeColor",
                                            "ChooseEntitlemenBtnPointsTextForeColor",
                                            "ChooseEntitlementBtnOkTextForeColor",

                                            "OkMsgScreenHeaderTextForeColor",
                                            "OkMsgScreenBtnCloseTextForeColor",

                                            "ScanCouponScreenHeaderTextForeColor",
                                            "ScanCouponScreenCouponHeaderTextForeColor",
                                            "ScanCouponScreenCouponInfoTextForeColor",
                                            "ScanCouponScreenBtnApplyTextForeColor",
                                            "ScanCouponScreenBtnCloseTextForeColor",

                                            "TimeOutCounterHeader1TextForeColor",
                                            "TimeOutCounterTimerTextForeColor",
                                            "TimeOutCounterHeader2TextForeColor",

                                            "DgvKioskTransactionsTextForeColor",
                                            "TxtTrxIdTextForeColor",
                                            "TxtToTimeHrsTextForeColor",
                                            "TxtToTimeMinsTextForeColor",
                                            "TxtFromTimeHrsTextForeColor",
                                            "TxtFromTimeMinsTextForeColor",
                                            "CmbFromTimeTTTextForeColor",
                                            "CmbToTimeTTTextForeColor",
                                            "CmbPosMachinesTextForeColor",
                                            "DgvKioskTransactionsControlTextForeColor",
                                            "KioskTransactionViewTxtMessageTextForeColor",
                                            "KioskTransactionViewSiteTextForeColor",

                                            "FrmKioskTransViewLblGreetingTextForeColor",
                                            "FrmKioskTransViewLblFromDateTextForeColor",
                                            "FrmKioskTransViewTxtFromTimeHrsTextForeColor",
                                            "FrmKioskTransViewTxtFromTimeMinsTextForeColor",
                                            "FrmKioskTransViewCmbFromTimeTTTextForeColor",
                                            "FrmKioskTransViewLabel1TextForeColor",
                                            "FrmKioskTransViewLblPosMachinesTextForeColor",
                                            "FrmKioskTransViewTxtToTimeHrsTextForeColor",
                                            "FrmKioskTransViewTxtToTimeMinsTextForeColor",
                                            "FrmKioskTransViewCmbToTimeTTTextForeColor",
                                            "FrmKioskTransViewCmbPosMachinesTextForeColor",
                                            "FrmKioskTransViewLblTrxIdTextForeColor",
                                            "FrmKioskTransViewTxtTrxIdTextForeColor",
                                            "FrmKioskTransViewBtnSearchTextForeColor",
                                            "FrmKioskTransViewBtnClearTextForeColor",
                                            "FrmKioskTransViewLblTransactionTextForeColor",
                                            "FrmKioskTransViewDgvKioskTransactionsTextForeColor",
                                            "FrmKioskTransViewTextBtnPrevForeColor",
                                            "FrmKioskTransViewTextBtnPrintReceiptForeColor",
                                            "FrmKioskTransViewTextBtnRefundForeColor",
                                            "FrmKioskTransViewTextBtnPrintPendingForeColor",
                                            "FrmKioskTransViewTextBtnIssueTempCardForeColor",
                                            "FrmKioskTransViewTextTxtMessageForeColor",
                                            "FrmKioskTransactionViewSiteTextForeColor",
                                            "FrmKioskTransViewDgvColumnHeadersTextForeColor",

                                            "DgvTransactionHeaderTextForeColor",
                                            "DgvPrintedTransactionLinesTextForeColor",
                                            "DgvPendingPrintTransactionLinesTextForeColor",
                                            "CmbPrintReasonTextForeColor",
                                            "FrmPrintTransactionLinesTxtMessageTextForeColor",
                                            "DgvTransactionHeaderControlsTextForeColor",
                                            "DgvPrintedTransactionLinesControlsTextForeColor",
                                            "DgvPendingPrintTransactionLinesControlsTextForeColor",

                                            "FrmPrintTransLblGreetingTextForeColor",
                                            "FrmPrintTransLblCardsPendingPrintTextForeColor",
                                            "FrmPrintTransLblPrintedCardsTextForeColor",
                                            "FrmPrintTransLblTransactionDetailsTextForeColor",
                                            "FrmPrintTransTxtMessageTextForeColor",
                                            "FrmPrintTransDgvTransactionHeaderTextForeColor",
                                            "FrmPrintTransDgvPendingPrintTransactionLinesTextForeColor",
                                            "FrmPrintTransDgvPrintedTransactionLinesTextForeColor",
                                            "FrmPrintTransLblPrintReasonTextForeColor",
                                            "FrmPrintTransCmbPrintReasonTextForeColor",
                                            "FrmPrintTransBtnCancelTextForeColor",
                                            "FrmPrintTransBtnPrintPendingTextForeColor",
                                            "FrmPrintTransLblSiteNameTextForeColor",
                                            "FrmPrintTransDgvTransactionHeaderControlsTextForeColor",
                                            "FrmPrintTransDgvPrintedTransactionLinesControlsTextForeColor",
                                            "FrmPrintTransDgvPendingPrintTransactionLinesControlsTextForeColor",
                                            "FrmPrintTransDgvTransactionHeaderHeaderTextForeColor",
                                            "FrmPrintTransDgvPendingPrintTransactionLinesHeaderTextForeColor",
                                            "FrmPrintTransDgvPrintedTransactionLinesHeaderTextForeColor",

                                            "FrmCashTransactionBtnHomeTextForeColor",
                                            "FrmCashTransactionButton1TextForeColor",
                                            "FrmCashTransactionLblTimeOutTextForeColor",
                                            "FrmCashTransactionLabel6TextForeColor",
                                            "FrmCashTransactionLabel1TextForeColor",
                                            "FrmCashTransactionLblTotalToPayTextForeColor",
                                            "FrmCashTransactionLabel8TextForeColor",
                                            "FrmCashTransactionLabel9TextForeColor",
                                            "FrmCashTransactionLblPaidTextForeColor",
                                            "FrmCashTransactionLblBalTextForeColor",
                                            "FrmCashTransactionLblProductCPValidityMsgTextForeColor",
                                            "FrmCashTransactionLblNoChangeTextForeColor",
                                            "FrmCashTransactionBtnCreditCardTextForeColor",
                                            "FrmCashTransactionBtnDebitCardTextForeColor",
                                            "FrmCashTransactionLblSiteNameTextForeColor",
                                            "FrmCashTransactionTxtMessageTextForeColor",

                                            "FskExecutePnlPurchaseDetailsHeadersTextForeColor",
                                            "FskExecuteInfoTextForeColor",
                                            "FskExecutePnlPurchaseHeaderTextForeColor",
                                            "FskExecuteOnlineLblSiteNameTextForeColor",
                                            "FskExecuteOnlineLblTransactionOTPTextForeColor",
                                            "FskExecuteOnlineTxtTransactionOTPTextForeColor",
                                            "FskExecuteOnlineLblTransactionIdTextForeColor",
                                            "FskExecuteOnlinePnlTrasactionIdTextForeColor",
                                            "FskExecuteOnlineBtnGetTransactionDetailsTextForeColor",
                                            "FskExecuteOnlineBtnCancelTextForeColor",
                                            "FskExecuteOnlineTxtMessageTextForeColor",
                                            "FskExecuteOnlineTransactionHeaderInfoTextForeColor",
                                            "FskExecuteOnlineTransactionLinesInfoTextForeColor",

                                            "FrmCustWaiverHeadersTextForeColor",
                                            "FrmCustWaiverDgvCustomerTextForeColor",
                                            "FrmCustWaiverBtnHomeTextForeColor",
                                            "FrmCustWaiverLblSignatoryCustomerNameTextForeColor",
                                            "FrmCustWaiverLblWaiverSetTextForeColor",
                                            "FrmCustWaiverLabel2TextForeColor",
                                            "FrmCustWaiverBtnCancelTextForeColor",
                                            "FrmCustWaiverBtnAddNewRelationsTextForeColor",
                                            "FrmCustWaiverBtnProceedTextForeColor",
                                            "FrmCustWaiverTxtMessageTextForeColor",

                                            "FrmCustOptionBtnHomeTextForeColor",
                                            "FrmCustOptionBtnNewRegistrationTextForeColor",
                                            "FrmCustOptionBtnExistingCustomerTextForeColor",
                                            "FrmCustOptionBtnCancelTextForeColor",
                                            "FrmCustOptionTxtMessageTextForeColor",

                                            "FrmCustOTPLblOTPTextForeColor",
                                            "FrmCustOTPLblOTPmsgTextForeColor",
                                            "FrmCustOTPLblTimeRemainingTextForeColor",
                                            "FrmCustOTPTxtOTPTextForeColor",
                                            "FrmCustOTPLinkLblResendOTPTextForeColor",
                                            "FrmCustOTPBtnOkayTextForeColor",
                                            "FrmCustOTPBtnCancelTextForeColor",
                                            "FrmCustOTPTxtMessageTextForeColor",

                                            "FrmCustSignConfirmationDgvCustomerSignedWaiverTextForeColor",
                                            "FrmCustSignConfirmationLblCustomerNameTextForeColor",
                                            "FrmCustSignConfirmationChkSignConfirmTextForeColor",
                                            "FrmCustSignConfirmationPbCheckBoxTextForeColor",
                                            "FrmCustSignConfirmationBtnOkayTextForeColor",
                                            "FrmCustSignConfirmationBtnCancelTextForeColor",
                                            "FrmCustSignConfirmationDgvCustomerSignedWaiverHeaderTextForeColor",

                                            "FrmGetCustInputLabel1TextForeColor",
                                            "FrmGetCustInputLabel2TextForeColor",
                                            "FrmGetCustInputLabel3TextForeColor",
                                            "FrmGetCustInputLblmsgTextForeColor",
                                            "FrmGetCustInputLmlEmailTextForeColor",
                                            "FrmGetCustInputTxtEmailTextForeColor",
                                            "FrmGetCustInputBtnOkTextForeColor",
                                            "FrmGetCustInputBtnCancelTextForeColor",
                                            "FrmGetCustInputTxtMessageTextForeColor",

                                            "FrmLinkCustLblCustomerTextForeColor",
                                            "FrmLinkCustLblRelatedCustomerTextForeColor",
                                            "FrmLinkCustLblCustomerValueTextForeColor",
                                            "FrmLinkCustLblRelatedCustomerValueTextForeColor",
                                            "FrmLinkCustLabel2TextForeColor",
                                            "FrmLinkCustCmbRelationTextForeColor",
                                            "FrmLinkCustBtnSaveTextForeColor",
                                            "FrmLinkCustBtnCancelTextForeColor",
                                            "FrmLinkCustTxtMessageTextForeColor",

                                            "FrmSelectWaiverBtnHomeTextForeColor",
                                            "FrmSelectWaiverLabel1TextForeColor",
                                            "FrmSelectWaiverLblCustomerTextForeColor",
                                            "FrmSelectWaiverLblReservationCodeTextForeColor",
                                            "FrmSelectWaiverTxtReservationCodeTextForeColor",
                                            "FrmSelectWaiverLblReservationCodeORTextForeColor",
                                            "FrmSelectWaiverLblTrxOTPTextForeColor",
                                            "FrmSelectWaiverTxtTrxOTPTextForeColor",
                                            "FrmSelectWaiverLabel4TextForeColor",
                                            "FrmSelectWaiverLabel7TextForeColor",
                                            "FrmSelectWaiverLblWaiverTextForeColor",
                                            "FrmSelectWaiverLblSelectionTextForeColor",
                                            "FrmSelectWaiverDgvWaiverSetTextForeColor",
                                            "FrmSelectWaiverBtnProceedTextForeColor",
                                            "FrmSelectWaiverBtnCancelTextForeColor",
                                            "FrmSelectWaiverTxtMessageTextForeColor",

                                            "FrmSignWaiverFilesBtnHomeTextForeColor",
                                            "FrmSignWaiverFilesLblSignWaiverFileTextForeColor",
                                            "FrmSignWaiverFilesLblCustomerNameTextForeColor",
                                            "FrmSignWaiverFilesLblCustomerContactTextForeColor",
                                            "FrmSignWaiverFilesPnlWaiverTextForeColor",
                                            "FrmSignWaiverFilesPnlWaiverDisplayTextForeColor",
                                            "FrmSignWaiverFilesBtnOkayTextForeColor",
                                            "FrmSignWaiverFilesBtnCancelTextForeColor",
                                            "FrmSignWaiverFilesTxtMessageTextForeColor",
                                            "FrmSignWaiverFilesChkSignConfirmTextForeColor",

                                            "FrmSignWaiversBtnHomeTextForeColor",
                                            "FrmSignWaiversLblSiteNameTextForeColor",
                                            "FrmSignWaiversLblCustomerTextForeColor",
                                            "FrmSignWaiversLblGreeting1TextForeColor",
                                            "FrmSignWaiversLblWaiverTextForeColor",
                                            "FrmSignWaiversLblSelectionTextForeColor",
                                            "FrmSignWaiversLabel1TextForeColor",
                                            "FrmSignWaiversFpnlWaiverSetTextForeColor",
                                            "FrmSignWaiversBtnCancelTextForeColor",
                                            "FrmSignWaiversTxtMessageTextForeColor",

                                            "FrmViewWaiverUIBtnHomeTextForeColor",
                                            "FrmViewWaiverUILblWaiverNameTextForeColor",
                                            "FrmViewWaiverUIWebBrowserTextForeColor",
                                            "FrmViewWaiverUIBtnCloseTextForeColor",

                                            "FrmEntitlementLblMsgTextForeColor",
                                            "FrmEntitlementBtnMsgTextForeColor",
                                            "FrmEntitlementBtnPointsTextForeColor",

                                            "FrmSplashScreenLabel1TextForeColor",

                                            "FrmSuccessMsgBtnHomeTextForeColor",
                                            "FrmSuccessMsgLblHeadingTextForeColor",
                                            "FrmSuccessMsgLblmsgTextForeColor",
                                            "FrmSuccessMsgLblBalanceMsgTextForeColor",
                                            "FrmSuccessMsgLblPointTextForeColor",
                                            "FrmSuccessMsgLblPasNoTextForeColor",
                                            "FrmSuccessMsgBtnCloseTextForeColor",
                                            "FrmSuccessMsgLblTrxNumberTextForeColor",

                                            "FSKCoverPageBtnExecuteOnlineTransactionTextForeColor",

                                            "PaymentGameCardLblSiteNameTextForeColor",
                                            "PaymentGameCardLblPaymentTextTextForeColor",
                                            "PaymentGameCardLblTotaltoPayTextTextForeColor",
                                            "PaymentGameCardLblTotalToPayTextForeColor",
                                            "PaymentGameCardLabel8TextForeColor",
                                            "PaymentGameCardLabel7TextForeColor",
                                            "PaymentGameCardLblTapCardTextTextForeColor",
                                            "PaymentGameCardLblCardNumberTextTextForeColor",
                                            "PaymentGameCardLblCardNumberTextForeColor",
                                            "PaymentGameCardLblAvailableCreditsTextTextForeColor",
                                            "PaymentGameCardLblAvailableCreditsTextForeColor",
                                            "PaymentGameCardLblPurchaseValueTextTextForeColor",
                                            "PaymentGameCardLblPurchaseValueTextForeColor",
                                            "PaymentGameCardLblBalanceCreditsTextTextForeColor",
                                            "PaymentGameCardLblBalanceCreditsTextForeColor",
                                            "PaymentGameCardBtnApplyTextForeColor",
                                            "PaymentGameCardBtnCancelTextForeColor",
                                            "PaymentGameCardTxtMessageTextForeColor",
                                            "PaymentGameCardBtnHomeTextForeColor",
                                            "PaymentGameCardTxtMessageErrorBackColor",
                                            "PaymentGameCardLblAvailableFreeEntriesTextTextForeColor",
                                            "PaymentGameCardLblAvailableFreeEntriesTextForeColor",
                                            "PaymentGameCardLblBalanceFreeEntriesTextTextForeColor",
                                            "PaymentGameCardLblBalanceFreeEntriesTextForeColor",

                                            "FrmLoyaltyLblLoyaltyTextTextForeColor",
                                            "FrmLoyaltyBtnLoyaltyYesTextForeColor",
                                            "FrmLoyaltyBtnLoyaltyNoTextForeColor",
                                            "FrmLoyaltyLblPhoneNoTextForeColor",
                                            "FrmLoyaltyTxtPhoneNoTextForeColor",
                                            "FrmLoyaltyBtnGoTextForeColor",
                                            "FrmLoyaltyListboxNamesTextForeColor",
                                            "FrmLoyaltyTxtFirstNameTextForeColor",
                                            "FrmLoyaltyBtnOkTextForeColor",
                                            "FrmLoyaltyBtnProceedWithoutLoyaltyTextForeColor",
                                            "FrmLoyaltyBtnCancelTextForeColor",
                                            "FrmLoyaltyTxtMessageTextForeColor",

                                            "AddCustomerRelationBtnHomeTextForeColor",
                                            "AddCustomerRelationLblGreetingTextForeColor",
                                            "AddCustomerRelationLblCardNumberTextForeColor",
                                            "AddCustomerRelationLblCustomerNameTextForeColor",
                                            "AddCustomerRelationTxtCardNumberTextForeColor",
                                            "AddCustomerRelationTxtCustomerNameTextForeColor",
                                            "AddCustomerRelationLblAddParticipantsTextForeColor",
                                            "AddCustomerRelationBtnBackTextForeColor",
                                            "AddCustomerRelationBtnConfirmTextForeColor",
                                            "AddCustomerRelationTxtboxMessageLineTextForeColor",
                                            "AddCustomerRelationGridTextForeColor",
                                            "AddCustomerRelationLblTimeRemainingTextForeColor",
                                            "AddCustomerRelationLblCardNumberForeColor",
                                            "AddCustomerRelationLblCustomerNameForeColor",
                                            "AddCustomerRelationLblGridFooterTextForeColor",
                                            "AddCustomerRelationGridHeaderTextForeColor",

                                            "frmUsrCtltPnlWaiversTextForeColor",
                                            "frmUsrCtltPnlWaiverSetTextForeColor",

                                            "FundsDonationsBtnProductTextForeColor",
                                            "FundsDonationsBtnTextForeColor",
                                            "FundsDonationsGreetingTextForeColor",
                                            "FundsDonationsLblFundDonationMessageTextForeColor",
                                            "FundsDonationsFooterTextForeColor",

                                            "ComboChildProductsQtyHomeButtonTextForeColor",
                                            "ComboChildProductsQtyGreetingLblTextForeColor",
                                            "ComboChildProductsQtyBackButtonTextForeColor",
                                            "ComboChildProductsQtyCancelButtonTextForeColor",
                                            "ComboChildProductsQtyProceedButtonTextForeColor",
                                            "ComboChildProductsQtyFooterTxtMsgTextForeColor",
                                            "ComboChildProductsQtyProductButtonTextForeColor",
                                            "ComboChildProductsQtyQuantityTextForeColor",
                                            "ComboChildProductsQtyLblAgeCriteriaTextForeColor",


                                            "SelectChildGreetingLblTxtForeColor",
                                            "SelectChildYourfamilyLblTextForeColor",
                                            "SelectChildYourFamilyGridTextForeColor",
                                            "SelectChildMemberDetailsGridTextForeColor",
                                            "SelectChildFooterTxtMsgTextForeColor",
                                            "SelectChildProceedButtonTextForeColor",
                                            "SelectChildSkipButtonTextForeColor",
                                            "SelectChildBackButtonTextForeColor",
                                            "SelectChildHomeButtonTextForeColor",
                                            "SelectChildMemberDetailsLblTextForeColor",
                                            "SelectChildGridHeaderTextForeColor",
                                            "SelectChildYourFamilyGridInfoTextForeColor",
                                            "SelectChildMemberDetailsGridInfoTextForeColor",

                                            "EnterChildDetailsGreetingLblTextForeColor",
                                            "EnterChildDetailsGridTextForeColor",
                                            "EnterChildDetailsFooterTextMsgTextForeColor",
                                            "EnterChildDetailsProceedButtonTextForeColor",
                                            "EnterChildDetailsChildAddedLabelTextForeColor",
                                            "EnterChildDetailsBackButtonTextForeColor",
                                            "EnterChildDetailsHomeButtonTextForeColor",
                                            "EnterChildDetailsTimeRemainingLblTextForeColor",
                                            "EnterChildDetailsGridHeaderTextForeColor",

                                            "ChildSummaryGreetingLblTextForeColor",
                                            "ChildSummaryFooterTxtMsgTextForeColor",
                                            "ChildSummaryProceedButtonTextForeColor",
                                            "ChildSummaryBackButtonTextForeColor",
                                            "ChildSummaryHomeButtonTextForeColor",

                                            "SelectAdultGreetingLblTxtForeColor",
                                            "SelectAdultYourfamilyLblTextForeColor",
                                            "SelectAdultYourFamilyGridTextForeColor",
                                            "SelectAdultMemberDetailsGridTextForeColor",
                                            "SelectAdultFooterTxtMsgTextForeColor",
                                            "SelectAdultProceedButtonTextForeColor",
                                            "SelectAdultSkipButtonTextForeColor",
                                            "SelectAdultBackButtonTextForeColor",
                                            "SelectAdultHomeButtonTextForeColor",
                                            "SelectAdultMemberDetailsLblTextForeColor",
                                            "SelectAdultGridFooterMsgLblTextForeColor",
                                            "SelectAdultNoRelationMsg1LblTextForeColor",
                                            "SelectAdultNoRelationMsg2LblTextForeColor",
                                            "SelectAdultNoRelationMsg3LblTextForeColor",
                                            "SelectAdultGridHeaderTextForeColor",
                                            "SelectAdultYourFamilyGridInfoTextForeColor",
                                            "SelectAdultMemberDetailsGridInfoTextForeColor",

                                            "EnterAdultDetailsGreetingLblTextForeColor",
                                            "EnterAdultDetailsGridTextForeColor",
                                            "EnterAdultDetailsFooterTextMsgTextForeColor",
                                            "EnterAdultDetailsProceedButtonTextForeColor",
                                            "EnterAdultDetailsChildAddedLabelTextForeColor",
                                            "EnterAdultDetailsBackButtonTextForeColor",
                                            "EnterAdultDetailsHomeButtonTextForeColor",
                                            "EnterAdultDetailsTimeRemainingLblTextForeColor",
                                            "EnterAdultDetailsGridHeaderTextForeColor",

                                            "AdultSummaryGreetingLblTextForeColor",
                                            "AdultSummaryFooterTxtMsgTextForeColor",
                                            "AdultSummaryProceedButtonTextForeColor",
                                            "AdultSummaryBackButtonTextForeColor",
                                            "AdultSummaryHomeButtonTextForeColor",

                                            "CheckInSummaryDetailsTextForeColor",
                                            "CheckInSummaryTotalToPayTextForeColor",
                                            "CheckInSummaryHomeButtonTextForeColor",
                                            "CheckInSummaryDiscountLabelTextForeColor",
                                            "CheckInSummaryApplyDiscountBtnTextForeColor",
                                            "CheckInSummaryCPValidityTextForeColor",
                                            "CheckInSummaryBackButtonTextForeColor",
                                            "CheckInSummaryProceedButtonTextForeColor",
                                            "CheckInSummaryFooterMessageTextForeColor",
                                            "CheckInSummaryLblPassportCouponTextForeColor",
                                            "CheckInSummaryPanelUsrCtrlLblTextForeColor",
                                            "CheckInSummaryPanelTextForeColor",

                                            "CalenderMsgTextForeColor",
                                            "CalenderBtnCancelTextForeColor",
                                            "CalenderBtnSaveTextForeColor",
                                            "CalenderItemHeaderTextForeColor",
                                            "CalenderItemTextForeColor",
                                            "CalenderGridTextForeColor",
                                            "CalenderGreetingTextForeColor",

                                            "PackageDetailsLblHeaderTextForeColor",
                                            "PackageDetailsLblTextForeColor",
                                            "GuestCountLblTextForeColor",
                                            "ScreendetailsLblTextForeColor",
                                            "ScreenDetailsLblTextForeColor",
                                            "LblGridFooterMsgTextForeColor",

                                            "PurchaseMenuGreetingTextForeColor",
                                            "PurchaseMenuBtnHomeTextForeColor",
                                            "PurchaseMenuFooterTextForeColor",
                                            "PurchaseMenuLblSiteNameTextForeColor",
                                            "PurchaseMenuBtnTextForeColor",

                                            "FrmGetEmailDetailsTxtCustomerEmailTextForeColor",
                                            "FrmGetEmailDetailsLblGreeting1TextForeColor",
                                            "FrmGetEmailDetailsTxtUserEntryEmailTextForeColor",
                                            "FrmGetEmailDetailsLabel1TextForeColor",
                                            "FrmGetEmailDetailsLabel2TextForeColor",
                                            "FrmGetEmailDetailsLabel3TextForeColor",
                                            "FrmGetEmailDetailsBtnCancelTextForeColor",
                                            "FrmGetEmailDetailsBtnOkTextForeColor",
                                            "FrmGetEmailDetailsFooterTxtMsgTextForeColor",
                                            "FrmGetEmailDetailsLblSkipDetailsTextForeColor",
                                            "FrmGetEmailDetailsLblNoteTextForeColor",

                                            "FrmReceiptDeliveryModeOptionsLblGreeting1TextForeColor",
                                            "FrmReceiptDeliveryModeOptionsBtnPrintTextForeColor",
                                            "FrmReceiptDeliveryModeOptionsBtnEmailTextForeColor",
                                            "FrmReceiptDeliveryModeOptionsBtnNoneTextForeColor",

                                            "UsrCtrlCartLblProductDescriptionTextForeColor",
                                            "UsrCtrlCartLblTotalPriceTextForeColor",
                                            "KioskCartQuantityTextForeColor",

                                            "FrmKioskCartLblGreeting1TextForeColor",
                                            "FrmKioskCartButtonTextForeColor",
                                            "FrmKioskCartDiscountButtonTextForeColor",
                                            "FrmKioskCartLblHeaderTextForeColor",
                                            "FrmKioskCartLblAmountTextForeColor",
                                            "FrmKioskCartFooterTxtMsgTextForeColor",
                                            "FrmKioskCartLblProductHeaderTextForeColor",

                                            "PreSelectPaymentModeBtnTextForeColor",
                                            "PreSelectPaymentModeLblHeadingTextForeColor",
                                            "PreSelectPaymentModeBackButtonTextForeColor",
                                            "PreSelectPaymentModeCancelButtonTextForeColor",
                                            "PreSelectPaymentModeBtnHomeTextForeColor",

                                            "PrintSummarySiteTextForeColor",
                                            "PrintSummaryLblGreetingTextForeColor",
                                            "PrintSummaryLblFromDateTextForeColor",
                                            "PrintSummaryLblFromDateTimeForeColor",
                                            "PrintSummaryLblToDateTextForeColor",
                                            "PrintSummaryLblToDateTimeForeColor",
                                            "PrintSummaryBtnPrevTextForeColor",
                                            "PrintSummaryBtnPrintTextForeColor",
                                            "PrintSummaryBtnEmailTextForeColor",
                                            "PrintSummaryNewTabTextForeColor",
                                            "PrintSummaryRePrintTabTextForeColor",
                                            "PrintSummaryComboTextForeColor",
                                            "PrintSummaryLblTabTextForeColor",

                                            "TextBoxLblMsgTextForeColor",
                                            "TextBoxTxtDataTextForeColor",

                                            "ComboDetailsBtnHomeTextForeColor",
                                            "ComboDetailsProductNameTextForeColor",
                                            "ComboDetailsPackageMsgTextForeColor",
                                            "ComboDetailsBackButtonTextForeColor",
                                            "ComboDetailsCancelButtonTextForeColor",
                                            "ComboDetailsProceedButtonTextForeColor",
                                            "ComboDetailsFooterTextForeColor",

                                            "UsrCtrlComboDetailsProductNameTextForeColor",
                                            "UsrCtrlComboDetailsNumberTextForeColor",

                                            "AttractionQtyHomeButtonTextForeColor",
                                            "AttractionQtyBackButtonTextForeColor",
                                            "AttractionQtyCancelButtonTextForeColor",
                                            "AttractionQtyProceedButtonTextForeColor",
                                            "AttractionQtyFooterTextForeColor",
                                            "AttractionQtyLblYourSelectionsTextForeColor",
                                            "AttractionQtyHeadersTextForeColor",
                                            "AttractionQtyValuesTextForeColor",
                                            "AttractionQtyTxtQtyTextForeColor",
                                            "AttractionQtyLblHowManyMsgTextForeColor",
                                            "AttractionQtyLblEnterQtyMsgTextForeColor",
                                            "AttractionQtyLblBuyTextTextForeColor",
                                            "AttractionQtyLblProductNameTextForeColor",
                                            "AttractionQtyLblComboDetailsTextForeColor",
                                            "AttractionQtyLblProductDescriptionTextForeColor",

                                            "ProcessingAttractionsLblProcessingMsgTextForeColor",
                                            "ProcessingAttractionsFooterTxtMsgTextForeColor",
                                            "ProcessingAttractionsProceedButtonTextForeColor",
                                            "ProcessingAttractionsCancelButtonTextForeColor",
                                            "ProcessingAttractionsBackButtonTextForeColor",
                                            "ProcessingAttractionsHomeButtonTextForeColor",

                                            "CardSaleOptionHeaderTextForeColor",
                                            "CardSaleOptionCancelBtnTextForeColor",
                                            "CardSaleOptionConfirmBtnTextForeColor",
                                            "CardSaleOptionLblNewCardTextForeColor",
                                            "CardSaleOptionLblExistingCardTextForeColor",

                                            "SelectSlotHomeButtonTextForeColor",
                                            "SelectSlotLblBookingSlotTextForeColor",
                                            "SelectSlotLblProductNameTextForeColor",
                                            "SelectSlotBackButtonTextForeColor",
                                            "SelectSlotCancelButtonTextForeColor",
                                            "SelectSlotProceedButtonTextForeColor",
                                            "SelectSlotFooterTextForeColor",
                                            "SelectSlotLblDateTextForeColor",
                                            "SelectSlotLblSelectSlotTextForeColor",
                                            "SelectSlotBtnDatesTextForeColor",
                                            "SelectSlotLblNoSchedulesTextForeColor",

                                            "UsrCtrlSlotLblSlotInfo",
                                            "UsrCtrlSlotLblScheduleTime",

                                            "UsrCtrlAttrcationSummaryLblProductNameTextForeColor",
                                            "UsrCtrlAttrcationSummaryLblSlotDetailsTextForeColor",

                                            "frmAddToCartAlertBtnCloseTextForeColor",
                                            "frmAddToCartAlertLblMsgTextForeColor",
                                            "frmAddToCartAlertBtnCheckOutTextForeColor",

                                            "SearchCustomerSearchBtnTextForeColor",
                                            "SearchCustomerNewRegistrationBtnTextForeColor",
                                            "SearchCustomerPrevBtnTextForeColor",
                                            "SearchCustomerFooterTextForeColor",
                                            "SearchCustomerLblExistingCustomerMsgForeColor",
                                            "SearchCustomerGreetingForeColor",
                                            "SearchCustomerLblNewCustomerHeaderForeColor",
                                            "SearchCustomerLblNewCustomerForeColor",
                                            "SearchCustomerExistingCustomerHeaderForeColor",
                                            "SearchCustomerExistingCustomerForeColor",
                                            "SearchCustomerLblEnterPhoneHeaderForeColor",
                                            "SearchCustomerLblEnterEmailHeaderForeColor",
                                            "SearchCustomerTxtPhoneNumForeColor",
                                            "SearchCustomerTxtEmailIdForeColor",

                                            "UsrCtrlSelectCustomerLblFirstNameTextForeColor",
                                            "UsrCtrlSelectCustomerLblLastNameTextForeColor",

                                            "CustomerFoundBtnOKTextForeColor",
                                            "CustomerFoundBtnPrevTextForeColor",
                                            "CustomerFoundLblCustomerNameTextForeColor",
                                            "CustomerFoundLblWelcomeMsgTextForeColor",
                                            "CustomerFoundLblClickOKMsgTextForeColor",

                                            "SelectCustomerLblGreetingTextForeColor",
                                            "SelectCustomerBtnProceedTextForeColor",
                                            "SelectCustomerBtnCancelTextForeColor",
                                            "SelectCustomerBtnPrevTextForeColor",
                                            "SelectCustomerLblFirstNameTextForeColor",
                                            "SelectCustomerLblLastNameTextForeColor",
                                            "SelectCustomerBackButtonTextForeColor",
                                            "SelectCustomerFooterTextForeColor",

                                            "WaiverSigningAlertLblGreetingTextForeColor",
                                            "WaiverSigningAlertLblMsgTextForeColor",
                                            "WaiverSigningAlertBackButtonTextForeColor",
                                            "WaiverSigningAlertCancelButtonTextForeColor",
                                            "WaiverSigningAlertSignButtonTextForeColor",
                                            "WaiverSigningAlertFooterTextForeColor",

                                            "UsrCtrlWaiverSigningAlertLblProductNameTextForeColor",
                                            "UsrCtrlWaiverSigningAlertLblParticipantsTextForeColor",

                                            "MapAttendeesLblGreetingTextForeColor",
                                            "MapAttendeesLblProductNameTextForeColor",
                                            "MapAttendeesLblMsgTextForeColor",
                                            "MapAttendeestBackButtonTextForeColor",
                                            "MapAttendeesCancelButtonTextForeColor",
                                            "MapAttendeesAssignButtonTextForeColor",
                                            "MapAttendeesFooterTextForeColor",
                                            "MapAttendeesLblQuantityTextForeColor",
                                            "MapAttendeesLblAssignParticipantsTextForeColor",
                                            "MapAttendeesMappedCustomerNameTextForeColor",

                                            "UsrCtrlMapAttendeeToProductBtnProductTextForeColor",
                                            "UsrCtrlMapAttendeeToProductBtnProductHighlightTextForeColor",

                                            "UsrCtrlMapAttendeesToQuantityLblQtyTextForeColor",
                                            "UsrCtrlMapAttendeesToQuantityLblQtySelectedTextForeColor",
                                            "UsrCtrlMapAttendeesToQuantityBtnProductQtySelectedTextForeColor",

                                            "CustomerRelationsGreetingTextForeColor",
                                            "CustomerRelationsBackButtonTextForeColor",
                                            "CustomerRelationsAddNewMemberButtonTextForeColor",
                                            "CustomerRelationsSearchAnotherCustomerButtonTextForeColor",
                                            "CustomerRelationsProceedButtonTextForeColor",
                                            "CustomerRelationsFooterTextForeColor",

                                            "UsrCtrlCustomerNameTextForeColor",
                                            "CustomerRelationsRelatedCustomerNameTextForeColor",
                                            "CustomerRelationsRelationshipTextForeColor",
                                            "CustomerRelationsSignStatusTextForeColor",
                                            "CustomerRelationsValidityTextForeColor",

                                            "FilteredCustomersGreetingTextForeColor",
                                            "FilteredCustomersCloseButtonTextForeColor",
                                            "FilteredCustomersLinkButtonTextForeColor",
                                            "UsrCtrlFilteredCustomersCustomerNameTextForeColor"
    };


        public class ThemeClass
        {
            public int ThemeId;
            public Color SiteHeadingForeColor = Color.White;
            public Color ScreenHeadingForeColor = Color.Black;
            public Color FieldLabelForeColor;
            public Color ApplicationVersionForeColor;
            public Color FooterMsgErrorHighlightBackColor;
            public Color ThemeForeColor = Color.White;//Starts:Modification on 17-Dec-2015 for introducing new theme
            public Color TextForeColor = Color.White;//Ends:Modification on 17-Dec-2015 for introducing new theme
            public Color TextBackGroundColor = Color.White;
            public bool ShowSiteHeading;
            public bool ShowHeaderMessage = true;
            public string HomeBigButtonTextAlignment = MIDDLELEFT;
            public string HomeMediumButtonTextAlignment = BOTTOMCENTER;
            public string HomeSmallButtonTextAlignment = BOTTOMCENTER;
            public string DirectCashButtonTextAlignment = MIDDLELEFT;
            public string DisplayGroupButtonTextAlignment = MIDDLELEFT;
            public string DisplayProductButtonTextAlignment = MIDDLELEFT;
            public string PaymentModeButtonTextAlignment = TOPCENTER;
            public string PreSelectPaymentModeButtonTextAlignment = TOPCENTER;
            public string RedeemTokenButtonTextAlignment = MIDDLELEFT;
            public string CustOptionBtnTextAlignment = MIDDLELEFT;
            public string FSKSalesTextAlignment = MIDDLELEFT;
            public string BasicCardCountButtonTextAlignment = MIDDLELEFT;
            public string DisplayProductsGreetingOption = OPT1;
            public int KeypadSizePercentage;
            public Font DefaultFont;
            public int HomeScreenOptionWidth = 0;
            public float HomeScreenOptionsLargeFontSize = 45F;
            public float HomeScreenOptionsSmallFontSize = 35.25F;
            public float HomeScreenOptionsMediumFontSize = 26F;
            public int KioskCalendarWidth = 900;
            public int KioskCalendarHeight = 900;
            //Playpass:Starts 
            //All below 13 variables are set from Congiguration.txt file 
            public Font HomePageMainButtonFont = null;//To adjust the Home page buttons(buy new card,check balance.. etc) font following line is added.
            public Font HomePageOtherButtonFont = null;//To adjust the Home page buttons(FAQ & English) font following line is added.
            public Font HomePageBottomTextFont = null;//To adjust the Home page bottom message text(Choose your option label) and other page bottom text font  following line is added.
            public Font ProductButtonFont = null;//To adjust the Topup product buttons font  following line is added.
            public Font FirstLableFont = null;//To adjust the First order labels (labels which displays welcome message on the top) font  following line is added.
            public Font SecondLableFont = null;//To adjust the Second order labels (labels which displays below the first order messages and smaller in size than the first order on the top) font  following line is added.
            public Font ThirdLableFont = null;//To adjust the third order labels (labels which displays  smaller in size than the second order) font  following line is added.
            public Font FourthLableFont = null;//To adjust the fourth order labels (labels which displays  smaller in size than the third order) font  following line is added.
            public Font CreditCardButtonFont = null;//To adjust credit card button  font in transaction page following line is added.
            public Font ActivityButtonFont = null;//To adjust activity and other two buttons  font in check balance page following line is added.
            public Font ButtonFont = null;//To adjust the font of save, cancel, next buttons following line is added.
            public Font TextBoxFont = null;//To adjust the grid data font following line is added.
            public Font GridDataFont = null;//To adjust all the text box size following line is added.
            public Font DisplayGroupButtonFont = null;//To adjust all the text box size following line is added.
            public int SplashAfterSeconds = 0;//this variable holds the time in seconds, to load the splash screen

            //Customized Fore color 01Jul2021
            //HomeSceen
            public Color HomeScreenOptionsTextForeColor;
            public Color HomeScreenBtnLanguageTextForeColor;
            public Color HomeScreenLanguageListForeColor;
            public Color HomeScreenBtnTermsTextForeColor;
            public Color HomeScreenFooterTextForeColor;
            public Color HomeScreenBtnRegisterTextForeColor;
            public Color HomeScreenBtnCheckBalanceTextForeColor;
            public Color HomeScreenBtnTransferTextForeColor;
            public Color HomeScreenBtnRedeemTokensTextForeColor;
            public Color HomeScreenBtnPauseTimeTextForeColor;
            public Color HomeScreenBtnPointsToTimeTextForeColor;
            public Color HomeScreenBtnHomeTextForeColor;
            public Color HomeScreenBtnSignWaiverTextForeColor;
            public Color HomeScreenBtnPurchaseTextForeColor;

            //frmChooseDisplayGroup -Dsiplay group selection screen
            public Color DisplayGroupBtnTextForeColor;
            public Color DisplayGroupCancelBtnTextForeColor;
            public Color DisplayGroupBackBtnTextForeColor;
            public Color DisplayGroupFooterTextForeColor;
            public Color DisplayGroupGreetingsTextForeColor;

            //frmChooseProducts -Prooducts selection screen
            public Color ChooseProductsBtnTextForeColor;
            public Color ChooseProductsCancelBtnTextForeColor;
            public Color ChooseProductVariableBtnTextForeColor;
            public Color ChooseProductsBackBtnTextForeColor;
            public Color ChooseProductsVariableBtnTextForeColor;
            public Color ChooseProductsFooterTextForeColor;
            public Color ChooseProductsGreetingsTextForeColor;
            public Color ChooseProductsBtnHomeTextForeColor;

            //frmTapCard – Card Tap Screen
            public Color TapCardScreenHeaderTextForeColor;
            public Color TapCardScreenNewCardTextForeColor;
            public Color TapCardScreenYesBtnTextForeColor;
            public Color TapCardScreenCloseBtnTextForeColor;
            public Color TapCardScreenNoBtnTextForeColor;
            public Color TapCardScreenButton1TextForeColor;
            public Color TapCardScreenNoteTextForeColor;
            public Color TapCardScreenLblMessageTextForeColor;

            //frmPaymentMode – PaymentOPtion selection screen
            public Color PaymentModeBtnTextForeColor;
            public Color PaymentModeSummaryHeaderTextForeColor;
            public Color PaymentModeDepositeHeaderTextForeColor;
            public Color PaymentModeDiscountHeaderTextForeColor;
            public Color PaymentModeSummaryInfoTextForeColor;
            public Color PaymentModeDepositeInfoTextForeColor;
            public Color PaymentModeDiscountInfoTextForeColor;
            public Color PaymentModeTotalToPayHeaderTextForeColor;
            public Color PaymentModeTotalToPayInfoTextForeColor;
            public Color PaymentModeDiscountBtnTextForeColor;
            public Color PaymentModeCPValidityTextForeColor;
            public Color PaymentModeBackButtonTextForeColor;
            public Color PaymentModeCancelButtonTextForeColor;
            public Color PaymentModeButtonsTextForeColor;
            public Color PaymentModeLblHeadingTextForeColor;
            public Color PaymentModeLblCardnumberTextForeColor;
            public Color PaymentModeLblPackageTextForeColor;
            public Color PaymentModeDiscountLabelTextForeColor;
            public Color PaymentModeLblPriceOfCardDepositTextForeColor;
            public Color PaymentModeTextMessageTextForeColor;
            public Color PaymentModeBtnHomeTextForeColor;
            public Color PaymentModeLblPassportCouponTextForeColor;

            //frmYesyNo – Yes / No form
            public Color YesNoScreenMessageTextForeColor;
            public Color YesNoScreenBtnYesTextForeColor;
            public Color YesNoScreenBtnNoTextForeColor;
            public Color YesNoScreenLblAdditionalMessageTextForeColor;

            //frmTransferForm.cs
            public Color TransferFormTapMessageTextForeColor;
            public Color TransferFormFooterTextForeColor;
            public Color TransferFormBtnPrevTextForeColor;
            public Color TransferFormBtnNextTextForeColor;
            public Color TransferFormCardDetailsHeaderTextForeColor;
            public Color TransferFormCardDetailsInfoTextForeColor;
            public Color TransferFormTimeRemainingTextForeColor;

            //frmFAQ.cs
            public Color FAQScreenHeaderTextForeColor;
            public Color FAQScreenBtnTermsTextForeColor;
            public Color FAQScreenBtnBackTextForeColor;

            //frmPauseTime(Pause time screen)
            public Color PauseTimeLblMessageTextForeColor;
            public Color PauseTimeCardNumberHeaderTextForeColor;
            public Color PauseTimeCardNumberInfoTextForeColor;
            public Color PauseTimeTimeHeaderTextForeColor;
            public Color PauseTimeTimeInfoTextForeColor;
            public Color PauseTimeETicketHeaderTextForeColor;
            public Color PauseTimeETicketInfoTextForeColor;
            public Color PauseTimeBackBtnTextForeColor;
            public Color PauseTimeOkBtnTextForeColor;

            //frmCardCount.cs
            public Color CardCountScreenHeader1TextForeColor;
            public Color CardCountScreenHeader2TextForeColor;
            public Color CardCountScreenCardBtnsTextForeColor;
            public Color CardCountScreenBackBtnTextForeColor;
            public Color CardCountScreenCancelBtnTextForeColor;

            //frmAdmin.cs
            public Color AdminScreenBtnCancelTextForeColor;
            public Color AdminScreenBtnExitTextForeColor;
            public Color AdminScreenBtnSetupTextForeColor;
            public Color AdminScreenBtnLoadBonusTextForeColor;
            public Color AdminScreenBtnPrintSummaryTextForeColor;
            public Color AdminScreenBtnKioskActivityTextForeColor;
            public Color AdminScreenBtnRebootComputerTextForeColor;
            public Color AdminScreenBtnTrxViewTextForeColor;

            //frmCashInsert(DirectCash)
            public Color CashInsertScreenHeader1TextForeColor;
            public Color CashInsertScreenHeader2TextForeColor;
            public Color CashInsertScreenCashInsertedInfoTextForeColor;
            public Color CashInsertScreenBtnNewCardTextForeColor;
            public Color CashInsertScreenBtnRechargeTextForeColor;
            public Color CashInsertScreenHeader3TextForeColor;
            public Color CashInsertScreenDenominationHeaderTextForeColor;
            public Color CashInsertScreenQuantityHeaderTextForeColor;
            public Color CashInsertScreenPointsHeaderTextForeColor;
            public Color CashInsertScreenCancelButtonTextForeColor;

            //frmAgeGate.cs
            public Color AgeGateScreenHeader1TextForeColor;
            public Color AgeGateScreenHeader2TextForeColor;
            public Color AgeGateScreenHeader3TextForeColor;
            public Color AgeGateScreenMonthTextBoxTextForeColor;
            public Color AgeGateScreenDateTextBoxTextForeColor;
            public Color AgeGateScreenYearTextBoxTextForeColor;
            public Color AgeGateScreenDateFormatTextForeColor;
            public Color AgeGateScreenReadConfirmTextForeColor;
            public Color AgeGateScreenlnkTermsTextForeColor;
            public Color AgeGateScreenBtnCancelTextForeColor;
            public Color AgeGateScreenBtnNextTextForeColor;

            //frmCustomer.cs
            public Color CustomerScreenHeader1TextForeColor;
            public Color CustomerScreenHeader2TextForeColor;
            public Color CustomerScreenDetailsHeaderTextForeColor;
            public Color CustomerScreenInfoTextForeColor;
            public Color CustomerScreenOptInTextForeColor;
            public Color CustomerScreenTermsAndConditionsTextForeColor;
            public Color CustomerScreenPrivacyTextForeColor;
            public Color CustomerScreenInkTermsTextForeColor = Color.Blue;
            public Color CustomerScreenBtnPrevTextForeColor;
            public Color CustomerScreenBtnSaveTextForeColor;
            public Color CustomerScreenFooterTextForeColor;
            public Color CustomerScreenLblTimeRemainingTextForeColor;
            public Color CustomerScreenBtnHomeTextForeColor;
            public Color CustomerScreenDgvLinkedRelationsTextForeColor;
            public Color CustomerScreenLblLinkedRelationsTextForeColor;
            public Color CustomerScreenBtnAddRelationTextForeColor;
            public Color CustomerScreenDgvLinkedRelationsHeaderTextForeColor;
            public Color CustomerScreenWhatsAppOptOutTextForeColor;
            public Color CustomerScreenPhotoTextForeColor;
            public Color CustomerScreenDgvLinkedRelationsInfoTextForeColor;

            //CheckBalanceScreen
            public Color CheckBalanceHeader1TextForeColor;
            public Color CheckBalanceCardNumerHeaderTextForeColor;
            public Color CheckBalanceCardNumberInfoTextForeColor;
            public Color CheckBalanceTicketModeHeaderTextForeColor;
            public Color CheckBalanceTicketModeInfoTextForeColor;
            public Color CheckBalanceBtnChangeTextForeColor;
            public Color CheckBalancePlayValueHeaderTextForeColor;
            public Color CheckBalancePlayValueInfoTextForeColor;
            public Color CheckBalanceBonusHeaderTextForeColor;
            public Color CheckBalanceBonusInfoTextForeColor;
            public Color CheckBalanceVirtualPointInfoTextForeColor;
            public Color CheckBalanceVirtualPointHeaderTextForeColor;
            public Color CheckBalanceCourtesyHeaderTextForeColor;
            public Color CheckBalanceCourtesyInfoTextForeColor;
            public Color CheckBalanceTimeHeaderTextForeColor;
            public Color CheckBalanceTimeInfoTextForeColor;
            public Color CheckBalanceLPHeaderTextForeColor;
            public Color CheckBalanceLPInfoTextForeColor;
            public Color CheckBalanceGameHeaderTextForeColor;
            public Color CheckBalanceGameInfoTextForeColor;
            public Color CheckBalanceETicketHeaderTextForeColor;
            public Color CheckBalanceETicketInfoTextForeColor;
            public Color CheckBalanceBackButtonTextForeColor;
            public Color CheckBalanceTopUpButtonTextForeColor;
            public Color CheckBalanceActivityButtonTextForeColor;
            public Color CheckBalanceFooterTextForeColor;
            public Color CheckBalanceTimeOutTextForeColor;
            public Color CheckBalanceBtnViewSignedWaiversTextForeColor;
            public Color CheckBalanceBtnHomeTextForeColor;

            //frmCustomerDashboard
            public Color CustomerDashboardCardNumberTextForeColor;
            public Color CustomerDashboardCardNumberInfoTextForeColor;
            public Color CustomerDashboardTicketModeTextForeColor;
            public Color CustomerDashboardTicketModeInfoTextForeColor;
            public Color CustomerDashboardChangeButtonTextForeColor;
            public Color CustomerDashboardHeader1TextForeColor;
            public Color CustomerDashboardHeader1DetailsTextForeColor;
            public Color CustomerDashboardHeader2TextForeColor;
            public Color CustomerDashboardHeader2DetailsTextForeColor;
            public Color CustomerDashboardBackBtnTextForeColor;
            public Color CustomerDashboardTopUpBtnTextForeColor;
            public Color CustomerDashboardRegisterBtnTextForeColor;
            public Color CustomerDashboardButton1TextForeColor;
            public Color CustomerDashboardLblPlayValueLabelTextForeColor;
            public Color CustomerDashboardLblCreditsTextForeColor;
            public Color CustomerDashboardLblTimeOutTextForeColor;
            public Color CustomerDashboardBtnHomeTextForeColor;
            public Color CustomerDashboardInfoTextForeColor;

            //frmCreditsToTime
            public Color CreditsToTimeHeaderTextForeColor;
            public Color CreditsToTimeDetailsHeaderTextForeColor;
            public Color CreditsToTimeDetailsInfoTextForeColor;
            public Color CreditsToTimeBtnPrevTextForeColor;
            public Color CreditsToTimeBtnSaveTextForeColor;
            public Color CreditsToTimeAfterSaveHeaderTextForeColor;
            public Color CreditsToTimeAfterSaveInfoTextForeColor;
            public Color CreditsToTimeFooterTextForeColor;
            public Color CreditsToTimeTimeRemainingTextForeColor;
            public Color CreditsToTimeBtnHomeTextForeColor;

            //frmRedeemTokens
            public Color RedeemTokensScreenHeaderTextForeColor;
            public Color RedeemTokensScreenTokenInsertedTextForeColor;
            public Color RedeemTokensScreenAvialableTokensTextForeColor;
            public Color RedeemTokensScreenBtnNewCardTextForeColor;
            public Color RedeemTokensScreenBtnLoadTextForeColor;
            public Color RedeemTokensScreenBtnBackTextForeColor;
            public Color RedeemTokensScreenFooterTextForeColor;
            public Color RedeemTokensScreenDenominationTextForeColor;
            public Color RedeemTokensScreenQuantityTextForeColor;
            public Color RedeemTokensScreenPointsTextForeColor;
            public Color RedeemTokensScreenTimeOutTextForeColor;
            public Color RedeemTokensScreenButton1extForeColor;
            public Color RedeemTokensScreenBtnHomeTextForeColor;

            //frmTransferTo
            public Color TransferToScreenHeaderTextForeColor;
            public Color TransferToScreenTimeRemainingTextForeColor;
            public Color TransferToScreenCardsHeaderTextForeColor;
            public Color TransferToScreenCreditsHeaderTextForeColor;
            public Color TransferToScreenPointsHeaderTextForeColor;
            public Color TransferToScreenCardsInfoTextForeColor;
            public Color TransferToScreenAvlTokensInfoTextForeColor;
            public Color TransferToScreenTransTokensInfoTextForeColor;
            public Color TransferToScreenBtnPrevTextForeColor;
            public Color TransferToScreenBtnNextTextForeColor;
            public Color TransferToScreenTransfererHeaderTextForeColor;
            public Color TransferToScreenTransfereeHeaderTextForeColor;
            public Color TransferToScreenCardsHeader2TextForeColor;
            public Color TransferToScreenCreditsHeader2TextForeColor;
            public Color TransferToScreenCardsInfo2TextForeColor;
            public Color TransferToScreenCreditsInfo2TextForeColor;
            public Color TransferToScreenFooterTextForeColor;
            public Color CardGamesPrevTextForeColor;
            public Color CardGamesdgvCardGamesForeColor;
            public Color CardGamesdgvCardGamesHeaderForeColor;
            public Color CardGamesBackColor = Color.Purple;

            //frmRegisterTnC
            public Color RegisterTnCHeaderTextForeColor;
            public Color RegisterTnCBtnYesTextForeColor;
            public Color RegisterTnCBtnCancelTextForeColor;
            public Color RegisterTnCBtnPrevTextForeColor;

            //frmTicketType
            public Color TicketTypeHeaderTextForeColor;
            public Color TicketTypeOption1TextForeColor;
            public Color TicketTypeOption2TextForeColor;
            public Color TicketTypeBtnOkTextForeColor;
            public Color TicketTypeBtnCancelTextForeColor;

            // CardCountBasic
            public Color CardCountBasicsHeader1TextForeColor;
            public Color CardCountBasicsHeader2TextForeColor;
            public Color CardCountBasicsHeader3TextForeColor;
            public Color CardCountBasicsBtnOneTextForeColor;
            public Color CardCountBasicsBtnTwoTextForeColor;
            public Color CardCountBasicsBtnThreeTextForeColor;
            public Color CardCountBasicsBtnPrevTextForeColor;

            //KioskActivity
            public Color KioskActivityHeaderTextForeColor;
            public Color KioskActivityDetailsHeaderTextForeColor;
            public Color KioskActivityBtnPrevTextForeColor;
            public Color KioskActivityDgvKioskActivityTextForeColor;
            public Color KioskActivityTxtMessageTextForeColor;
            public Color KioskActivityDgvKioskActivityHeaderTextForeColor;
            public Color KioskActivityDgvKioskActivityInfoTextForeColor;

            //frmCardTransaction
            public Color CardTransactionTimeOutTextForeColor;
            public Color CardTransactionSummaryHeaderTextForeColor;
            public Color CardTransactionSummaryInfoTextForeColor;
            public Color CardTransactionDepositeHeaderTextForeColor;
            public Color CardTransactionDepositeInfoTextForeColor = Color.Black;
            public Color CardTransactionDiscountHeaderTextForeColor;
            public Color CardTransactionDiscountInfoTextForeColor = Color.Black;
            public Color CardTransactionFundHeaderTextForeColor = Color.White;
            public Color CardTransactionFundInfoTextForeColor = Color.Black;
            public Color CardTransactionDonationHeaderTextForeColor = Color.White;
            public Color CardTransactionDonationInfoTextForeColor = Color.Black;
            public Color CardTransactionTotalToPayHeaderTextForeColor;
            public Color CardTransactionTotalToPayInfoTextForeColor;
            public Color CardTransactionAmountPaidHeaderTextForeColor;
            public Color CardTransactionAmountPaidInfoTextForeColor;
            public Color CardTransactionBalanceToPayHeaderTextForeColor;
            public Color CardTransactionBalanceToPayInfoTextForeColor;
            public Color CardTransactionCPVlaiditTextForeColor;
            public Color CardTransactionCancelButtonTextForeColor;
            public Color CardTransactionBtnCreditTextForeColor;
            public Color CardTransactionBtnDebitTextForeColor;
            public Color CardTransactionFooterTextForeColor;
            public Color CardTransactionBtnHomeTextForeColor;
            public Color CardTransactionPaymentButtonsTextForeColor;
            public Color CardTransactionLblTimeOutTextForeColor;

            //frmLoadBonus
            public Color LoadBonusHeaderTextForeColor;
            public Color LoadBonusCardHeaderTextForeColor;
            public Color LoadBonusCreditsHeaderTextForeColor;
            public Color LoadBonusBonusHeaderTextForeColor;
            public Color LoadBonusCardInfoTextForeColor;
            public Color LoadBonusCreditsInfoTextForeColor;
            public Color LoadBonusBonusInfoTextForeColor;
            public Color LoadBonusFooterTextForeColor;
            public Color LoadBonusBtnPrevTextForeColor;
            public Color LoadBonusBtnSaveTextForeColor;

            //frmLoyalty
            public Color LoyaltyHeaderTextForeColor;
            public Color LoyaltyBtnYesTextForeColor;
            public Color LoyaltyBtnNoTextForeColor;
            public Color LoyaltyPhoneNoTextForeColor;
            public Color LoyaltyPhoneNoInfoTextForeColor;
            public Color LoyaltyBtnGoTextForeCOlor;
            public Color LoyaltyFirstNameHeaderTextForeColor;
            public Color LoyaltyNameListTextForeColor;
            public Color LoyaltyFirstNameTextForeColor;
            public Color LoyaltyBtnOkTextForeColor;
            public Color LoyaltyBtnCancelTextForeColor;
            public Color LoyaltyBtnProceedTextForeColor;
            public Color LoyaltyFooterTextForeColor;

            //frmThankYou
            public Color ThankYouScreenHeader1TextForeColor;
            public Color ThankYouScreenHeader2TextForeColor;
            public Color ThankYouScreenBtnPrevTextForeColor;

            //frmUpsellProduct
            public Color UpsellProductScreenTimeOutTextForeColor;
            public Color UpsellProductScreenHeader1TextForeColor;
            public Color UpsellProductScreenHeader2TextForeColor;
            public Color UpsellProductScreenDesc1TextForeColor;
            public Color UpsellProductScreenDesc2TextForeColor;
            public Color UpsellProductScreenDesc3TextForeColor;
            public Color UpsellProductScreenBtnYesTextForeColor;
            public Color UpsellProductScreenBtnNoTextForeColor;
            public Color UpsellProductScreenFooterTextForeColor;
            public Color UpsellProductScreenBtnHomeTextForeColor;

            //frmChooseEntitlement
            public Color ChooseEntitlementHeaderTextForeColor;
            public Color ChooseEntitlementBtnTimeTextForeColor;
            public Color ChooseEntitlemenBtnPointsTextForeColor;
            public Color ChooseEntitlementBtnOkTextForeColor;

            //frmOkMsg
            public Color OkMsgScreenHeaderTextForeColor;
            public Color OkMsgScreenBtnCloseTextForeColor;

            //frmScanCoupon
            public Color ScanCouponScreenHeaderTextForeColor;
            public Color ScanCouponScreenCouponHeaderTextForeColor;
            public Color ScanCouponScreenCouponInfoTextForeColor;
            public Color ScanCouponScreenBtnApplyTextForeColor;
            public Color ScanCouponScreenBtnCloseTextForeColor;

            //frmTimeOutCounter
            public Color TimeOutCounterHeader1TextForeColor;
            public Color TimeOutCounterTimerTextForeColor;
            public Color TimeOutCounterHeader2TextForeColor;

            //Fireball version CEC & Portrait Kiosk
            //frmKioskTransactionView-
            public Color FrmKioskTransViewLblGreetingTextForeColor;
            public Color FrmKioskTransViewLblFromDateTextForeColor;
            public Color FrmKioskTransViewTxtFromTimeHrsTextForeColor;
            public Color FrmKioskTransViewTxtFromTimeMinsTextForeColor;
            public Color FrmKioskTransViewCmbFromTimeTTTextForeColor;
            public Color FrmKioskTransViewLabel1TextForeColor;
            public Color FrmKioskTransViewTxtToTimeHrsTextForeColor;
            public Color FrmKioskTransViewLblPosMachinesTextForeColor;
            public Color FrmKioskTransViewTxtToTimeMinsTextForeColor;
            public Color FrmKioskTransViewCmbToTimeTTTextForeColor;
            public Color FrmKioskTransViewCmbPosMachinesTextForeColor;
            public Color FrmKioskTransViewLblTrxIdTextForeColor;
            public Color FrmKioskTransViewTxtTrxIdTextForeColor;
            public Color FrmKioskTransViewBtnSearchTextForeColor;
            public Color FrmKioskTransViewBtnClearTextForeColor;
            public Color FrmKioskTransViewLblTransactionTextForeColor;
            public Color FrmKioskTransViewDgvKioskTransactionsTextForeColor;
            public Color FrmKioskTransViewTextBtnPrevForeColor;
            public Color FrmKioskTransViewTextBtnPrintReceiptForeColor;
            public Color FrmKioskTransViewTextBtnRefundForeColor;
            public Color FrmKioskTransViewTextBtnPrintPendingForeColor;
            public Color FrmKioskTransViewTextBtnIssueTempCardForeColor;
            public Color FrmKioskTransViewTextTxtMessageForeColor;
            public Color FrmKioskTransactionViewSiteTextForeColor;
            public Color FrmKioskTransViewDgvColumnHeadersTextForeColor;

            public Color FrmPrintTransLblGreetingTextForeColor;
            public Color FrmPrintTransLblCardsPendingPrintTextForeColor;
            public Color FrmPrintTransLblPrintedCardsTextForeColor;
            public Color FrmPrintTransLblTransactionDetailsTextForeColor;
            public Color FrmPrintTransTxtMessageTextForeColor;
            public Color FrmPrintTransDgvTransactionHeaderTextForeColor;
            public Color FrmPrintTransDgvPendingPrintTransactionLinesTextForeColor;
            public Color FrmPrintTransDgvPrintedTransactionLinesTextForeColor;
            public Color FrmPrintTransLblPrintReasonTextForeColor;
            public Color FrmPrintTransCmbPrintReasonTextForeColor;
            public Color FrmPrintTransBtnCancelTextForeColor;
            public Color FrmPrintTransBtnPrintPendingTextForeColor;
            public Color FrmPrintTransLblSiteNameTextForeColor;
            public Color FrmPrintTransDgvTransactionHeaderControlsTextForeColor;
            public Color FrmPrintTransDgvPrintedTransactionLinesControlsTextForeColor;
            public Color FrmPrintTransDgvPendingPrintTransactionLinesControlsTextForeColor;
            public Color FrmPrintTransDgvTransactionHeaderHeaderTextForeColor;
            public Color FrmPrintTransDgvPendingPrintTransactionLinesHeaderTextForeColor;
            public Color FrmPrintTransDgvPrintedTransactionLinesHeaderTextForeColor;

            //frmCashTransaction
            public Color FrmCashTransactionBtnHomeTextForeColor;
            public Color FrmCashTransactionButton1TextForeColor;
            public Color FrmCashTransactionLblTimeOutTextForeColor;
            public Color FrmCashTransactionLabel6TextForeColor;
            public Color FrmCashTransactionLabel1TextForeColor;
            public Color FrmCashTransactionLblTotalToPayTextForeColor;
            public Color FrmCashTransactionLabel8TextForeColor;
            public Color FrmCashTransactionLabel9TextForeColor;
            public Color FrmCashTransactionLblPaidTextForeColor;
            public Color FrmCashTransactionLblBalTextForeColor;
            public Color FrmCashTransactionLblProductCPValidityMsgTextForeColor;
            public Color FrmCashTransactionLblNoChangeTextForeColor;
            public Color FrmCashTransactionBtnCreditCardTextForeColor;
            public Color FrmCashTransactionBtnDebitCardTextForeColor;
            public Color FrmCashTransactionLblSiteNameTextForeColor;
            public Color FrmCashTransactionTxtMessageTextForeColor;

            //FskExecuteOnlineTransaction
            public Color FskExecutePnlPurchaseDetailsHeadersTextForeColor;
            public Color FskExecuteInfoTextForeColor;
            public Color FskExecutePnlPurchaseHeaderTextForeColor;
            public Color FskExecuteOnlineLblSiteNameTextForeColor;
            public Color FskExecuteOnlineLblTransactionOTPTextForeColor;
            public Color FskExecuteOnlineTxtTransactionOTPTextForeColor;
            public Color FskExecuteOnlineLblTransactionIdTextForeColor;
            public Color FskExecuteOnlinePnlTrasactionIdTextForeColor;
            public Color FskExecuteOnlineBtnGetTransactionDetailsTextForeColor;
            public Color FskExecuteOnlineBtnCancelTextForeColor;
            public Color FskExecuteOnlineTxtMessageTextForeColor;
            public Color FskExecuteOnlineTransactionHeaderInfoTextForeColor;
            public Color FskExecuteOnlineTransactionLinesInfoTextForeColor;

            //FrmCusomerDetailsForWaivers
            public Color FrmCustWaiverHeadersTextForeColor;
            public Color FrmCustWaiverDgvCustomerTextForeColor;
            public Color FrmCustWaiverBtnHomeTextForeColor;
            public Color FrmCustWaiverLblSignatoryCustomerNameTextForeColor;
            public Color FrmCustWaiverLblWaiverSetTextForeColor;
            public Color FrmCustWaiverLabel2TextForeColor;
            public Color FrmCustWaiverBtnCancelTextForeColor;
            public Color FrmCustWaiverBtnAddNewRelationsTextForeColor;
            public Color FrmCustWaiverBtnProceedTextForeColor;
            public Color FrmCustWaiverTxtMessageTextForeColor;

            //usrCtlWaiverSet
            public Color frmUsrCtltPnlWaiversTextForeColor;
            public Color frmUsrCtltPnlWaiverSetTextForeColor;

            //frmCustomerOptions
            public Color FrmCustOptionBtnHomeTextForeColor;
            public Color FrmCustOptionBtnNewRegistrationTextForeColor;
            public Color FrmCustOptionBtnExistingCustomerTextForeColor;
            public Color FrmCustOptionBtnCancelTextForeColor;
            public Color FrmCustOptionTxtMessageTextForeColor;

            //frmCustomerOTP
            public Color FrmCustOTPLblOTPTextForeColor;
            public Color FrmCustOTPLblOTPmsgTextForeColor;
            public Color FrmCustOTPLblTimeRemainingTextForeColor;
            public Color FrmCustOTPTxtOTPTextForeColor;
            public Color FrmCustOTPLinkLblResendOTPTextForeColor;
            public Color FrmCustOTPBtnOkayTextForeColor;
            public Color FrmCustOTPBtnCancelTextForeColor;
            public Color FrmCustOTPTxtMessageTextForeColor;

            //frmCustomerSignatureConfirmation
            public Color FrmCustSignConfirmationDgvCustomerSignedWaiverTextForeColor;
            public Color FrmCustSignConfirmationLblCustomerNameTextForeColor;
            public Color FrmCustSignConfirmationChkSignConfirmTextForeColor;
            public Color FrmCustSignConfirmationPbCheckBoxTextForeColor;
            public Color FrmCustSignConfirmationBtnOkayTextForeColor;
            public Color FrmCustSignConfirmationBtnCancelTextForeColor;
            public Color FrmCustSignConfirmationDgvCustomerSignedWaiverHeaderTextForeColor;

            //frmGetCustomerInput
            public Color FrmGetCustInputLabel1TextForeColor;
            public Color FrmGetCustInputLabel2TextForeColor;
            public Color FrmGetCustInputLabel3TextForeColor;
            public Color FrmGetCustInputLblmsgTextForeColor;
            public Color FrmGetCustInputLmlEmailTextForeColor;
            public Color FrmGetCustInputTxtEmailTextForeColor;
            public Color FrmGetCustInputBtnOkTextForeColor;
            public Color FrmGetCustInputBtnCancelTextForeColor;
            public Color FrmGetCustInputTxtMessageTextForeColor;

            //frmLinkRelatedCustomer
            public Color FrmLinkCustLblCustomerTextForeColor;
            public Color FrmLinkCustLblRelatedCustomerTextForeColor;
            public Color FrmLinkCustLblCustomerValueTextForeColor;
            public Color FrmLinkCustLblRelatedCustomerValueTextForeColor;
            public Color FrmLinkCustLabel2TextForeColor;
            public Color FrmLinkCustCmbRelationTextForeColor;
            public Color FrmLinkCustBtnSaveTextForeColor;
            public Color FrmLinkCustBtnCancelTextForeColor;
            public Color FrmLinkCustTxtMessageTextForeColor;

            //frmSelectWaiverOPtions
            public Color FrmSelectWaiverBtnHomeTextForeColor;
            public Color FrmSelectWaiverLabel1TextForeColor;
            public Color FrmSelectWaiverLblCustomerTextForeColor;
            public Color FrmSelectWaiverLblReservationCodeTextForeColor;
            public Color FrmSelectWaiverTxtReservationCodeTextForeColor;
            public Color FrmSelectWaiverLblReservationCodeORTextForeColor;
            public Color FrmSelectWaiverLblTrxOTPTextForeColor;
            public Color FrmSelectWaiverTxtTrxOTPTextForeColor;
            public Color FrmSelectWaiverLabel4TextForeColor;
            public Color FrmSelectWaiverLabel7TextForeColor;
            public Color FrmSelectWaiverLblWaiverTextForeColor;
            public Color FrmSelectWaiverLblSelectionTextForeColor;
            public Color FrmSelectWaiverDgvWaiverSetTextForeColor;
            public Color FrmSelectWaiverBtnProceedTextForeColor;
            public Color FrmSelectWaiverBtnCancelTextForeColor;
            public Color FrmSelectWaiverTxtMessageTextForeColor;

            //frmSignWaiverFiles
            public Color FrmSignWaiverFilesBtnHomeTextForeColor;
            public Color FrmSignWaiverFilesLblSignWaiverFileTextForeColor;
            public Color FrmSignWaiverFilesLblCustomerNameTextForeColor;
            public Color FrmSignWaiverFilesLblCustomerContactTextForeColor;
            public Color FrmSignWaiverFilesPnlWaiverTextForeColor;
            public Color FrmSignWaiverFilesPnlWaiverDisplayTextForeColor;
            public Color FrmSignWaiverFilesBtnOkayTextForeColor;
            public Color FrmSignWaiverFilesBtnCancelTextForeColor;
            public Color FrmSignWaiverFilesTxtMessageTextForeColor;
            public Color FrmSignWaiverFilesChkSignConfirmTextForeColor;

            //frmSignWaivers
            public Color FrmSignWaiversBtnHomeTextForeColor;
            public Color FrmSignWaiversLblSiteNameTextForeColor;
            public Color FrmSignWaiversLblCustomerTextForeColor;
            public Color FrmSignWaiversLblGreeting1TextForeColor;
            public Color FrmSignWaiversLblWaiverTextForeColor;
            public Color FrmSignWaiversLblSelectionTextForeColor;
            public Color FrmSignWaiversLabel1TextForeColor;
            public Color FrmSignWaiversFpnlWaiverSetTextForeColor;
            public Color FrmSignWaiversBtnCancelTextForeColor;
            public Color FrmSignWaiversTxtMessageTextForeColor;

            //frmViewWaiverUI
            public Color FrmViewWaiverUIBtnHomeTextForeColor;
            public Color FrmViewWaiverUILblWaiverNameTextForeColor;
            public Color FrmViewWaiverUIWebBrowserTextForeColor;
            public Color FrmViewWaiverUIBtnCloseTextForeColor;

            //frmEntitlements
            public Color FrmEntitlementLblMsgTextForeColor;
            public Color FrmEntitlementBtnMsgTextForeColor;
            public Color FrmEntitlementBtnPointsTextForeColor;

            //frmSplashScreen
            public Color FrmSplashScreenLabel1TextForeColor;

            //frmSuccessMsg
            public Color FrmSuccessMsgBtnHomeTextForeColor;
            public Color FrmSuccessMsgLblHeadingTextForeColor;
            public Color FrmSuccessMsgLblmsgTextForeColor;
            public Color FrmSuccessMsgLblBalanceMsgTextForeColor;
            public Color FrmSuccessMsgLblPointTextForeColor;
            public Color FrmSuccessMsgLblPasNoTextForeColor;
            public Color FrmSuccessMsgBtnCloseTextForeColor;
            public Color FrmSuccessMsgLblTrxNumberTextForeColor = Color.White;

            //FSKCoverpages
            public Color FSKCoverPageBtnExecuteOnlineTransactionTextForeColor;

            //frmPaymentGameCard
            public Color PaymentGameCardLblSiteNameTextForeColor;
            public Color PaymentGameCardLblPaymentTextTextForeColor;
            public Color PaymentGameCardLblTotaltoPayTextTextForeColor;
            public Color PaymentGameCardLblTotalToPayTextForeColor;
            public Color PaymentGameCardLabel8TextForeColor;
            public Color PaymentGameCardLabel7TextForeColor;
            public Color PaymentGameCardLblTapCardTextTextForeColor;
            public Color PaymentGameCardLblCardNumberTextTextForeColor;
            public Color PaymentGameCardLblCardNumberTextForeColor;
            public Color PaymentGameCardLblAvailableCreditsTextTextForeColor;
            public Color PaymentGameCardLblAvailableCreditsTextForeColor;
            public Color PaymentGameCardLblPurchaseValueTextTextForeColor;
            public Color PaymentGameCardLblPurchaseValueTextForeColor;
            public Color PaymentGameCardLblBalanceCreditsTextTextForeColor;
            public Color PaymentGameCardLblBalanceFreeEntriesTextForeColor;
            public Color PaymentGameCardLblAvailableFreeEntriesTextTextForeColor;
            public Color PaymentGameCardLblAvailableFreeEntriesTextForeColor;
            public Color PaymentGameCardLblBalanceFreeEntriesTextTextForeColor;
            public Color PaymentGameCardLblBalanceCreditsTextForeColor;
            public Color PaymentGameCardBtnApplyTextForeColor;
            public Color PaymentGameCardBtnCancelTextForeColor;
            public Color PaymentGameCardTxtMessageTextForeColor;
            public Color PaymentGameCardBtnHomeTextForeColor;
            public Color PaymentGameCardTxtMessageErrorBackColor = Color.Red;

            //frmLoyalty.cs
            public Color FrmLoyaltyLblLoyaltyTextTextForeColor;
            public Color FrmLoyaltyBtnLoyaltyYesTextForeColor;
            public Color FrmLoyaltyBtnLoyaltyNoTextForeColor;
            public Color FrmLoyaltyLblPhoneNoTextForeColor;
            public Color FrmLoyaltyTxtPhoneNoTextForeColor;
            public Color FrmLoyaltyBtnGoTextForeColor;
            public Color FrmLoyaltyListboxNamesTextForeColor;
            public Color FrmLoyaltyTxtFirstNameTextForeColor;
            public Color FrmLoyaltyBtnOkTextForeColor;
            public Color FrmLoyaltyBtnProceedWithoutLoyaltyTextForeColor;
            public Color FrmLoyaltyBtnCancelTextForeColor;
            public Color FrmLoyaltyTxtMessageTextForeColor;

            //frmAddCustomerRelation.cs
            public Color AddCustomerRelationBtnHomeTextForeColor;
            public Color AddCustomerRelationLblGreetingTextForeColor;
            public Color AddCustomerRelationLblCardNumberTextForeColor;
            public Color AddCustomerRelationLblCustomerNameTextForeColor;
            public Color AddCustomerRelationTxtCardNumberTextForeColor;
            public Color AddCustomerRelationTxtCustomerNameTextForeColor;
            public Color AddCustomerRelationLblAddParticipantsTextForeColor;
            public Color AddCustomerRelationBtnBackTextForeColor;
            public Color AddCustomerRelationBtnConfirmTextForeColor;
            public Color AddCustomerRelationTxtboxMessageLineTextForeColor;
            public Color AddCustomerRelationGridTextForeColor;
            public Color AddCustomerRelationLblTimeRemainingTextForeColor;
            public Color AddCustomerRelationLblCardNumberForeColor;
            public Color AddCustomerRelationLblCustomerNameForeColor;
            public Color AddCustomerRelationLblGridFooterTextForeColor;
            public Color AddCustomerRelationGridHeaderTextForeColor;

            //frmFundRaiserAndDonation.cs
            public Color FundsDonationsBtnProductTextForeColor = Color.Purple;
            public Color FundsDonationsBtnTextForeColor = Color.White;
            public Color FundsDonationsGreetingTextForeColor = Color.White;
            public Color FundsDonationsLblFundDonationMessageTextForeColor = Color.White;
            public Color FundsDonationsFooterTextForeColor = Color.Purple;

            //frmComboChildProductsQty
            public Color ComboChildProductsQtyGreetingLblTextForeColor = Color.White;
            public Color ComboChildProductsQtyBackButtonTextForeColor = Color.White;
            public Color ComboChildProductsQtyCancelButtonTextForeColor = Color.White;
            public Color ComboChildProductsQtyProceedButtonTextForeColor = Color.White;
            public Color ComboChildProductsQtyFooterTxtMsgTextForeColor = Color.White;
            public Color ComboChildProductsQtyProductButtonTextForeColor = Color.Black;
            public Color ComboChildProductsQtyQuantityTextForeColor = Color.Black;
            public Color ComboChildProductsQtyHomeButtonTextForeColor = Color.White;
            public Color ComboChildProductsQtyLblAgeCriteriaTextForeColor = Color.White;

            //frmSelectChild
            public Color SelectChildGreetingLblTxtForeColor = Color.White;
            public Color SelectChildYourfamilyLblTextForeColor = Color.White;
            public Color SelectChildYourFamilyGridTextForeColor = Color.Black;
            public Color SelectChildMemberDetailsGridTextForeColor = Color.Black;
            public Color SelectChildFooterTxtMsgTextForeColor = Color.White;
            public Color SelectChildProceedButtonTextForeColor = Color.White;
            public Color SelectChildSkipButtonTextForeColor = Color.White;
            public Color SelectChildBackButtonTextForeColor = Color.White;
            public Color SelectChildHomeButtonTextForeColor = Color.White;
            public Color SelectChildMemberDetailsLblTextForeColor = Color.White;
            public Color SelectChildGridHeaderTextForeColor;
            public Color SelectChildYourFamilyGridInfoTextForeColor;
            public Color SelectChildMemberDetailsGridInfoTextForeColor;

            //frmEnterChildDetails
            public Color EnterChildDetailsGreetingLblTextForeColor = Color.White;
            public Color EnterChildDetailsGridTextForeColor = Color.Black;
            public Color EnterChildDetailsFooterTextMsgTextForeColor = Color.White;
            public Color EnterChildDetailsProceedButtonTextForeColor = Color.White;
            public Color EnterChildDetailsChildAddedLabelTextForeColor = Color.White;
            public Color EnterChildDetailsBackButtonTextForeColor = Color.White;
            public Color EnterChildDetailsHomeButtonTextForeColor = Color.White;
            public Color EnterChildDetailsTimeRemainingLblTextForeColor = Color.White;
            public Color EnterChildDetailsGridHeaderTextForeColor;

            //frmChildSummary
            public Color ChildSummaryGreetingLblTextForeColor = Color.White;
            public Color ChildSummaryFooterTxtMsgTextForeColor = Color.White;
            public Color ChildSummaryProceedButtonTextForeColor = Color.White;
            public Color ChildSummaryBackButtonTextForeColor = Color.White;
            public Color ChildSummaryHomeButtonTextForeColor = Color.White;

            //frmSelectAdult
            public Color SelectAdultGreetingLblTxtForeColor = Color.White;
            public Color SelectAdultYourfamilyLblTextForeColor = Color.White;
            public Color SelectAdultYourFamilyGridTextForeColor = Color.Black;
            public Color SelectAdultMemberDetailsGridTextForeColor = Color.Black;
            public Color SelectAdultFooterTxtMsgTextForeColor = Color.White;
            public Color SelectAdultProceedButtonTextForeColor = Color.White;
            public Color SelectAdultSkipButtonTextForeColor = Color.White;
            public Color SelectAdultBackButtonTextForeColor = Color.White;
            public Color SelectAdultHomeButtonTextForeColor = Color.White;
            public Color SelectAdultMemberDetailsLblTextForeColor = Color.White;
            public Color SelectAdultGridFooterMsgLblTextForeColor = Color.White;
            public Color SelectAdultNoRelationMsg1LblTextForeColor = Color.White;
            public Color SelectAdultNoRelationMsg2LblTextForeColor = Color.White;
            public Color SelectAdultNoRelationMsg3LblTextForeColor = Color.White;
            public Color SelectAdultGridHeaderTextForeColor;
            public Color SelectAdultYourFamilyGridInfoTextForeColor;
            public Color SelectAdultMemberDetailsGridInfoTextForeColor;

            //frmEnterAdultDetails
            public Color EnterAdultDetailsGreetingLblTextForeColor = Color.White;
            public Color EnterAdultDetailsGridTextForeColor = Color.Black;
            public Color EnterAdultDetailsFooterTextMsgTextForeColor = Color.White;
            public Color EnterAdultDetailsProceedButtonTextForeColor = Color.White;
            public Color EnterAdultDetailsChildAddedLabelTextForeColor = Color.White;
            public Color EnterAdultDetailsBackButtonTextForeColor = Color.White;
            public Color EnterAdultDetailsHomeButtonTextForeColor = Color.White;
            public Color EnterAdultDetailsTimeRemainingLblTextForeColor = Color.White;
            public Color EnterAdultDetailsGridHeaderTextForeColor;

            //frmAdultSummary
            public Color AdultSummaryGreetingLblTextForeColor = Color.White;
            public Color AdultSummaryFooterTxtMsgTextForeColor = Color.White;
            public Color AdultSummaryProceedButtonTextForeColor = Color.White;
            public Color AdultSummaryBackButtonTextForeColor = Color.White;
            public Color AdultSummaryHomeButtonTextForeColor = Color.White;

            //frmCheckInSummary
            public Color CheckInSummaryDetailsTextForeColor = Color.White;
            public Color CheckInSummaryTotalToPayTextForeColor = Color.White;
            public Color CheckInSummaryHomeButtonTextForeColor = Color.White;
            public Color CheckInSummaryDiscountLabelTextForeColor = Color.White;
            public Color CheckInSummaryApplyDiscountBtnTextForeColor = Color.White;
            public Color CheckInSummaryCPValidityTextForeColor = Color.White;
            public Color CheckInSummaryBackButtonTextForeColor = Color.White;
            public Color CheckInSummaryProceedButtonTextForeColor = Color.White;
            public Color CheckInSummaryFooterMessageTextForeColor = Color.White;
            public Color CheckInSummaryLblPassportCouponTextForeColor = Color.White;
            public Color CheckInSummaryPanelUsrCtrlLblTextForeColor = Color.White;
            public Color CheckInSummaryPanelTextForeColor = Color.White;

            //frmEditDateOfBirth
            public Color CalenderMsgTextForeColor = Color.White;
            public Color CalenderBtnCancelTextForeColor = Color.White;
            public Color CalenderBtnSaveTextForeColor = Color.White;
            public Color CalenderItemHeaderTextForeColor = Color.White;
            public Color CalenderItemTextForeColor = Color.Black;
            public Color CalenderGridTextForeColor = Color.Black;
            public Color CalenderGreetingTextForeColor = Color.White;

            public Color PackageDetailsLblHeaderTextForeColor = Color.White;
            public Color PackageDetailsLblTextForeColor = Color.White;
            public Color GuestCountLblTextForeColor = Color.White;
            public Color ScreenDetailsLblTextForeColor = Color.White;
            public Color LblGridFooterMsgTextForeColor = Color.White;

            //frmHome.cs 
            public Color HomeScreenBtnNewCardTextForeColor = Color.White;
            public Color HomeScreenBtnRechargeTextForeColor = Color.White;
            public Color HomeScreenBtnPlaygroundEntryTextForeColor = Color.White;
            public Color HomeScreenBtnFoodAndBeveragesTextForeColor = Color.White;
            public Color HomeScreenBtnAttractionsTextForeColor = Color.White;

            //frmPurchase.cs 
            public Color PurchaseMenuGreetingTextForeColor = Color.White;
            public Color PurchaseMenuBtnHomeTextForeColor = Color.White;
            public Color PurchaseMenuFooterTextForeColor = Color.White;
            public Color PurchaseMenuLblSiteNameTextForeColor = Color.White;
            public Color PurchaseMenuBtnTextForeColor = Color.White;

            //frmGetEmailDetails
            public Color FrmGetEmailDetailsTxtCustomerEmailTextForeColor = Color.Black;
            public Color FrmGetEmailDetailsLblGreeting1TextForeColor = Color.White;
            public Color FrmGetEmailDetailsTxtUserEntryEmailTextForeColor = Color.Black;
            public Color FrmGetEmailDetailsLabel1TextForeColor = Color.White;
            public Color FrmGetEmailDetailsLabel2TextForeColor = Color.White;
            public Color FrmGetEmailDetailsLabel3TextForeColor = Color.White;
            public Color FrmGetEmailDetailsBtnCancelTextForeColor = Color.White;
            public Color FrmGetEmailDetailsBtnOkTextForeColor = Color.White;
            public Color FrmGetEmailDetailsFooterTxtMsgTextForeColor = Color.White;
            public Color FrmGetEmailDetailsLblSkipDetailsTextForeColor = Color.White;
            public Color FrmGetEmailDetailsLblNoteTextForeColor = Color.White;

            //frmReceiptDeliveryModeOptions
            public Color FrmReceiptDeliveryModeOptionsLblGreeting1TextForeColor = Color.White;
            public Color FrmReceiptDeliveryModeOptionsBtnPrintTextForeColor = Color.White;
            public Color FrmReceiptDeliveryModeOptionsBtnEmailTextForeColor = Color.White;
            public Color FrmReceiptDeliveryModeOptionsBtnNoneTextForeColor = Color.White;

            //UsrCtrlCart
            public Color UsrCtrlCartLblProductDescriptionTextForeColor = Color.Black;
            public Color UsrCtrlCartLblTotalPriceTextForeColor = Color.Black;
            public Color KioskCartQuantityTextForeColor = Color.White;

            //frmKioskCart
            public Color FrmKioskCartLblGreeting1TextForeColor = Color.White;
            public Color FrmKioskCartButtonTextForeColor = Color.White;
            public Color FrmKioskCartDiscountButtonTextForeColor = Color.White;
            public Color FrmKioskCartLblHeaderTextForeColor = Color.White;
            public Color FrmKioskCartLblAmountTextForeColor = Color.White;
            public Color FrmKioskCartFooterTxtMsgTextForeColor = Color.White;
            public Color FrmKioskCartLblProductHeaderTextForeColor = Color.White;

            //PreSelectPaymentMode
            public Color PreSelectPaymentModeBtnTextForeColor = Color.White;
            public Color PreSelectPaymentModeLblHeadingTextForeColor = Color.White;
            public Color PreSelectPaymentModeBackButtonTextForeColor = Color.White;
            public Color PreSelectPaymentModeCancelButtonTextForeColor = Color.White;
            public Color PreSelectPaymentModeBtnHomeTextForeColor = Color.White;

            //frmKioskPrintSummary
            public Color PrintSummarySiteTextForeColor = Color.White;
            public Color PrintSummaryLblGreetingTextForeColor = Color.White;
            public Color PrintSummaryLblFromDateTextForeColor = Color.White;
            public Color PrintSummaryLblFromDateTimeForeColor = Color.DarkOrchid;
            public Color PrintSummaryLblToDateTextForeColor = Color.White;
            public Color PrintSummaryLblToDateTimeForeColor = Color.DarkOrchid;
            public Color PrintSummaryBtnPrevTextForeColor = Color.White;
            public Color PrintSummaryBtnPrintTextForeColor = Color.White;
            public Color PrintSummaryBtnEmailTextForeColor = Color.White;
            public Color PrintSummaryNewTabTextForeColor = Color.White;
            public Color PrintSummaryRePrintTabTextForeColor = Color.White;
            public Color PrintSummaryComboTextForeColor = Color.DarkOrchid;
            public Color PrintSummaryLblTabTextForeColor = Color.DarkOrchid;

            //frmTextBox
            public Color TextBoxLblMsgTextForeColor = Color.DarkOrchid;
            public Color TextBoxTxtDataTextForeColor = Color.DarkOrchid;

            //frmComboDetails
            public Color ComboDetailsBtnHomeTextForeColor;
            public Color ComboDetailsProductNameTextForeColor;
            public Color ComboDetailsPackageMsgTextForeColor;
            public Color ComboDetailsBackButtonTextForeColor;
            public Color ComboDetailsCancelButtonTextForeColor;
            public Color ComboDetailsProceedButtonTextForeColor;
            public Color ComboDetailsFooterTextForeColor;

            //usrCtrlComboDetails
            public Color UsrCtrlComboDetailsProductNameTextForeColor;
            public Color UsrCtrlComboDetailsNumberTextForeColor;

            //frmAttractionQty
            public Color AttractionQtyHomeButtonTextForeColor;
            public Color AttractionQtyBackButtonTextForeColor;
            public Color AttractionQtyCancelButtonTextForeColor;
            public Color AttractionQtyProceedButtonTextForeColor;
            public Color AttractionQtyFooterTextForeColor;
            public Color AttractionQtyLblYourSelectionsTextForeColor;
            public Color AttractionQtyHeadersTextForeColor;
            public Color AttractionQtyValuesTextForeColor;
            public Color AttractionQtyTxtQtyTextForeColor;
            public Color AttractionQtyLblHowManyMsgTextForeColor;
            public Color AttractionQtyLblEnterQtyMsgTextForeColor;
            public Color AttractionQtyLblBuyTextTextForeColor;
            public Color AttractionQtyLblProductNameTextForeColor;
            public Color AttractionQtyLblComboDetailsTextForeColor;
            public Color AttractionQtyLblProductDescriptionTextForeColor;

            //frmProcessingAttractions
            public Color ProcessingAttractionsLblProcessingMsgTextForeColor;
            public Color ProcessingAttractionsFooterTxtMsgTextForeColor;
            public Color ProcessingAttractionsProceedButtonTextForeColor;
            public Color ProcessingAttractionsCancelButtonTextForeColor;
            public Color ProcessingAttractionsBackButtonTextForeColor;
            public Color ProcessingAttractionsHomeButtonTextForeColor;

            //frmCardSaleOption
            public Color CardSaleOptionHeaderTextForeColor;
            public Color CardSaleOptionCancelBtnTextForeColor;
            public Color CardSaleOptionConfirmBtnTextForeColor;
            public Color CardSaleOptionLblNewCardTextForeColor;
            public Color CardSaleOptionLblExistingCardTextForeColor;

            //frmSelectSlot
            public Color SelectSlotHomeButtonTextForeColor;
            public Color SelectSlotLblBookingSlotTextForeColor;
            public Color SelectSlotLblProductNameTextForeColor;
            public Color SelectSlotBackButtonTextForeColor;
            public Color SelectSlotCancelButtonTextForeColor;
            public Color SelectSlotProceedButtonTextForeColor;
            public Color SelectSlotFooterTextForeColor;
            public Color SelectSlotLblDateTextForeColor;
            public Color SelectSlotLblSelectSlotTextForeColor;
            public Color SelectSlotBtnDatesTextForeColor;
            public Color SelectSlotLblNoSchedulesTextForeColor;

            //usrCtrlSlot
            public Color UsrCtrlSlotLblSlotInfo;
            public Color UsrCtrlSlotLblScheduleTime;

            //frmAttractionSummary
            public Color AttractionSummaryLblBookingInfoForeColor;
            public Color AttractionSummaryLblProductNameForeColor;
            public Color AttractionSummaryLblQtyForeColor;
            public Color AttractionSummaryBtnPrevForeColor;
            public Color AttractionSummaryBtnCancelForeColor;
            public Color AttractionSummaryBtnProceedForeColor;
            public Color AttractionSummaryTxtMessageForeColor;
            public Color AttractionSummaryBtnHomeForeColor;

            //usrCtrlAttrcationSummary
            public Color UsrCtrlAttrcationSummaryLblProductNameTextForeColor;
            public Color UsrCtrlAttrcationSummaryLblSlotDetailsTextForeColor;

            //usrCtrlAttrcationSummary
            public Color frmAddToCartAlertBtnCloseTextForeColor;
            public Color frmAddToCartAlertLblMsgTextForeColor;
            public Color frmAddToCartAlertBtnCheckOutTextForeColor;

            //frmFetchCustomer
            public Color SearchCustomerSearchBtnTextForeColor;
            public Color SearchCustomerNewRegistrationBtnTextForeColor;
            public Color SearchCustomerPrevBtnTextForeColor;
            public Color SearchCustomerFooterTextForeColor;
            public Color SearchCustomerGreetingForeColor;
            public Color SearchCustomerLblNewCustomerHeaderForeColor;
            public Color SearchCustomerLblNewCustomerForeColor;
            public Color SearchCustomerExistingCustomerHeaderForeColor;
            public Color SearchCustomerExistingCustomerForeColor;
            public Color SearchCustomerLblEnterPhoneHeaderForeColor;
            public Color SearchCustomerLblEnterEmailHeaderForeColor;
            public Color SearchCustomerTxtEmailIdForeColor;
            public Color SearchCustomerTxtPhoneNumForeColor;

            //frmCustomerFound
            public Color CustomerFoundBtnOKTextForeColor;
            public Color CustomerFoundBtnPrevTextForeColor;
            public Color CustomerFoundLblCustomerNameTextForeColor;
            public Color CustomerFoundLblWelcomeMsgTextForeColor;
            public Color CustomerFoundLblClickOKMsgTextForeColor;

            //frmPickCustomer
            public Color SelectCustomerLblGreetingTextForeColor;
            public Color SelectCustomerBtnProceedTextForeColor;
            public Color SelectCustomerBtnCancelTextForeColor;
            public Color SelectCustomerBtnPrevTextForeColor;
            public Color SelectCustomerLblFirstNameTextForeColor;
            public Color SelectCustomerLblLastNameTextForeColor;
            public Color SelectCustomerFooterTextForeColor;
            public Color SelectCustomerBackButtonTextForeColor;

            //usrCtrlPickCustomer
            public Color UsrCtrlSelectCustomerLblFirstNameTextForeColor;
            public Color UsrCtrlSelectCustomerLblLastNameTextForeColor;

            //frmWaiverSigningAlert
            public Color WaiverSigningAlertLblGreetingTextForeColor;
            public Color WaiverSigningAlertLblMsgTextForeColor;
            public Color WaiverSigningAlertBackButtonTextForeColor;
            public Color WaiverSigningAlertCancelButtonTextForeColor;
            public Color WaiverSigningAlertSignButtonTextForeColor;
            public Color WaiverSigningAlertFooterTextForeColor;

            //usrCtrlWaiverSigningAlert
            public Color UsrCtrlWaiverSigningAlertLblProductNameTextForeColor;
            public Color UsrCtrlWaiverSigningAlertLblParticipantsTextForeColor;

            //MapAttendees
            public Color MapAttendeesLblGreetingTextForeColor;
            public Color MapAttendeesLblProductNameTextForeColor;
            public Color MapAttendeesLblMsgTextForeColor;
            public Color MapAttendeestBackButtonTextForeColor;
            public Color MapAttendeesCancelButtonTextForeColor;
            public Color MapAttendeesAssignButtonTextForeColor;
            public Color MapAttendeesFooterTextForeColor;
            public Color MapAttendeesLblQuantityTextForeColor;
            public Color MapAttendeesLblAssignParticipantsTextForeColor;
            public Color UsrCtrlMapAttendeesToQuantityBtnProductQtySelectedTextForeColor;
            public Color MapAttendeesMappedCustomerNameTextForeColor;            

            //usrCtrlMapAttendeeToProduct
            public Color UsrCtrlMapAttendeeToProductBtnProductTextForeColor;
            public Color UsrCtrlMapAttendeeToProductBtnProductHighlightTextForeColor;

            //usrCtrlMapAttendeesToQuantity
            public Color UsrCtrlMapAttendeesToQuantityLblQtyTextForeColor;
            public Color UsrCtrlMapAttendeesToQuantityLblQtySelectedTextForeColor;

            //frmCustomerRelations
            public Color CustomerRelationsGreetingTextForeColor;
            public Color CustomerRelationsBackButtonTextForeColor;
            public Color CustomerRelationsAddNewMemberButtonTextForeColor;
            public Color CustomerRelationsSearchAnotherCustomerButtonTextForeColor;
            public Color CustomerRelationsProceedButtonTextForeColor;
            public Color CustomerRelationsFooterTextForeColor;

            //UsrCtrlCustomer
            public Color UsrCtrlCustomerNameTextForeColor;

            //UsrCtrlCustomerRelations
            public Color CustomerRelationsRelatedCustomerNameTextForeColor;
            public Color CustomerRelationsRelationshipTextForeColor;
            public Color CustomerRelationsSignStatusTextForeColor;
            public Color CustomerRelationsValidityTextForeColor;

            //frmFilteredCustomers
            public Color FilteredCustomersGreetingTextForeColor;
            public Color FilteredCustomersCloseButtonTextForeColor;
            public Color FilteredCustomersLinkButtonTextForeColor;

            //usrCtrlFilteredCustomers
            public Color UsrCtrlFilteredCustomersCustomerNameTextForeColor;
        }
        public static ContentAlignment GetContextAligment(string textAlignmentValue)
        {
            log.LogMethodEntry(textAlignmentValue);
            ContentAlignment contentAlignment = ContentAlignment.MiddleLeft;
            switch (textAlignmentValue.ToUpper())
            {
                case "TOPLEFT":
                    contentAlignment = ContentAlignment.TopLeft;
                    break;
                case "TOPCENTER":
                    contentAlignment = ContentAlignment.TopCenter;
                    break;
                case "TOPRIGHT":
                    contentAlignment = ContentAlignment.TopRight;
                    break;
                case "MIDDLELEFT":
                    contentAlignment = ContentAlignment.MiddleLeft;
                    break;
                case "MIDDLECENTER":
                    contentAlignment = ContentAlignment.MiddleCenter;
                    break;
                case "MIDDLERIGHT":
                    contentAlignment = ContentAlignment.MiddleRight;
                    break;
                case "BOTTOMLEFT":
                    contentAlignment = ContentAlignment.BottomLeft;
                    break;
                case "BOTTOMCENTER":
                    contentAlignment = ContentAlignment.BottomCenter;
                    break;
                case "BOTTOMRIGHT":
                    contentAlignment = ContentAlignment.BottomRight;
                    break;
            }
            log.LogMethodExit();
            return contentAlignment;
        }
        public static ThemeClass CurrentTheme;

        public class configuration
        {
            public int baport;
            public int prport;
            public int dispport;
            public int coinAcceptorport;
            public int cardAcceptorport;

            public class acceptorInfo
            {
                public string Name;
                public Image Image;
                public decimal Value;
                public string HexCode;
                public bool isToken = false;
            }

            public acceptorInfo[] Notes = new acceptorInfo[11];
            public acceptorInfo[] Coins = new acceptorInfo[11];
        }
        public static configuration config = new KioskStatic.configuration();


        public class AlohaLoyaltyMember
        {
            String[][] memberValues;
            string[] memberColumns;

            public String[][] values
            {
                get { return memberValues; }
                set { memberValues = value; }
            }

            public String[] columnNames
            {
                get { return memberColumns; }
                set { memberColumns = value; }
            }
        }

        public class AlohaMemberDetails
        {
            List<String> LoyaltyCardNumber;
            List<String> FirstName;
            List<String> LastName;
            List<String> PhoneNumber;
            public AlohaMemberDetails()
            {
                log.LogMethodEntry();
                LoyaltyCardNumber = new List<String>();
                FirstName = new List<String>();
                LastName = new List<String>();
                PhoneNumber = new List<String>();
                log.LogMethodExit();
            }
            public AlohaMemberDetails(String cardNumber, String firstName, String lastName, String phoneNumber)
                : this()
            {
                log.LogMethodEntry(cardNumber, firstName, lastName, phoneNumber);
                LoyaltyCardNumber.Add(cardNumber.ToString());
                FirstName.Add(firstName.ToString());
                LastName.Add(lastName.ToString());
                PhoneNumber.Add(phoneNumber.ToString());
                log.LogMethodExit();
            }
            public List<String> AlohaLoyaltyCardNumber
            {
                get { return LoyaltyCardNumber; }
                set { LoyaltyCardNumber = value; }
            }
            public List<String> LoyaltyFirstName
            {
                get { return FirstName; }
                set { FirstName = value; }
            }
            public List<String> LoyaltyLastName
            {
                get { return LastName; }
                set { LastName = value; }
            }
            public List<String> LoyaltyPhoneNumber
            {
                get { return PhoneNumber; }
                set { PhoneNumber = value; }
            }
        }
        public class receipt_format
        {
            public string head;
            public string a1;
            public string a2;
            public string a21;
            public string a3;
            public string a4;
            public string a5;
            public string a6;
            public string f1;
            public string f2;
            public string f21;
            public string f3;
            public string f4;
            public string f5;
            public string f6;
        }
        public static receipt_format rc;

        public static void get_receipt()
        {
            log.LogMethodEntry();
            rc = new receipt_format();
            SqlCommand sqlcmd = Utilities.getCommand();
            sqlcmd.CommandText = "select * from site";
            SqlDataReader rsreceipt;
            rsreceipt = sqlcmd.ExecuteReader();
            if (rsreceipt.Read())
            {
                rc.head = Utilities.ParafaitEnv.SiteName;
                rc.a1 = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Date") + ": @Date";
                rc.a2 = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Bill No") + ": @TrxNo " + MessageContainerList.GetMessage(Utilities.ExecutionContext, "TrxId") + ": @TrxId";
                rc.a21 = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Kiosk") + ": @POS";
                rc.a3 = MessageContainerList.GetMessage(Utilities.ExecutionContext, "GST Reg No") + ": " + KioskStatic.Utilities.ParafaitEnv.TaxIdentificationNumber;
                rc.a4 = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Product") + "            " + MessageContainerList.GetMessage(Utilities.ExecutionContext, "Qt.") + " " + MessageContainerList.GetMessage(Utilities.ExecutionContext, "Price") + "      " + MessageContainerList.GetMessage(Utilities.ExecutionContext, "Tax") + "        " + MessageContainerList.GetMessage(Utilities.ExecutionContext, "Amt");
                rc.a5 = "-------            --- -----      ---        ---";
                rc.a6 = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Card") + ": @CardNumber";
                rc.f1 = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Transaction Total") + " @Total";
                rc.f2 = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Credits") + ": @CreditBalance";
                rc.f21 = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Bonus") + ": @BonusBalance";
                rc.f3 = MessageContainerList.GetMessage(Utilities.ExecutionContext, "e-Tickets") + ": @Tickets";
                rc.f4 = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Loyalty Points") + ": @LoyaltyPoints";
                rc.f5 = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Have a Nice Day") + " @CustomerName";
                rc.f6 = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Thank You, Visit Again");
            }
            rsreceipt.Close();
            log.LogMethodExit();
        }

        public static Dictionary<string, string> configDictionary = new Dictionary<string, string>()
        {
            { "PrinterPort", "PRINTER_PORT" },
            { "BillAcceptorPort", "BILL_ACCEPTOR_PORT" },
            { "CardDispenserPort", "CARD_DISPENSER_PORT" },
            { "CoinAcceptorPort", "COIN_ACCEPTOR_PORT" },
            { "CardAcceptorPort", "CARD_ACCEPTOR_PORT" },
            { "UIThemeNo", "UI_THEME_NO" },
            { "DisablePurchase", "DISABLE_PURCHASE" },
            { "RegistrationAllowed", "REGISTRATION_ALLOWED" },
            { "DisableNewCard", "DISABLE_NEW_CARD" },
            { "IgnorePrinterError", "IGNORE_PRINTER_ERROR" },
            { "ShowBonus", "SHOW_BONUS" },
            { "ShowCourtesy", "SHOW_COURTESY" },
            { "ShowGames", "SHOW_GAMES" },
            { "ShowTickets", "SHOW_TICKETS" },
            { "ShowVirtualPoints", "SHOW_VIRTUAL_POINTS" },
            { "ShowLoyaltyPoints", "SHOW_LOYALTY_POINTS" },
            { "EnableTransfer", "ENABLE_TRANSFER" },
            { "EnableRedeemTokens", "ENABLE_REDEEM_TOKENS" },
            { "EnableFAQ", "ENABLE_FAQ" },
            { "ShowTime", "SHOW_TIME" },
            { "DisableCustomerRegistration", "DISABLE_CUSTOMER_REGISTRATION" },
            { "AllowPointsToTimeConversion", "ALLOW_POINTS_TO_TIME_CONVERSION" },
            { "EnablePauseCard","ENABLE_PAUSE_CARD" },
            { "EnableWaiverSignInKiosk","ENABLE_WAIVER_SIGN_IN_KIOSK" },
            { "EnablePlaygroundEntry","ENABLE_CHECK-IN_PRODUCTS_IN_KIOSK" },
            { "DebugMode", "DEBUG_MODE" },
            { "AllowRegisterWithoutCard", "REGISTER_CUSTOMER_WITHOUT_CARD" }
        };

        public static void get_config(ExecutionContext executionContext, string source = "")
        {
            log.LogMethodEntry(source);
            try
            {
                config.prport = Convert.ToInt32(getConfigValue("PrinterPort"));
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while converting to Int ", ex);
                config.prport = -1;
            }
            try
            {
                config.baport = Convert.ToInt32(getConfigValue("BillAcceptorPort"));
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while converting to Int ", ex);
                config.baport = -1;
            }
            try
            {
                config.dispport = Convert.ToInt32(getConfigValue("CardDispenserPort"));
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while converting to Int ", ex);
                config.dispport = -1;
            }
            try
            {
                config.coinAcceptorport = Convert.ToInt32(getConfigValue("CoinAcceptorPort"));
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while converting to Int ", ex);
                config.coinAcceptorport = -1;
            }
            try
            {
                config.cardAcceptorport = Convert.ToInt32(getConfigValue("CardAcceptorPort"));
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while converting to Int ", ex);
                config.cardAcceptorport = -1;
            }
            SqlCommand sqlcmd = Utilities.getCommand();
            sqlcmd.CommandText = "select * from KioskMoneyAcceptorInfo where Active = 'Y' order by NoteCoinFlag, DenominationId";
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int denominationId = Convert.ToInt32(dt.Rows[i]["DenominationId"]);
                if (denominationId < 1 || denominationId > 10)
                    continue;

                switch (dt.Rows[i]["NoteCoinFlag"].ToString())
                {
                    case "N":
                        {
                            config.Notes[denominationId] = new configuration.acceptorInfo();
                            config.Notes[denominationId].Name = dt.Rows[i]["Name"].ToString();
                            config.Notes[denominationId].Value = Convert.ToDecimal(dt.Rows[i]["Value"]);
                            config.Notes[denominationId].Image = Utilities.ConvertToImage(dt.Rows[i]["Image"]);
                            config.Notes[denominationId].HexCode = dt.Rows[i]["AcceptorHexCode"].ToString();
                            config.Notes[denominationId].isToken = false;
                            break;
                        }
                    case "C":
                    case "T":
                        {
                            config.Coins[denominationId] = new configuration.acceptorInfo();
                            config.Coins[denominationId].Name = dt.Rows[i]["Name"].ToString();
                            config.Coins[denominationId].Value = Convert.ToDecimal(dt.Rows[i]["Value"]);
                            config.Coins[denominationId].Image = Utilities.ConvertToImage(dt.Rows[i]["Image"]);
                            config.Coins[denominationId].HexCode = dt.Rows[i]["AcceptorHexCode"].ToString();
                            config.Coins[denominationId].isToken = dt.Rows[i]["NoteCoinFlag"].ToString().Equals("T");
                            break;
                        }
                }
            }

            string strCoinAcceptorModel = Utilities.getParafaitDefaults("COIN_ACCEPTOR");
            string strBillAcceptorModel = Utilities.getParafaitDefaults("BILL_ACCEPTOR");
            string strCardDispenserModel = Utilities.getParafaitDefaults("CARD_DISPENSER");
            try//25-06-2015:starts
            {//changes for admin activity log option
                showActivityDuration = Utilities.getParafaitDefaults("KIOSK_ACTIVITY_DISPLAY_DURATION");
            }
            catch { }//25-06-2015:Ends

            if (Enum.IsDefined(typeof(Semnox.Parafait.KioskCore.CoinAcceptor.CoinAcceptor.Models), strCoinAcceptorModel))
                CoinAcceptorModel = (Semnox.Parafait.KioskCore.CoinAcceptor.CoinAcceptor.Models)Enum.Parse(typeof(Semnox.Parafait.KioskCore.CoinAcceptor.CoinAcceptor.Models), strCoinAcceptorModel, true);
            else
                CoinAcceptorModel = Semnox.Parafait.KioskCore.CoinAcceptor.CoinAcceptor.Models.UCA2;

            if (Enum.IsDefined(typeof(Semnox.Parafait.KioskCore.BillAcceptor.BillAcceptor.Models), strBillAcceptorModel))
                BillAcceptorModel = (Semnox.Parafait.KioskCore.BillAcceptor.BillAcceptor.Models)Enum.Parse(typeof(Semnox.Parafait.KioskCore.BillAcceptor.BillAcceptor.Models), strBillAcceptorModel, true);
            else
                BillAcceptorModel = Semnox.Parafait.KioskCore.BillAcceptor.BillAcceptor.Models.NV9USB; //BillAcceptor.Models.ICTL77; --Legacy/open port Cleanup

            if (Enum.IsDefined(typeof(Semnox.Parafait.KioskCore.CardDispenser.CardDispenser.Models), strCardDispenserModel))
                CardDispenserModel = (Semnox.Parafait.KioskCore.CardDispenser.CardDispenser.Models)Enum.Parse(typeof(Semnox.Parafait.KioskCore.CardDispenser.CardDispenser.Models), strCardDispenserModel, true);
            else
                CardDispenserModel = Semnox.Parafait.KioskCore.CardDispenser.CardDispenser.Models.K720;

            getTheme(executionContext, source);

            try
            {
                debugMode = Convert.ToBoolean(getConfigValue("DebugMode"));
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while converting to Boolean ", ex);
            }

            if (!Directory.Exists(LogDirectory))
                Directory.CreateDirectory(LogDirectory);

            foreach (string f in Directory.GetFiles(LogDirectory))
            {
                if (File.GetCreationTime(f) < DateTime.Now.AddDays(-7))
                    File.Delete(f);
            }

            getParafaitDefaults();

            PaymentGatewayFactory.GetInstance().Initialize(Utilities, true, null, KioskStatic.KioskActivityWriteToLog);

            getAllPaymentModes();
            getPaymentModes();
            initializePaymentModes();
            try { ShowBonus = getConfigValue("ShowBonus").Equals("Y") ? true : false; }
            catch { }
            try { ShowTickets = getConfigValue("ShowTickets").Equals("Y") ? true : false; }
            catch { }
            try { ShowLoyaltyPoints = getConfigValue("ShowLoyaltyPoints").Equals("Y") ? true : false; }
            catch { }
            try { EnableTransfer = (!getConfigValue("EnableTransfer").Equals("N") && !getConfigValue("EnableTransfer").Equals("DISABLE")) ? true : false; }
            catch { }
            try { EnableRedeemTokens = getConfigValue("EnableRedeemTokens").Equals("Y") ? true : false; }
            catch { }
            try { EnableFAQ = getConfigValue("EnableFAQ").Equals("Y") ? true : false; }
            catch { }
            try { ShowCourtesy = getConfigValue("ShowCourtesy").Equals("Y") ? true : false; }
            catch { }
            try { ShowGames = getConfigValue("ShowGames").Equals("Y") ? true : false; }
            catch { }
            try { ShowTime = getConfigValue("ShowTime").Equals("Y") ? true : false; }
            catch { }
            try { DisablePurchase = getConfigValue("DisablePurchase").Equals("Y") ? true : false; }
            catch { }
            try { DisableNewCard = getConfigValue("DisableNewCard").Equals("Y") ? true : false; }
            catch { }
            try { RegistrationAllowed = getConfigValue("RegistrationAllowed").Equals("Y") ? true : false; }
            catch { }
            try { DisableCustomerRegistration = getConfigValue("DisableCustomerRegistration").Equals("Y") ? true : false; }
            catch { }
            try { AllowPointsToTimeConversion = getConfigValue("AllowPointsToTimeConversion").Equals("Y") ? true : false; }
            catch { }
            try { EnablePauseCard = getConfigValue("EnablePauseCard").Equals("Y") ? true : false; }
            catch { }
            try { EnableWaiverSignInKiosk = getConfigValue("EnableWaiverSignInKiosk").Equals("Y") ? true : false; }
            catch { }
            try { EnablePlaygroundEntry = getConfigValue("EnablePlaygroundEntry").Equals("Y") ? true : false; }
            catch { }
            try { ShowVirtualPoints = getConfigValue("ShowVirtualPoints").Equals("Y") ? true : false; }
            catch { }
            getSiteHeading();

            try
            {
                CardDispenserAddress = Convert.ToInt32(Utilities.getParafaitDefaults("CARD_DISPENSER_ADDRESS"));
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing getParafaitDefaults(CARD_DISPENSER_ADDRESS)", ex);
                CardDispenserAddress = 8;
            }
            try
            {
                UpdateConfigs();
            }
            catch (Exception ex)
            {
                log.Error("Error occured while updating the config", ex);
            }
            try
            {
                SetKioskTypeInAppConfig(source);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while updating the config", ex);
            }
            try
            {
                GetDateMonthFormat();
            }
            catch (Exception ex)
            {
                log.Error("Error occured in GetDateMonthFormat()", ex);
            }
            try
            {
                SetCalendarSelection();
            }
            catch (Exception ex)
            {
                log.Error("Error occured in SetCalendarSelection()", ex);
            }
        }

        public static void UpdateConfigs()
        {
            log.LogMethodEntry();
            bool isRuntimeConfigExist = false;
            string runtimeNewtonsoftConfig = "<runtime>" +
                                           "<assemblyBinding xmlns='urn:schemas-microsoft-com:asm.v1'>" +
                                            "<dependentAssembly>" +
                                            "<assemblyIdentity name='Newtonsoft.Json' publicKeyToken='30ad4fe6b2a6aeed' culture='neutral' />" +
                                            "<bindingRedirect oldVersion='0.0.0.0 - 12.0.0.0' newVersion='12.0.0.0' /> " +
                                            "</dependentAssembly>" +
                                            "</assemblyBinding>" +
                                            "</runtime >";

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                foreach (XmlElement element in xmlDoc.DocumentElement.Cast<XmlNode>().Where(n => n.NodeType != XmlNodeType.Comment))
                {
                    if (element.Name == "runtime")
                    {
                        if (element.InnerXml.Contains("Newtonsoft.Json"))
                        {
                            isRuntimeConfigExist = true;
                        }
                    }
                }

                //create a new element if runtime config element doesn't exists
                if (!isRuntimeConfigExist)
                {
                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    xfrag.InnerXml = runtimeNewtonsoftConfig;
                    xmlDoc.DocumentElement.AppendChild(xfrag);
                }

                if (!(isRuntimeConfigExist))
                {
                    xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                    ConfigurationManager.RefreshSection("runtime");
                }

                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                XmlNode appSettingsNode = xmlDoc.SelectSingleNode("/configuration/appSettings");
                if (appSettingsNode == null)
                {
                    appSettingsNode = CreateAppSettingsNode(xmlDoc);
                }
                bool updated = false;

                if (IsAppSettingExists(appSettingsNode, "SMARTRO_TIMEOUT") == false)
                {
                    AddToAppSettings(xmlDoc, appSettingsNode, "SMARTRO_TIMEOUT", "60");
                    updated = true;
                }
                if (updated)
                {
                    xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                    ConfigurationManager.RefreshSection("appSettings");
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                throw new Exception(ex.Message);
            }
        }

        private static void AddToAppSettings(XmlDocument xmlDoc, XmlNode appSettingsNode, string key, string value)
        {
            log.LogMethodEntry(xmlDoc, appSettingsNode, key, value);
            XmlElement exeMode = xmlDoc.CreateElement("add");
            exeMode.SetAttribute("key", key);
            exeMode.SetAttribute("value", value);
            appSettingsNode.AppendChild(exeMode);
            log.LogMethodExit();
        }

        private static bool IsAppSettingExists(XmlNode appSettingsNode, string key)
        {
            log.LogMethodEntry(appSettingsNode, key);
            if (appSettingsNode.SelectSingleNode(@"add[@key='" + key + "']") != null)
            {
                log.LogMethodExit(true);
                return true;
            }
            log.LogMethodExit(false);
            return false;
        }

        private static XmlNode CreateAppSettingsNode(XmlDocument xmlDoc)
        {
            log.LogMethodEntry(xmlDoc);
            XmlElement appSettingsElement = xmlDoc.CreateElement("appSettings");
            xmlDoc.DocumentElement.AppendChild(appSettingsElement);
            log.LogMethodExit(xmlDoc);
            return appSettingsElement;
        }

        public static void KioskActivityWriteToLog(int KioskTrxId, string Activity, int TrxId, int Value, string Message, int POSMachineId, string POSMachine)
        {
            log.LogMethodEntry(KioskTrxId, Activity, TrxId, Value, Message, POSMachineId, POSMachine);
            string message = string.Join("Kiosk Transaction Id:" + KioskTrxId + Environment.NewLine,
                                         "Activity :" + Activity + Environment.NewLine,
                                         "Transaction Id: " + TrxId + Environment.NewLine,
                                         "Value :" + Value + Environment.NewLine,
                                         "Message :" + Message + Environment.NewLine,
                                         "KioskId :" + POSMachineId + Environment.NewLine,
                                         "Kiosk Name :" + POSMachine);
            logToFile(message);
            log.LogMethodExit();
        }

        public static void getParafaitDefaults()
        {
            log.LogMethodEntry();
            try
            {
                MONEY_SCREEN_TIMEOUT = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("MONEY_SCREEN_TIMEOUT")));
            }
            catch (Exception ex)
            {
                log.Error("MONEY_SCREEN_TIMEOUT", ex);
                MONEY_SCREEN_TIMEOUT = 60;
            }


            try
            {
                TIME_IN_MINUTES_PER_CREDIT = Convert.ToDouble(KioskStatic.Utilities.getParafaitDefaults("TIME_IN_MINUTES_PER_CREDIT"));
            }
            catch (Exception ex)
            {
                log.Error("TIME_IN_MINUTES_PER_CREDIT", ex);
                TIME_IN_MINUTES_PER_CREDIT = 0;
            }

            ProductScreenGreeting = Utilities.getParafaitDefaults("KIOSK_PRODUCT_SCREEN_GREETING");
            VIPTerm = Utilities.getParafaitDefaults("VIP_TERM_VARIANT");

            try
            {
                //AllowRegisterWithoutCard = getConfigValue("AllowRegisterWithoutCard").ToLower().Equals("true");
                AllowRegisterWithoutCard = Utilities.getParafaitDefaults("REGISTER_CUSTOMER_WITHOUT_CARD").Equals("Y");
            }
            catch (Exception ex)
            {
                log.Error("REGISTER_CUSTOMER_WITHOUT_CARD", ex);
                AllowRegisterWithoutCard = false;
            }

            PlayKioskAudio = Utilities.getParafaitDefaults("PLAY_KIOSK_AUDIO").Equals("Y");

            ENABLE_CLOSE_IN_READ_CARD_SCREEN = Utilities.getParafaitDefaults("ENABLE_CLOSE_IN_READ_CARD_SCREEN").Equals("Y");
            DISABLE_PURCHASE_ON_CARD_LOW_LEVEL = Utilities.getParafaitDefaults("DISABLE_PURCHASE_ON_CARD_LOW_LEVEL").Equals("Y");

            try { AllowOverPay = Utilities.getParafaitDefaults("ALLOW_OVERPAY_IN_KIOSK").Equals("Y"); }
            catch (Exception ex)
            {
                log.Error("ALLOW_OVERPAY_IN_KIOSK", ex);
            }
            try { RegisterBeforePurchase = Utilities.getParafaitDefaults("REGISTER_BEFORE_PURCHASE").Equals("Y"); }
            catch (Exception ex)
            {
                log.Error("REGISTER_BEFORE_PURCHASE", ex);
            }
            try { ShowRegistrationTAndC = Utilities.getParafaitDefaults("SHOW_REGISTRATION_TERMS_AND_CONDITIONS").Equals("Y"); }
            catch (Exception ex)
            {
                log.Error("SHOW_REGISTRATION_TERMS_AND_CONDITIONS", ex);
            }
            try { ShowCheckInTAndC = ParafaitDefaultContainerList.GetParafaitDefault(KioskStatic.Utilities.ExecutionContext, "SHOW_CHECK-IN_TERMS_AND_CONDITIONS").Equals("Y"); }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            CustomerScreenMessage1 = Utilities.MessageUtils.getMessage(752);
            CustomerScreenMessage2 = Utilities.MessageUtils.getMessage(762, Utilities.ParafaitEnv.SiteName);

            KIOSK_CARD_VALUE_FORMAT = Utilities.getParafaitDefaults("KIOSK_CARD_VALUE_FORMAT").Trim();
            if (string.IsNullOrEmpty(KIOSK_CARD_VALUE_FORMAT))
                KIOSK_CARD_VALUE_FORMAT = Utilities.ParafaitEnv.AMOUNT_FORMAT;

            try
            {
                CreditCardReaderPortNo = Convert.ToInt32(Utilities.getParafaitDefaults("CREDIT_CARD_TERMINAL_PORT_NO"));
            }
            catch (Exception ex)
            {
                log.Error("CREDIT_CARD_TERMINAL_PORT_NO", ex);
                CreditCardReaderPortNo = 0;
            }

            SPLIT_AND_MAP_VARIABLE_PRODUCT = KioskStatic.Utilities.getParafaitDefaults("SPLIT_AND_MAP_VARIABLE_PRODUCT").Equals("Y");//added 08-Jun-2015 for Aloha update
                                                                                                                                     //gg
            ALLOW_DECIMALS_IN_VARIABLE_RECHARGE = KioskStatic.Utilities.getParafaitDefaults("ALLOW_DECIMALS_IN_VARIABLE_RECHARGE").Equals("Y");

            IS_ALOHA_ENV = KioskStatic.Utilities.getParafaitDefaults("IS_ALOHA_ENV").Equals("Y") & KioskStatic.Utilities.getParafaitDefaults("THIRD_PARTY_SYSTEM_SYNCH_URL").Trim().Equals("");
            IGNORE_THIRD_PARTY_SYNCH_ERROR = KioskStatic.Utilities.getParafaitDefaults("IGNORE_THIRD_PARTY_SYNCH_ERROR").Equals("Y");

            int loadRegProduct = -1;
            Int32.TryParse(KioskStatic.Utilities.getParafaitDefaults("LOAD_PRODUCT_ON_REGISTRATION"), out loadRegProduct);
            if (loadRegProduct != -1)
            {
                object o = KioskStatic.Utilities.executeScalar(@"select isnull(p.credits, 0) + isnull(p.bonus, 0) 
                                                                        + ISNULL((select sum(isnull(pcp.CreditPlus,0))
                                                                             from ProductCreditPlus pcp
                                                                            where pcp.product_id = p.product_id
                                                                             AND ISNULL(pcp.IsActive ,1) = 1
                                                                             AND CreditPlusType in ('A','G')),0)
                                                                   from products p where product_id = @productId", new SqlParameter("@productId", loadRegProduct));
                if (o != null)
                    RegistrationBonusAmount = Convert.ToDouble(o);
            }

            try
            {
                MAX_VARIABLE_RECHARGE_AMOUNT = Convert.ToDouble(KioskStatic.Utilities.getParafaitDefaults("MAX_VARIABLE_RECHARGE_AMOUNT"));
            }
            catch (Exception ex)
            {
                log.Error("MAX_VARIABLE_RECHARGE_AMOUNT", ex);
            }

            REVERSE_KIOSK_TOPUP_CARD_NUMBER = KioskStatic.Utilities.getParafaitDefaults("REVERSE_KIOSK_TOPUP_CARD_NUMBER").Equals("Y");

            KioskStatic.logToFile("REVERSE_DESKTOP_CARD_NUMBER: " + KioskStatic.Utilities.ParafaitEnv.REVERSE_DESKTOP_CARD_NUMBER.ToString());
            KioskStatic.logToFile("REVERSE_KIOSK_TOPUP_CARD_NUMBER: " + REVERSE_KIOSK_TOPUP_CARD_NUMBER.ToString());
            log.LogMethodExit();
        }

        public static DataTable GetProductDisplayGroups(string Function, string entitlementType, List<int> enabledDisplayGroupIdList = null)
        {
            log.LogMethodEntry(Function, entitlementType, enabledDisplayGroupIdList);
            string ProductType;
            SqlCommand Productcmd = KioskStatic.Utilities.getCommand();
            DateTime serverTime = ServerDateTime.Now;

            if (Function == "I")
            {
                if (KioskStatic.Utilities.getParafaitDefaults("ALLOW_VARIABLE_NEW_CARD_ISSUE").Equals("Y"))
                {
                    if (entitlementType == "Time")
                        ProductType = "('GAMETIME', 'VARIABLECARD')";
                    else
                        ProductType = "('NEW', 'CARDSALE', 'VARIABLECARD') ";
                }
                else
                {
                    if (entitlementType == "Time")
                        ProductType = "('GAMETIME')";
                    else
                        ProductType = "('NEW', 'CARDSALE') ";
                }
            }
            else if (Function == "C")
            {
                ProductType = "('CHECK-IN', 'COMBO') ";
            }
            else if (Function == "F")
            {
                ProductType = "('MANUAL') ";
            }
            else if (Function == "A")
            {
                ProductType = "('ATTRACTION', 'COMBO') ";
            }
            else
            {
                if (entitlementType == "Time")
                    ProductType = "('GAMETIME', 'VARIABLECARD')";
                else
                    ProductType = "('RECHARGE', 'CARDSALE', 'VARIABLECARD') ";
            }

            string displayGroupFilter = " ProductDisplayGroupFormat PDGF ";
            //enabledDisplayGroupIdList = new List<int>() { 0, 1 };
            if (enabledDisplayGroupIdList != null && enabledDisplayGroupIdList.Any())
            {
                displayGroupFilter = "( SELECT * FROM ProductDisplayGroupFormat Where id in ( ";
                for (int i = 0; i < enabledDisplayGroupIdList.Count; i++)
                {
                    displayGroupFilter = displayGroupFilter + enabledDisplayGroupIdList[i]
                                          + ((i != enabledDisplayGroupIdList.Count - 1) ? "," : "");
                }
                displayGroupFilter = displayGroupFilter + " ) ) as PDGF ";
            }

            Productcmd.CommandText = @"select distinct
                                       isnull(case pdgf.displayGroup when '' then null else pdgf.displayGroup end, 'Others') display_group , pdgf.SortOrder, pdgf.ImageFileName, pdgf.BackgroundImageFileName
                                     from products p join (select pdg.ProductId, pdgf.DisplayGroup, pdgf.SortOrder, pdgf.ImageFileName, pdgf.BackgroundImageFileName
					                     from ProductsDisplayGroup PDG, " + displayGroupFilter +
                                     @"   where pdg.DisplayGroupId = pdgf.id 
									     and not exists (select 1  
										  				    from POSProductExclusions e  
														    where e.POSMachineId = @POSMachine  
														      and e.ProductDisplayGroupFormatId = pdg.DisplayGroupId)  
													        ) pdgf on pdgf.ProductId = p.product_id, product_type pt
                                     where p.product_type_id = pt.product_type_id 
                                     and p.active_flag = 'Y'
                                     and (p.POSTypeId = @Counter or @Counter = -1 or p.POSTypeId is null)
                                     and (p.expiryDate >= @today or p.expiryDate is null)
                                     and (p.StartDate <= @today or p.StartDate is null)
                                     and not exists (select 1 -- Skip fund raiser/donation products
                                                      from lookupView lv, category ct
                                                     where lv.LookupName ='FUND_RAISER_AND_DONATIONS'
                                                       and ISNULL(lv.description,'A') = ct.Name
                                                       and ct.CategoryId = p.CategoryId)
                                     --and ISNULL(p.WaiverSetId, -1) = -1
                                     --and not exists (select 1 
                                     --                  from FacilityWaiver fw, CheckInFacility fac, WaiverSet ws,
                                     --FacilityMapDetails fmd, FacilityMap fm,
	                                 --                       ProductsAllowedInFacility paif
                                     --                 where fac.FacilityId = fw.FacilityId
                                     --                    and fac.active_flag = 'Y'
                                     --                    and ISNULL(fw.EffectiveFrom, getdate()) >= getdate()
                                     --                    and ISNULL(fw.EffectiveTo, getdate()) <= getdate()
                                     --                    and fw.IsActive = 1
                                     --                    and fw.FacilityWaiverId = ws.WaiverSetId
                                     --                    and ws.IsActive = 1
                                     --                    and fac.FacilityId = fmd.FacilityId
                                     --                    and fmd.IsActive = 1
                                     --                    and fmd.FacilityMapId = fm.FacilityMapId
                                     --                    and fm.IsActive = 1
                                     --                    and paif.FacilityMapId = fmd.FacilityMapId
                                     --                    and paif.IsActive = 1
                                     --                    and paif.ProductsId = p.product_id)
                                     --and not exists (select 1
                                                     --from POSProductExclusions e, ProductsDisplayGroup pd 
                                                     --where e.POSMachineId = @POSMachine 
                                                     --and e.ProductDisplayGroupFormatId = pd.DisplayGroupId
                                                     --and pd.ProductId = p.product_id)
                                     and not exists (select 1 
                                                       from comboproduct cp, 
                                                            products childp, 
                                                            product_type cpt
                                                      where cp.product_id = p.product_id
                                                        and cp.childProductId = childp.Product_id
                                                        and cpt.product_type_id = childp.product_type_id
                                                        and ((@functionType = 'A' and cpt.product_type in ('CHECK-IN'))
                                                            OR (@functionType = 'C' and cpt.product_type in ('ATTRACTION'))
                                                             )
                                                    )
                                     and case when pt.product_type = 'COMBO' then 
                                              (select count(1) 
                                                 from comboproduct cp,
                                                      products childp, 
                                                      product_type cpt
                                                 where cp.product_id = p.product_id
                                                  and cp.isactive = 1
                                                  and cp.childProductId = childp.Product_id
                                                  and cpt.product_type_id = childp.product_type_id
                                                  and ((@functionType = 'A' and cpt.product_type in ('ATTRACTION'))
                                                        OR (@functionType = 'C' and cpt.product_type in ('CHECK-IN'))
                                                       ))  
                                              else 1 end > 0
                                     and (not exists (select 1 
                                                     from ProductCalendar pc
                                                     where pc.product_id = p.product_id)
                                                     or exists (select 1 
                                                                from (select top 1 date, day, case when @nowHour between isnull(FromTime, @nowHour) and isnull(case ToTime when 0 then 24 else ToTime end, @nowHour) then 0 else 1 end sort, 
                                                                isnull(FromTime, @nowHour) FromTime, isnull(ToTime, @nowHour) ToTime, ShowHide -- // select in the order of specific date, day of month, weekday, every day. 
                --if there are multiple slots on same day, take the one which is in current hour
                                                                            from ProductCalendar pc
                                                         where pc.product_id = p.product_id
                                                         and (Date = @today -- // specific date
                                                         or Day = @DayNumber -- // 1001 to 1031
                                                         or Day = @weekDay -- // 0 to 6
                                                         or Day = -1) -- //everyday
                                                         order by 1 desc, 2 desc, 3) inView
                                                         where (ShowHide = 'Y' 
                                                                and (@nowHour >= FromTime and @nowHour <= case ToTime when 0 then 24 else ToTime end))
                                                            or (ShowHide = 'N' 
                                                                and (@nowHour < FromTime or @nowHour > case ToTime when 0 then 24 else ToTime end)))) 
                                      and pt.product_type in " + ProductType +
                                      @"order by SortOrder, display_group";
            Productcmd.Parameters.AddWithValue("@functionType", Function);
            Productcmd.Parameters.AddWithValue("@POSMachine", KioskStatic.Utilities.ParafaitEnv.POSMachineId);
            Productcmd.Parameters.AddWithValue("@Counter", KioskStatic.Utilities.ParafaitEnv.POSTypeId);
            Productcmd.Parameters.AddWithValue("@today", serverTime.Date);
            Productcmd.Parameters.AddWithValue("@nowHour", serverTime.Hour + serverTime.Minute / 100.0);
            Productcmd.Parameters.AddWithValue("@DayNumber", serverTime.Day + 1000); // day of month stored as 1000 + day of month

            int dayofweek = -1;
            switch (serverTime.DayOfWeek)
            {
                case DayOfWeek.Sunday: dayofweek = 0; break;
                case DayOfWeek.Monday: dayofweek = 1; break;
                case DayOfWeek.Tuesday: dayofweek = 2; break;
                case DayOfWeek.Wednesday: dayofweek = 3; break;
                case DayOfWeek.Thursday: dayofweek = 4; break;
                case DayOfWeek.Friday: dayofweek = 5; break;
                case DayOfWeek.Saturday: dayofweek = 6; break;
                default: break;
            }
            Productcmd.Parameters.AddWithValue("@weekDay", dayofweek);

            DataTable ProductTbl = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(Productcmd);
            try
            {
                da.Fill(ProductTbl);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ProductTbl = null;
            }
            log.LogMethodExit(ProductTbl);
            return ProductTbl;

        }//Ends:Modification on 17-Jan-2017 for adding display group
        public static string ReverseTopupCardNumber(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            if (REVERSE_KIOSK_TOPUP_CARD_NUMBER == false)
            {
                log.LogMethodExit(cardNumber);
                return cardNumber;
            }
            else
            {
                try
                {
                    char[] arr = cardNumber.ToCharArray();

                    for (int i = 0; i < cardNumber.Length / 2; i += 2)
                    {
                        char x = arr[i];
                        char y = arr[i + 1];

                        arr[i] = arr[cardNumber.Length - i - 2];
                        arr[i + 1] = arr[cardNumber.Length - i - 1];

                        arr[cardNumber.Length - i - 2] = x;
                        arr[cardNumber.Length - i - 1] = y;
                    }
                    string returnValue = new string(arr);
                    log.LogMethodExit(returnValue);
                    return returnValue;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.LogMethodExit(cardNumber);
                    return cardNumber;
                }
            }
        }

        public class CreditCardPaymentModeDetails
        {
            public int PaymentModeId;
            public string PaymentMode;
            public string Gateway;
            public double SurchargePercentage;
        }



        public static bool get_shutcode()
        {
            log.LogMethodEntry();
            SqlCommand sqlcmd = Utilities.getCommand();
            sqlcmd.CommandText = "select * from shutcode";
            rsshutcode = sqlcmd.ExecuteReader();
            if (rsshutcode.Read())
            {
                shut_click = rsshutcode[0].ToString();
                shut_code = rsshutcode[1].ToString();
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        public static bool check_mail(string emailAddress)
        {
            log.LogMethodEntry(emailAddress);
            bool functionReturnValue = false;
            string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$";
            Match emailAddressMatch = Regex.Match(emailAddress, pattern);
            if (emailAddressMatch.Success)
            {
                functionReturnValue = true;
            }
            else
            {
                functionReturnValue = false;
            }
            log.LogMethodExit(functionReturnValue);
            return functionReturnValue;
        }

        //public static staticDataExchange StaticDataExchange = null;

        public static DataTable getProductDetails(int productId)
        {
            log.LogMethodEntry(productId);
            DataTable dt = new DataTable();
            dt = Utilities.executeDataTable(@"select p.product_id,
                                                    isnull((select translation 
                                                             from ObjectTranslations
                                                             where ElementGuid = p.Guid
                                                               and Element = 'PRODUCT_NAME'
                                                                and LanguageId = @lang), p.product_name) as product_name, 
                                                    isnull((select translation 
                                                            from ObjectTranslations
                                                            where ElementGuid = p.Guid
                                                            and Element = 'DESCRIPTION'
                                                            and LanguageId = @lang), p.description) as description, pt.product_type,
                                                       case when TaxInclusivePrice = 'Y' then p.price else p.price * (1 + isnull(t.tax_percentage, 0)/100.0) end as Price,
                                                       p.QuantityPrompt, isnull(p.CardCount, 0) CardCount, 
                                                       isnull(Credits, 0) + isnull(bonus, 0) + 
				                                        isnull((select sum(pcp.CreditPlus) 
                                                                from productCreditPlus pcp 
                                                                where pcp.product_id = @productId 
                                                                and pcp.creditplustype in ('A', 'G','B') ), 0)
									                                        as Credits, 
                                                        isnull((select sum(pcp.CreditPlus) 
                                                            from productCreditPlus pcp 
                                                            where pcp.product_id = @productId 
                                                            and pcp.creditplustype in ('M') ), 0)
						                                        as TimePoint, 
					                                        isnull((select sum(pcp.CreditPlus) 
                                                            from productCreditPlus pcp 
                                                            where pcp.product_id = @productId 
                                                            and pcp.creditplustype in ('L') ), 0)
						                                        as LoyaltyPoints, 
				                                        isnull((select sum(pcp.CreditPlus) 
                                                            from productCreditPlus pcp 
                                                            where pcp.product_id = @productId 
                                                            and pcp.creditplustype in ('T') ), 0)
						                                        as Tickets,
                                                       p.CategoryId, p.TaxInclusivePrice,ISNULL(p.AutoGenerateCardNumber,'N') as AutoGenerateCardNumber,
                                                        isnull(t.tax_percentage, 0) taxPercentage,
                                                       p.RegisteredCustomerOnly, 
                                                     case when TaxInclusivePrice = 'Y' then 
                                                              ( (p.price ) / (1.0 + ISNULL(t.tax_percentage,0) / 100.0))* (isnull(t.tax_percentage, 0)/100.0) 
                                                         Else 
                                                               ((p.price )* (isnull(t.tax_percentage, 0)/100.0)) 
                                                          end as taxAmount,
                                                      case when TaxInclusivePrice = 'Y' and isnull(t.tax_percentage,0) > 0  then 
                                                              (p.price )/ (1.0 + isnull(t.tax_percentage,0) / 100.0)
                                                         else  
                                                              (p.price )
                                                          end as ProductPrice, -- without tax
                                                        ISNULL(p.face_value, 0)  as face_value,
                                                        pt.CardSale as CardSale,
                                                        cf.FacilityName as FacilityName,
                                                        cf.FacilityId as CheckInFacilityId
                                                from products p left outer join tax t
                                                    on p.tax_id = t.tax_id
                                                   left outer join CheckInFacility cf  on p.CheckInFacilityId = cf.FacilityId,
                                                    product_type pt
                                                where pt.product_type_id = p.product_type_id
                                                  and product_id = @productId", new SqlParameter[]
                                                                                { new SqlParameter("@productId", productId),
                                                                                   new SqlParameter("@lang", Utilities.ParafaitEnv.LanguageId)
                                                                                });
            //da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                double price = 0;
                if (dt.Rows[0]["Price"] != DBNull.Value)
                    price = Convert.ToDouble(dt.Rows[0]["Price"]);

                if (-1 != Promotions.getProductPromotionPrice(null, productId, dt.Rows[0]["CategoryId"], dt.Rows[0]["TaxInclusivePrice"].ToString(), Convert.ToDouble(dt.Rows[0]["taxPercentage"]), ref price, KioskStatic.Utilities))
                {
                    dt.Rows[0]["Price"] = price;
                }
            }
            log.LogMethodExit(dt);
            return dt;
        }

        public static string GetProductCreditPlusValidityMessage(int productId, string entitlementType)
        {
            log.LogMethodEntry(productId, entitlementType);
            string cpValidtyQry = @"select p.Product_id, 
                                       isnull((select translation 
                                                    from ObjectTranslations
                                                    where ElementGuid = p.Guid
                                                    and Element = 'PRODUCT_NAME'
                                                    and LanguageId = @lang), p.product_name) as product_name, 
                                       ProductCreditPlusId, CreditPlus, pcp.CreditPlusType, 
									   la.Attribute CreditPLusTypeName,
                                       pcp.ValidForDays,
                                       pcp.Frequency, getDate() as entitlementReferenceDate,
                                       ISNULL(periodFrom, getDate()) as periodFrom,
                                       ISNULL(PeriodTo, 
                                       CASE WHEN ValidForDays IS NOT NULL THEN
                                                 DATEADD(HOUR, 6, DATEADD(DAY, pcp.ValidForDays, DATEDIFF(D,0,GETDATE())))
                                             ELSE
                                               CASE WHEN pcp.Minutes is not null then
                                                    DATEADD(MINUTE, pcp.Minutes, getdate()) 
                                               ELSE CASE WHEN (pcp.Frequency != NULL AND (pcp.Frequency = 'D' OR pcp.Frequency = 'W' OR pcp.frequency = 'M')) THEN
                                                              DATEADD(YEAR,1, getdate()) 
                                                          WHEN (pcp.Frequency != NULL AND (pcp.Frequency = 'B' OR pcp.frequency = 'Y' OR pcp.frequency = 'A')) THEN
                                                               DATEADD(YEAR,3, getdate()) 
                                                          ELSE PeriodTo
                                                          END
                                                END
                                            END ) as periodTo	 
                                 from products p, 
                                      ProductCreditPlus pcp ,
                                      LoyaltyAttributes la
                                where p.product_id = @productId 
                                  and pcp.product_id = p.product_id 
                                  and ISNULL(pcp.CreditPlusType,'Cash') = ISNULL(la.CreditPlusType,'Cash')";
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@productId", productId);
            sqlParameters[1] = new SqlParameter("@lang", Utilities.ParafaitEnv.LanguageId);
            DataTable productCreditPlusDataTable = Utilities.executeDataTable(cpValidtyQry, sqlParameters);
            string pcpValidatiyMessage = "";
            int expiryDays = 0;
            bool hasTimeEntitlement = false;
            if (productCreditPlusDataTable.Rows.Count > 0)
            {

                foreach (DataRow pcpRow in productCreditPlusDataTable.Rows)
                {
                    if (pcpRow["periodTo"] != DBNull.Value)
                    {
                        bool withInADayMsg = ((Convert.ToDateTime(pcpRow["PeriodTo"]) - ServerDateTime.Now).TotalHours <= 30);
                        if (pcpRow["CreditPlusType"].ToString() == "M")
                        {
                            hasTimeEntitlement = true;
                            pcpValidatiyMessage = pcpValidatiyMessage + " "
                                                + Utilities.MessageUtils.getMessage((withInADayMsg ? 1720 : 1718)
                                                                                   , pcpRow["CreditPLusTypeName"].ToString()
                                                                                   , (withInADayMsg ? "" : Convert.ToDateTime(pcpRow["PeriodTo"]).ToString(Utilities.ParafaitEnv.DATETIME_FORMAT))
                                                                                   , Environment.NewLine);
                        }
                        else
                        {
                            pcpValidatiyMessage = pcpValidatiyMessage + " "
                                                + Utilities.MessageUtils.getMessage((withInADayMsg ? 1721 : 1719)
                                                                                   , pcpRow["CreditPLusTypeName"].ToString()
                                                                                   , ""
                                                                                   , (withInADayMsg ? "" : Convert.ToDateTime(pcpRow["PeriodTo"]).ToString(Utilities.ParafaitEnv.DATETIME_FORMAT))
                                                                                   , Environment.NewLine);
                        }


                    }
                }
            }
            if (entitlementType == "Time")
            {
                try
                {
                    expiryDays = Convert.ToInt32(Utilities.getParafaitDefaults("CONVERTED_TIME_ENTITLEMENT_VALID_FOR_DAYS"));
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    KioskStatic.logToFile("Unable to find valid value for expiryDays" + ex.Message);
                    expiryDays = 1;
                    KioskStatic.logToFile("expiryDays: " + expiryDays.ToString());
                }
                if (expiryDays > 0 && hasTimeEntitlement == false)
                {
                    DateTime expiryDate = (new DateTime(ServerDateTime.Now.Year, ServerDateTime.Now.Month, ServerDateTime.Now.Day).AddHours(6)).AddDays(expiryDays);
                    //dateadd(HH, 6, convert(datetime, convert(varchar, getdate(), 101), 101) + @ExpiryDays
                    bool withInADayMsg = (expiryDays <= 1);
                    pcpValidatiyMessage = pcpValidatiyMessage
                                            + Utilities.MessageUtils.getMessage((withInADayMsg ? 1720 : 1718)
                                                                               , Utilities.MessageUtils.getMessage("Time")
                                                                               , (withInADayMsg ? "" : expiryDate.ToString(Utilities.ParafaitEnv.DATETIME_FORMAT))
                                                                               , Environment.NewLine);
                }
            }

            log.LogMethodExit(pcpValidatiyMessage);
            return pcpValidatiyMessage;
        }
        public static Semnox.Parafait.KioskCore.CardAcceptor.CardAcceptor getCardAcceptor()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return new Semnox.Parafait.KioskCore.CardAcceptor.SK310UR04();
        }
        public static Semnox.Parafait.KioskCore.CardAcceptor.CardAcceptor getCardAcceptor(string serialPortNum)
        {
            return new Semnox.Parafait.KioskCore.CardAcceptor.SK310UR04(serialPortNum);
        }
        public static Semnox.Parafait.KioskCore.CardDispenser.CardDispenser getCardDispenser(string serialPortNum)
        {
            switch (CardDispenserModel)
            {
                case Semnox.Parafait.KioskCore.CardDispenser.CardDispenser.Models.K720: return (new Semnox.Parafait.KioskCore.CardDispenser.D1801(serialPortNum, KioskStatic.CardDispenserAddress));
                case Semnox.Parafait.KioskCore.CardDispenser.CardDispenser.Models.K720RF: return (new Semnox.Parafait.KioskCore.CardDispenser.K720RF(serialPortNum, KioskStatic.CardDispenserAddress));
                case Semnox.Parafait.KioskCore.CardDispenser.CardDispenser.Models.SCT0M0: return (new Semnox.Parafait.KioskCore.CardDispenser.SCT0M0(serialPortNum));
                default: return (new Semnox.Parafait.KioskCore.CardDispenser.D1801(serialPortNum, KioskStatic.CardDispenserAddress));
            }
        }
        public static Semnox.Parafait.KioskCore.BillAcceptor.BillAcceptor getBillAcceptor()
        {
            switch (BillAcceptorModel)
            {
                case Semnox.Parafait.KioskCore.BillAcceptor.BillAcceptor.Models.ICTL77: return (new Semnox.Parafait.KioskCore.BillAcceptor.ICTL77());
                case Semnox.Parafait.KioskCore.BillAcceptor.BillAcceptor.Models.NV9USB:
                //case BillAcceptor.Models.GBAST1:
                //    {
                //        foreach (BaseDevice bd in microcoinSDK.GetAllDevices())
                //        {
                //            if (bd.DeviceType == "Notereader")
                //                return (new GBAST1((NoteReader)bd));
                //        }
                //        return null;
                //    }
                default: return (new Semnox.Parafait.KioskCore.BillAcceptor.NV9USB());// return (new ICTL77());
            }
        }
        public static Semnox.Parafait.KioskCore.BillAcceptor.BillAcceptor getBillAcceptor(string serialPortNum)
        {
            switch (BillAcceptorModel)
            {
                case Semnox.Parafait.KioskCore.BillAcceptor.BillAcceptor.Models.ICTL77: return (new Semnox.Parafait.KioskCore.BillAcceptor.ICTL77(serialPortNum));
                case Semnox.Parafait.KioskCore.BillAcceptor.BillAcceptor.Models.NV9USB: return (new Semnox.Parafait.KioskCore.BillAcceptor.NV9USB(serialPortNum));
                default: return (new Semnox.Parafait.KioskCore.BillAcceptor.NV9USB(serialPortNum));
            }
        }
        public static Semnox.Parafait.KioskCore.CoinAcceptor.CoinAcceptor getCoinAcceptor(int serialPortNumber)
        {
            log.LogMethodEntry(serialPortNumber);
            log.LogVariableState("CoinAcceptorModel", CoinAcceptorModel);
            switch (CoinAcceptorModel)
            {
                //case CoinAcceptor.Models.IMP10: return (new IMP10());
                case CoinAcceptor.CoinAcceptor.Models.UCA2:
                    log.LogMethodExit("UCA2");
                    return (new Semnox.Parafait.KioskCore.CoinAcceptor.UCA2(serialPortNumber));
                //case CoinAcceptor.Models.HS636: return (new HS636());
                case CoinAcceptor.CoinAcceptor.Models.MICROCOIN_SP: return (new Semnox.Parafait.KioskCore.CoinAcceptor.MicrocoinSP());
                //    {
                //        foreach (BaseDevice bd in microcoinSDK.GetAllDevices())
                //        {
                //            if (bd.DeviceType == "Validator")
                //                return (new MicrocoinSP((CoinValidator)bd));
                //        }
                //        return null;
                //    }
                default:
                    log.LogMethodExit("default-UCA2");
                    return (new CoinAcceptor.UCA2(serialPortNumber));
            }
        }
        //public static CoinAcceptor getCoinAcceptor(string serialPortNum)
        //{
        //  return (new UCA2(serialPortNum));
        //}

        public static void logToFile(string logText)
        {

            string file = LogDirectory + Environment.MachineName + " Kiosk " + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            lock (LogDirectory)
            {
                File.AppendAllText(file, DateTime.Now.ToString("h:mm:ss tt") + ": " + logText + Environment.NewLine);
            }
        }

        static void getSiteHeading()
        {
            if (CurrentTheme.ShowSiteHeading)
                SiteHeading = KioskStatic.Utilities.MessageUtils.getMessage(415, KioskStatic.Utilities.ParafaitEnv.SiteName);
            else
                SiteHeading = "";
        }

        static void getTheme(ExecutionContext executionContext, string source)
        {
            log.LogMethodEntry(source);
            CurrentTheme = new ThemeClass();
            CurrentTheme.ThemeId = 1;
            CurrentTheme.ScreenHeadingForeColor = Color.Black;
            CurrentTheme.FieldLabelForeColor = Color.Black;
            CurrentTheme.ApplicationVersionForeColor = Color.WhiteSmoke;
            CurrentTheme.FooterMsgErrorHighlightBackColor = Color.Red;
            //CurrentTheme.HomeScreenBackgroundImage = Properties.Resources.Home_screen;
            CurrentTheme.ShowSiteHeading = true;
            CurrentTheme.ShowHeaderMessage = true;
            CurrentTheme.KeypadSizePercentage = 100;
            CurrentTheme.DefaultFont = new Font("Gotham Rounded Bold", 10);
            CurrentTheme.HomeScreenOptionsLargeFontSize = 45F;
            CurrentTheme.HomeScreenOptionsSmallFontSize = 35.25F;
            CurrentTheme.HomeScreenOptionsMediumFontSize = 26F;
            CurrentTheme.HomeScreenOptionWidth = 0;
            //CurrentTheme.FAQButton = Properties.Resources.terms_button;
            //CurrentTheme.TapCardBackgroundImage = Properties.Resources.tap_card_box;
            //CurrentTheme.ChooseEntitlementImage = Properties.Resources.credit_debit_button;
            //CurrentTheme.PauseTimeBackgroundImage = Properties.Resources.pause_time_box;
            ////CurrentTheme.TimeOutCounterBackgroundImage = Properties.Resources.pause_time_box;
            //CurrentTheme.CountdownTimer = Properties.Resources.countdown_timer;
            //CurrentTheme.TextBoxBackgroundImage = Properties.Resources.text_entry_box2;
            //CurrentTheme.NoOfCardsButtonImage = Properties.Resources.No_of_Card_Button;
            //CurrentTheme.YesNoButtonImage = Properties.Resources.btnYes_image;
            //CurrentTheme.HomeScreenClientLogo = Properties.Resources.semnox_logo;

            if (source.ToString().ToLower().Equals("tabletop"))
            {
                CurrentTheme.HomeBigButtonTextAlignment = BOTTOMCENTER;
                CurrentTheme.DirectCashButtonTextAlignment = BOTTOMCENTER;
                CurrentTheme.DisplayGroupButtonTextAlignment = MIDDLECENTER;
                CurrentTheme.DisplayProductButtonTextAlignment = MIDDLECENTER;
                CurrentTheme.RedeemTokenButtonTextAlignment = BOTTOMCENTER;
                CurrentTheme.CustOptionBtnTextAlignment = BOTTOMCENTER;
                CurrentTheme.FSKSalesTextAlignment = BOTTOMCENTER;
                CurrentTheme.BasicCardCountButtonTextAlignment = BOTTOMCENTER;
                CurrentTheme.PaymentModeButtonTextAlignment = MIDDLELEFT;
                CurrentTheme.KioskCalendarWidth = 900;
                CurrentTheme.KioskCalendarHeight = 900;
                CurrentTheme.HomeScreenOptionsLargeFontSize = 30F;
                CurrentTheme.HomeScreenOptionsSmallFontSize = 22F;
            }
            else
            {
                CurrentTheme.HomeBigButtonTextAlignment = MIDDLELEFT;
                CurrentTheme.DirectCashButtonTextAlignment = MIDDLELEFT;
                CurrentTheme.PaymentModeButtonTextAlignment = TOPCENTER;
                CurrentTheme.DisplayGroupButtonTextAlignment = MIDDLELEFT;
                CurrentTheme.DisplayProductButtonTextAlignment = MIDDLELEFT;
                CurrentTheme.RedeemTokenButtonTextAlignment = MIDDLELEFT;
                CurrentTheme.CustOptionBtnTextAlignment = MIDDLELEFT;
                CurrentTheme.FSKSalesTextAlignment = MIDDLELEFT;
                CurrentTheme.BasicCardCountButtonTextAlignment = MIDDLELEFT;
            }
            CurrentTheme.HomeMediumButtonTextAlignment = BOTTOMCENTER;
            CurrentTheme.HomeSmallButtonTextAlignment = BOTTOMCENTER;
            CurrentTheme.PreSelectPaymentModeButtonTextAlignment = TOPCENTER;
            CurrentTheme.DisplayProductsGreetingOption = OPT1;

            string configFile = System.Windows.Forms.Application.StartupPath + @"\Parafait Kiosk Configuration.txt";
            logToFile("Kiosk Config file: " + configFile);
            if (File.Exists(configFile))
            {
                try
                {
                    CurrentTheme.ThemeId = Convert.ToInt32(getConfigValue("UIThemeNo"));
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    CurrentTheme.ThemeId = (source.ToString().ToLower().Equals("tabletop") ? 6 : 1);
                }

                string[] lines = File.ReadAllLines(configFile);
                string theme = "[theme" + CurrentTheme.ThemeId.ToString() + "]";
                string themeWithoutBrackets = "Theme" + CurrentTheme.ThemeId.ToString();
                logToFile("Kiosk Theme: " + theme);
                string themeFolderFullPath = Application.StartupPath + @"\Media\Images\" + themeWithoutBrackets;
                string themeFolderPath = string.Empty;
                if (Directory.Exists(themeFolderFullPath))
                {
                    logToFile("Kiosk Theme folder " + themeFolderFullPath + " is found");
                    themeFolderPath = themeFolderFullPath;
                }
                else
                {
                    string backupFolder = Application.StartupPath + @"\Media\Images\";
                    logToFile("Separate Kiosk Theme folder " + themeFolderFullPath + " is not defined. Kiosk will try use images available under root folder: " + backupFolder);
                }
                SetDefaultForeColors(executionContext, lines, theme, source);
                bool themeEntryFound = false;
                bool defaultbackgroundimageIsProvided = false;
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (line.Replace(" ", "").ToLower().Equals(theme))
                    {
                        themeEntryFound = true;
                        for (int j = i + 1; j < lines.Length; j++)
                        {
                            string themeLine = lines[j];
                            if (themeLine.Replace(" ", "").ToLower().Contains("[theme"))
                                break;

                            if (themeLine.Trim() == "")
                                continue;

                            logToFile(themeLine);
                            string[] nameValue = themeLine.Split('=');
                            switch (nameValue[0].Trim().ToLower())
                            {
                                case "siteheadingforecolor":
                                    if (nameValue.Length > 1)
                                    {
                                        Color color = Color.FromName(nameValue[1].Trim());
                                        if (color.ToArgb() == 0)
                                            color = Color.FromArgb(Int32.Parse(nameValue[1].Trim(), System.Globalization.NumberStyles.HexNumber));

                                        if (color.ToArgb() != 0)
                                            CurrentTheme.SiteHeadingForeColor = color;
                                    }
                                    break;
                                case "applicationversionforecolor":
                                    if (nameValue.Length > 1)
                                    {
                                        Color color = Color.FromName(nameValue[1].Trim());
                                        if (color.ToArgb() == 0)
                                            color = Color.FromArgb(Int32.Parse(nameValue[1].Trim(), System.Globalization.NumberStyles.HexNumber));

                                        if (color.ToArgb() != 0)
                                            CurrentTheme.ApplicationVersionForeColor = color;
                                    }
                                    break;
                                case "screenheadingforecolor":
                                    if (nameValue.Length > 1)
                                    {
                                        Color color = Color.FromName(nameValue[1].Trim());
                                        if (color.ToArgb() == 0)
                                            color = Color.FromArgb(Int32.Parse(nameValue[1].Trim(), System.Globalization.NumberStyles.HexNumber));

                                        if (color.ToArgb() != 0)
                                            CurrentTheme.ScreenHeadingForeColor = color;
                                    }
                                    break;
                                case "fieldlabelforecolor":
                                    if (nameValue.Length > 1)
                                    {
                                        Color color = Color.FromName(nameValue[1].Trim());
                                        if (color.ToArgb() == 0)
                                            color = Color.FromArgb(Int32.Parse(nameValue[1].Trim(), System.Globalization.NumberStyles.HexNumber));

                                        if (color.ToArgb() != 0)
                                            CurrentTheme.FieldLabelForeColor = color;
                                    }
                                    break;
                                case "footermsgerrorhighlightbackColor":
                                    if (nameValue.Length > 1)
                                    {
                                        Color color = Color.FromName(nameValue[1].Trim());
                                        if (color.ToArgb() == 0)
                                            color = Color.FromArgb(Int32.Parse(nameValue[1].Trim(), System.Globalization.NumberStyles.HexNumber));

                                        if (color.ToArgb() != 0)
                                            CurrentTheme.FooterMsgErrorHighlightBackColor = color;
                                    }
                                    break;
                                case "showheadermessage":
                                    if (nameValue.Length > 1)
                                    {
                                        try
                                        {
                                            CurrentTheme.ShowHeaderMessage = nameValue[1].Trim().ToLower().Equals("true");
                                        }
                                        catch (Exception ex)
                                        {
                                            log.Error(ex);
                                        }
                                    }
                                    break;
                                case "showsiteheading":
                                    if (nameValue.Length > 1)
                                    {
                                        try
                                        {
                                            CurrentTheme.ShowSiteHeading = nameValue[1].Trim().ToLower().Equals("true");
                                        }
                                        catch (Exception ex)
                                        {
                                            log.Error(ex);
                                        }
                                    }
                                    break;
                                case "keypadsizepercentage":
                                    if (nameValue.Length > 1)
                                    {
                                        try
                                        {
                                            CurrentTheme.KeypadSizePercentage = Convert.ToInt32(nameValue[1].Trim());
                                        }
                                        catch (Exception ex)
                                        {
                                            log.Error(ex);
                                        }
                                    }
                                    break;
                                ////All below 13 cases are added to introduce font feature in portrait mode. these are set from Congiguration.txt file.
                                //case "homescreenbackgroundimage":
                                //    if (nameValue.Length > 1)
                                //    {
                                //        try
                                //        {
                                //            CurrentTheme.HomeScreenBackgroundImage = GetImage(CurrentTheme.HomeScreenBackgroundImage, nameValue[1].Trim(), themeFolderPath);
                                //        }
                                //        catch (Exception ex)
                                //        {
                                //            log.Error(ex);
                                //            logToFile(ex.Message);
                                //        }
                                //    }
                                //    break;
                                //case "defaultbackgroundimage":
                                //    if (nameValue.Length > 1)
                                //    {
                                //        defaultbackgroundimageIsProvided = true;
                                //        try
                                //        {
                                //             CurrentTheme.DefaultBackgroundImage = GetImage(CurrentTheme.DefaultBackgroundImage, nameValue[1].Trim(), themeFolderPath);
                                //        }
                                //        catch (Exception ex)
                                //        {
                                //            log.Error(ex);
                                //            logToFile(ex.Message);
                                //        }

                                //        CurrentTheme.SelectAdultBackgroundImage =
                                //        CurrentTheme.SelectChildBackgroundImage =
                                //        CurrentTheme.EnterAdultBackgroundImage =
                                //        CurrentTheme.EnterChildBackgroundImage =
                                //        CurrentTheme.TransactionViewBackgroundImage =
                                //        CurrentTheme.SelectWaiverOptionsBackgroundImage =
                                //        CurrentTheme.AgeGateBackgroundImage =
                                //        CurrentTheme.BalanceBackgroundImage =
                                //        CurrentTheme.CardCountBackgroundImage =
                                //        CurrentTheme.PaymentModeBackgroundImage =
                                //        CurrentTheme.ProductBackgroundImage =
                                //        CurrentTheme.RedeemBackgroundImage =
                                //        CurrentTheme.RegistrationBackgroundImage =
                                //        CurrentTheme.TransactionBackgroundImage =
                                //        CurrentTheme.TransferBackgroundImage =
                                //        CurrentTheme.FAQBackgroundImage =
                                //        CurrentTheme.SuccessBackgroundImage =
                                //        CurrentTheme.ActivityGamesBackGroundImage =
                                //        CurrentTheme.UpsellBackgroundImage =
                                //        CurrentTheme.FundRaiserBackgroundImage =
                                //        CurrentTheme.CartScreenBackgroundImage =
                                //        CurrentTheme.EmailDetailsBackgroundImage =
                                //        CurrentTheme.PurchaseMenuBackgroundImage = CurrentTheme.DefaultBackgroundImage;

                                //        CurrentTheme.AgeGateBackgroundImage = GetImage(CurrentTheme.AgeGateBackgroundImage, "AgeGateBackground", themeFolderPath);
                                //        CurrentTheme.BalanceBackgroundImage = GetImage(CurrentTheme.BalanceBackgroundImage, "BalanceBackground", themeFolderPath);
                                //        CurrentTheme.BalanceAnimationImage = GetImage(CurrentTheme.BalanceAnimationImage, "BalanceAnimation", themeFolderPath);
                                //        CurrentTheme.BalanceButtonImage = GetImage(CurrentTheme.BalanceButtonImage, "BalanceButtonImage", themeFolderPath);
                                //        CurrentTheme.CardCountBackgroundImage = GetImage(CurrentTheme.CardCountBackgroundImage, "CardCountBackground", themeFolderPath);
                                //        CurrentTheme.PaymentModeBackgroundImage = GetImage(CurrentTheme.PaymentModeBackgroundImage, "PaymentModeBackground", themeFolderPath);
                                //        CurrentTheme.ProductBackgroundImage = GetImage(CurrentTheme.ProductBackgroundImage, "ProductBackground", themeFolderPath);
                                //        CurrentTheme.RedeemBackgroundImage = GetImage(CurrentTheme.RedeemBackgroundImage, "RedeemBackground", themeFolderPath);
                                //        CurrentTheme.RegistrationBackgroundImage = GetImage(CurrentTheme.RegistrationBackgroundImage, "RegistrationBackground", themeFolderPath);
                                //        CurrentTheme.TransactionBackgroundImage = GetImage(CurrentTheme.TransactionBackgroundImage, "TransactionBackground", themeFolderPath);
                                //        CurrentTheme.TransferBackgroundImage = GetImage(CurrentTheme.TransferBackgroundImage, "TransferBackground", themeFolderPath);
                                //        CurrentTheme.UpsellBackgroundImage = GetImage(CurrentTheme.UpsellBackgroundImage, "UpsellBackground", themeFolderPath);
                                //        CurrentTheme.CartScreenBackgroundImage = GetImage(CurrentTheme.CartScreenBackgroundImage, "CartScreenBackground", themeFolderPath);
                                //        CurrentTheme.EmailDetailsBackgroundImage = GetImage(CurrentTheme.EmailDetailsBackgroundImage, "EmailDetailsBackground", themeFolderPath);
                                //        CurrentTheme.PurchaseMenuBackgroundImage = GetImage(CurrentTheme.PurchaseMenuBackgroundImage, "PurchaseMenuBackground", themeFolderPath);
                                //        CurrentTheme.KioskCartIcon = GetImage(CurrentTheme.KioskCartIcon, "KioskCartIcon", themeFolderPath);
                                //        //CurrentTheme.KioskCartQtyLableBackGround = GetImage(CurrentTheme.KioskCartQtyLableBackGround, "KioskCartQtyLableBackGround", themeFolderPath); 
                                //        CurrentTheme.InsertCashAnimation = GetImage(CurrentTheme.InsertCashAnimation, "InsertCashAnimation", themeFolderPath);
                                //        CurrentTheme.BackButtonImage = GetImage(CurrentTheme.BackButtonImage, "Back_button_box", themeFolderPath);
                                //        CurrentTheme.KioskActivityTableImage = GetImage(CurrentTheme.KioskActivityTableImage, "KioskActivityTable", themeFolderPath);
                                //        CurrentTheme.ReceiptModeBackgroundImage = GetImage(CurrentTheme.ReceiptModeBackgroundImage, "ReceiptModeBackgroundImage", themeFolderPath);
                                //        CurrentTheme.ReceiptModeBtnBackgroundImage = GetImage(CurrentTheme.ReceiptModeBtnBackgroundImage, "ReceiptModeBtnBackgroundImage", themeFolderPath);
                                //        CurrentTheme.CartItemBackgroundImageSmall = GetImage(CurrentTheme.CartItemBackgroundImageSmall, "CartItemBackgroundImageSmall", themeFolderPath);
                                //        CurrentTheme.CartItemBackgroundImage = GetImage(CurrentTheme.CartItemBackgroundImage, "CartItemBackgroundImage", themeFolderPath);
                                //        CurrentTheme.CartDeleteButtonBackgroundImage = GetImage(CurrentTheme.CartDeleteButtonBackgroundImage, "CartDeleteButtonBackgroundImage", themeFolderPath);
                                //        CurrentTheme.WaiverSigningInstructions = GetImage(CurrentTheme.WaiverSigningInstructions, "WaiverSigningInstructions", themeFolderPath);
                                //        CurrentTheme.TablePurchaseSummary = GetImage(CurrentTheme.TablePurchaseSummary, "TablePurchaseSummary", themeFolderPath);
                                //        CurrentTheme.LoyaltyFormLogoImage = GetImage(CurrentTheme.LoyaltyFormLogoImage, "LoyaltyFormLogo", themeFolderPath);
                                //        CurrentTheme.FAQButton = GetImage(CurrentTheme.FAQButton, "FAQButton", themeFolderPath);
                                //        CurrentTheme.LanguageButton = GetImage(CurrentTheme.LanguageButton, "LanguageButton", themeFolderPath);
                                //        CurrentTheme.NewPlayPassButtonSmall = GetImage(CurrentTheme.NewPlayPassButtonSmall, "NewPlayPassButtonSmall", themeFolderPath);
                                //        CurrentTheme.NewPlayPassButtonBig = GetImage(CurrentTheme.NewPlayPassButtonBig, "NewPlayPassButtonBig", themeFolderPath);
                                //        CurrentTheme.PurchaseButtonSmall = GetImage(CurrentTheme.PurchaseButtonSmall, "PurchaseButtonSmall", themeFolderPath);
                                //        CurrentTheme.PurchaseButtonBig = GetImage(CurrentTheme.PurchaseButtonBig, "PurchaseButtonBig", themeFolderPath);
                                //        CurrentTheme.SignWaiverButtonBig = GetImage(CurrentTheme.SignWaiverButtonBig, "Sign_Waiver_Button_Big", themeFolderPath);
                                //        CurrentTheme.SignWaiverButtonSmall = GetImage(CurrentTheme.SignWaiverButtonSmall, "Sign_Waiver_Button_Small", themeFolderPath);
                                //        CurrentTheme.FoodAndBeverageBig = GetImage(CurrentTheme.FoodAndBeverageBig, "FoodAndBeverageBig", themeFolderPath);
                                //        CurrentTheme.FoodAndBeverageSmall = GetImage(CurrentTheme.FoodAndBeverageSmall, "FoodAndBeverageSmall", themeFolderPath);
                                //        CurrentTheme.PlaygroundEntryBig = GetImage(CurrentTheme.PlaygroundEntryBig, "Playground_Entry_Big", themeFolderPath);
                                //        CurrentTheme.PlaygroundEntrySmall = GetImage(CurrentTheme.PlaygroundEntrySmall, "Playground_Entry_Small", themeFolderPath);
                                //        CurrentTheme.BottomMessageLineImage = GetImage(CurrentTheme.BottomMessageLineImage, "bottom_bar", themeFolderPath);
                                //        CurrentTheme.ScrollUpEnabled = GetImage(CurrentTheme.ScrollUpEnabled, "Scroll_Up_Button", themeFolderPath);
                                //        CurrentTheme.ScrollUpDisabled = GetImage(CurrentTheme.ScrollUpDisabled, "Scroll_Up_Button_Disabled", themeFolderPath);
                                //        CurrentTheme.ScrollDownEnabled = GetImage(CurrentTheme.ScrollDownEnabled, "Scroll_Down_Button", themeFolderPath);
                                //        CurrentTheme.ScrollDownDisabled = GetImage(CurrentTheme.ScrollDownDisabled, "Scroll_Down_Button_Disabled", themeFolderPath);
                                //        CurrentTheme.ScrollLeftEnabled = GetImage(CurrentTheme.ScrollLeftEnabled, "Scroll_Left_Button", themeFolderPath);
                                //        CurrentTheme.ScrollLeftDisabled = GetImage(CurrentTheme.ScrollLeftDisabled, "Scroll_Left_Button_Disabled", themeFolderPath);
                                //        CurrentTheme.ScrollRightEnabled = GetImage(CurrentTheme.ScrollRightEnabled, "Scroll_Right_Button", themeFolderPath);
                                //        CurrentTheme.ScrollRightDisabled = GetImage(CurrentTheme.ScrollRightDisabled, "Scroll_Right_Button_Disabled", themeFolderPath);
                                //        CurrentTheme.PreSelectPaymentBackground = GetImage(CurrentTheme.PreSelectPaymentBackground, "PreSelectPaymentBackground", themeFolderPath);
                                //        CurrentTheme.RechargePlayPassButtonBig = GetImage(CurrentTheme.RechargePlayPassButtonBig, "RechargePlayPassButtonBig", themeFolderPath);
                                //        CurrentTheme.RechargePlayPassButtonSmall = GetImage(CurrentTheme.RechargePlayPassButtonSmall, "RechargePlayPassButtonSmall", themeFolderPath);
                                //        CurrentTheme.TransferPointButtonSmall = GetImage(CurrentTheme.TransferPointButtonSmall, "TransferPointButtonSmall", themeFolderPath);
                                //        CurrentTheme.TransferPointButtonBig = GetImage(CurrentTheme.TransferPointButtonBig, "TransferPointButtonBig", themeFolderPath);
                                //        CurrentTheme.RegisterPassSmall = GetImage(CurrentTheme.RegisterPassSmall, "RegisterPassButtonSmall", themeFolderPath);
                                //        CurrentTheme.RegisterPassBig = GetImage(CurrentTheme.RegisterPassBig, "RegisterPassButtonBig", themeFolderPath);
                                //        CurrentTheme.CheckBalanceButtonSmall = GetImage(CurrentTheme.CheckBalanceButtonSmall, "CheckBalanceButtonSmall", themeFolderPath);
                                //        CurrentTheme.CheckBalanceButtonBig = GetImage(CurrentTheme.CheckBalanceButtonBig, "CheckBalanceButtonBig", themeFolderPath);
                                //        CurrentTheme.ExchangeTokensButtonSmall = GetImage(CurrentTheme.ExchangeTokensButtonSmall, "ExchangeTokensButtonSmall", themeFolderPath);
                                //        CurrentTheme.ExchangeTokensButtonBig = GetImage(CurrentTheme.ExchangeTokensButtonBig, "ExchangeTokensButtonBig", themeFolderPath);
                                //        CurrentTheme.PauseCardButtonSmall = GetImage(CurrentTheme.PauseCardButtonSmall, "PauseCardButtonSmall", themeFolderPath);
                                //        CurrentTheme.PauseCardButtonBig = GetImage(CurrentTheme.PauseCardButtonBig, "PauseCardButtonBig", themeFolderPath);
                                //        CurrentTheme.PointsToTimeButtonSmall = GetImage(CurrentTheme.PointsToTimeButtonSmall, "PointsToTimeButtonSmall", themeFolderPath);
                                //        CurrentTheme.PointsToTimeButtonBig = GetImage(CurrentTheme.PointsToTimeButtonBig, "PointsToTimeButtonBig", themeFolderPath);
                                //        CurrentTheme.DisplayGroupButton = GetImage(CurrentTheme.DisplayGroupButton, "ChooseDisplayGroupButton", themeFolderPath);
                                //        CurrentTheme.ChooseProductButton = GetImage(CurrentTheme.ChooseProductButton, "ChooseProductButton", themeFolderPath);
                                //        CurrentTheme.CashButtonBig = GetImage(CurrentTheme.CashButtonBig, "CashButtonBig", themeFolderPath);
                                //        CurrentTheme.CashButtonSmall = GetImage(CurrentTheme.CashButtonSmall, "CashButtonSmall", themeFolderPath);
                                //        CurrentTheme.CreditCardButtonBig = GetImage(CurrentTheme.CreditCardButtonBig, "CreditCardButtonBig", themeFolderPath);
                                //        CurrentTheme.CreditCardButtonSmall = GetImage(CurrentTheme.CreditCardButtonSmall, "CreditCardButtonSmall", themeFolderPath);
                                //        CurrentTheme.DebitCardButton = GetImage(CurrentTheme.DebitCardButton, "DebitCardButton", themeFolderPath);
                                //        CurrentTheme.DiscountButton = GetImage(CurrentTheme.DiscountButton, "DiscountButton", themeFolderPath);
                                //        CurrentTheme.TapCardBackgroundImage = GetImage(CurrentTheme.TapCardBackgroundImage, "TapCardBackGround", themeFolderPath);
                                //        CurrentTheme.ChooseEntitlementImage = GetImage(CurrentTheme.ChooseEntitlementImage, "ChooseEntitlementBackGround", themeFolderPath);
                                //        CurrentTheme.FAQBackgroundImage = GetImage(CurrentTheme.FAQBackgroundImage, "FAQBackGround", themeFolderPath);
                                //        CurrentTheme.SuccessBackgroundImage = GetImage(CurrentTheme.SuccessBackgroundImage, "SuccessBackGround", themeFolderPath);
                                //        CurrentTheme.PauseTimeBackgroundImage = GetImage(CurrentTheme.PauseTimeBackgroundImage, "PauseTimeBackGround", themeFolderPath);
                                //        CurrentTheme.TextBoxBackgroundImage = GetImage(CurrentTheme.TextBoxBackgroundImage, "TextBoxBackGround", themeFolderPath);
                                //        CurrentTheme.NoOfCardsButtonImage = GetImage(CurrentTheme.NoOfCardsButtonImage, "CardCountButtonBackGround", themeFolderPath);
                                //        CurrentTheme.KioskPaymentScreen = GetImage(CurrentTheme.KioskPaymentScreen, "PaymentScreenImage", themeFolderPath);
                                //        CurrentTheme.AdminBackgroundImage = GetImage(CurrentTheme.AdminBackgroundImage, "AdminBackGround", themeFolderPath);
                                //        //CurrentTheme.TimeOutCounterBackgroundImage = GetImage(CurrentTheme.TimeOutCounterBackgroundImage, "TimeOutCounterBackGround");
                                //        CurrentTheme.CountdownTimer = GetImage(CurrentTheme.CountdownTimer, "countdown_timer", themeFolderPath);
                                //        CurrentTheme.LoyaltyBackGroundImage = GetImage(CurrentTheme.LoyaltyBackGroundImage, "LoyaltyBackGroundImage", themeFolderPath);
                                //        CurrentTheme.YesNoButtonImage = GetImage(CurrentTheme.YesNoButtonImage, "YesNoButtonImage", themeFolderPath);
                                //        CurrentTheme.ActivityGamesBackGroundImage = GetImage(CurrentTheme.ActivityGamesBackGroundImage, "ActivityGamesBackGround", themeFolderPath);
                                //        CurrentTheme.MoneyScreenTimerButtonImage = GetImage(CurrentTheme.MoneyScreenTimerButtonImage, "MoneyScreenTimerButtonImage", themeFolderPath);
                                //        CurrentTheme.FundRaiserBackgroundImage = GetImage(CurrentTheme.FundRaiserBackgroundImage, "FundsRaiserBackgroundImage", themeFolderPath);
                                //        CurrentTheme.FundRaiserPictureBoxLogo = GetImage(CurrentTheme.FundRaiserPictureBoxLogo, "FundRaiserPictureBoxLogo", themeFolderPath);
                                //        CurrentTheme.DonationsPictureBoxLogo = GetImage(CurrentTheme.DonationsPictureBoxLogo, "DonationsPictureBoxLogo", themeFolderPath);
                                //        CurrentTheme.HomeScreenClientLogo = GetImage(CurrentTheme.HomeScreenClientLogo, "HomeScreenClientLogo", themeFolderPath);
                                //        CurrentTheme.GetTransactionDetailsButton = GetImage(CurrentTheme.GetTransactionDetailsButton, "Search_Button", themeFolderPath);
                                //        CurrentTheme.ExecuteOnlineTrxButtonBig = GetImage(CurrentTheme.ExecuteOnlineTrxButtonBig, "Execute_Online_Trx_Button", themeFolderPath);  
                                //        CurrentTheme.ExecuteOnlineTrxButtonSmall = GetImage(CurrentTheme.ExecuteOnlineTrxButtonSmall, "ExecuteOnlineTrxButtonSmall", themeFolderPath);  
                                //        //CurrentTheme.FSKSalesButton = GetImage(CurrentTheme.FSKSalesButton, "FSK_Sales_Button", themeFolderPath);
                                //        CurrentTheme.SucessFnBImage = GetImage(CurrentTheme.SucessFnBImage, "SucessFnBImage", themeFolderPath);
                                //        CurrentTheme.SucessCartImage = GetImage(CurrentTheme.SucessCartImage, "SucessCartImage", themeFolderPath);
                                //        CurrentTheme.TapCardBox = GetImage(CurrentTheme.TapCardBox, "TapCardBox", themeFolderPath);
                                //        CurrentTheme.ScanCouponBox = GetImage(CurrentTheme.ScanCouponBox, "ScanCouponBox", themeFolderPath);
                                //        CurrentTheme.YesNoBox = GetImage(CurrentTheme.YesNoBox, "YesNoBox", themeFolderPath);
                                //        CurrentTheme.OkMsgBox = GetImage(CurrentTheme.OkMsgBox, "OkMsgBox", themeFolderPath);
                                //        CurrentTheme.TicketTypeBox = GetImage(CurrentTheme.TicketTypeBox, "TicketTypeBox", themeFolderPath);
                                //        CurrentTheme.EntitlementBox = GetImage(CurrentTheme.EntitlementBox, "EntitlementBox", themeFolderPath);
                                //        CurrentTheme.ScanCouponButtons = GetImage(CurrentTheme.ScanCouponButtons, "ScanCouponButtons", themeFolderPath);
                                //        CurrentTheme.TapCardButtons = GetImage(CurrentTheme.TapCardButtons, "TapCardButtons", themeFolderPath);
                                //        CurrentTheme.OkMsgButtons = GetImage(CurrentTheme.OkMsgButtons, "OkMsgButtons", themeFolderPath);
                                //        CurrentTheme.YesNoButtons = GetImage(CurrentTheme.YesNoButtons, "YesNoButtons", themeFolderPath);
                                //        CurrentTheme.PauseTimeButtons = GetImage(CurrentTheme.PauseTimeButtons, "PauseTimeButtons", themeFolderPath);
                                //        CurrentTheme.SelectAdultBackgroundImage = GetImage(CurrentTheme.SelectAdultBackgroundImage, "SelectAdultBackground", themeFolderPath);
                                //        CurrentTheme.SelectChildBackgroundImage = GetImage(CurrentTheme.SelectChildBackgroundImage, "SelectChildBackground", themeFolderPath);
                                //        CurrentTheme.EnterAdultBackgroundImage = GetImage(CurrentTheme.EnterAdultBackgroundImage, "EnterAdultBackground", themeFolderPath);
                                //        CurrentTheme.EnterChildBackgroundImage = GetImage(CurrentTheme.EnterChildBackgroundImage, "EnterChildBackground", themeFolderPath);
                                //        CurrentTheme.SelectWaiverOptionsBackgroundImage = GetImage(CurrentTheme.SelectWaiverOptionsBackgroundImage, "SelectWaiverOptionsBackground", themeFolderPath);
                                //        CurrentTheme.CustomerSignatureConfirmationBox = GetImage(CurrentTheme.CustomerSignatureConfirmationBox, "CustomerSignatureConfirmationBox", themeFolderPath);
                                //        CurrentTheme.CustomerSignatureConfirmationButtons = GetImage(CurrentTheme.CustomerSignatureConfirmationButtons, "CustomerSignatureConfirmationButtons", themeFolderPath);
                                //        CurrentTheme.TransactionViewBackgroundImage = GetImage(CurrentTheme.TransactionViewBackgroundImage, "TransactionViewBackground", themeFolderPath);
                                //        CurrentTheme.WaiverButtonBorderImage = GetImage(CurrentTheme.WaiverButtonBorderImage, "WaiverButtonBorderImage", themeFolderPath);
                                //        CurrentTheme.CalenderBackgroundImage = GetImage(CurrentTheme.CalenderBackgroundImage, "CalenderBackgroundImage", themeFolderPath);
                                //        CurrentTheme.CalenderBtnBackgroundImage = GetImage(CurrentTheme.CalenderBtnBackgroundImage, "CalenderBtnBackgroundImage", themeFolderPath);
                                //    }

                                //    break;
                                //case "splashscreenimage"://playpas1:starts
                                //    if (nameValue.Length > 1)
                                //    {
                                //        try
                                //        {
                                //            string splashImageName = nameValue[1].Trim();
                                //            CurrentTheme.SplashScreenImage = GetImage(CurrentTheme.CustomerSignatureConfirmationButtons, splashImageName, themeFolderPath);

                                //        }
                                //        catch { CurrentTheme.SplashScreenImage = null; }
                                //    }
                                //    break;//playpas1:Ends
                                case "splashafterseconds"://playpas1:starts
                                    if (nameValue.Length > 1)
                                    {
                                        try
                                        {
                                            CurrentTheme.SplashAfterSeconds = int.Parse(nameValue[1].Trim());
                                        }
                                        catch { CurrentTheme.SplashAfterSeconds = 20; }
                                    }
                                    break;
                                case "defaultfontname": //playpass:Starts
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.DefaultFont = CustomFontConverter.ConvertStringToFont(Utilities.ExecutionContext, nameValue[1].Trim());
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;//playpass:Ends
                                case "homescreenoptionslargefontsize":
                                    float tempLargeSize = 45F;
                                    if (source.ToString().ToLower().Equals("tabletop"))
                                    {
                                        tempLargeSize = 30F;
                                    }
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        if (float.TryParse(nameValue[1].Trim(), out CurrentTheme.HomeScreenOptionsLargeFontSize) == false)
                                        {
                                            CurrentTheme.HomeScreenOptionsLargeFontSize = tempLargeSize;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                        CurrentTheme.HomeScreenOptionsLargeFontSize = tempLargeSize;
                                    }
                                    break;
                                case "homescreenoptionsmediumfontsize":
                                    float tempMediumSize = 26F;
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        if (float.TryParse(nameValue[1].Trim(), out CurrentTheme.HomeScreenOptionsMediumFontSize) == false)
                                        {
                                            CurrentTheme.HomeScreenOptionsMediumFontSize = tempMediumSize;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                        CurrentTheme.HomeScreenOptionsMediumFontSize = tempMediumSize;
                                    }
                                    break;
                                case "homescreenoptionssmallfontsize":
                                    float tempSmallSize = 35.25F;
                                    if (source.ToString().ToLower().Equals("tabletop"))
                                    {
                                        tempSmallSize = 22F;
                                    }
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        if (float.TryParse(nameValue[1].Trim(), out CurrentTheme.HomeScreenOptionsSmallFontSize) == false)
                                        {
                                            CurrentTheme.HomeScreenOptionsSmallFontSize = tempSmallSize;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                        CurrentTheme.HomeScreenOptionsSmallFontSize = tempSmallSize;
                                    }
                                    break;
                                case "homescreenoptionwidth":
                                    if (nameValue.Length > 1)
                                    {
                                        try
                                        {
                                            if (int.TryParse(nameValue[1].Trim(), out CurrentTheme.HomeScreenOptionWidth) == false)
                                            {
                                                CurrentTheme.HomeScreenOptionWidth = 0;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            log.Error(ex);
                                            CurrentTheme.HomeScreenOptionWidth = 0;
                                        }
                                    }
                                    break;
                                case "homepagemainbuttonfont": //playpass:Starts
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.HomePageMainButtonFont = CustomFontConverter.ConvertStringToFont(Utilities.ExecutionContext, nameValue[1].Trim());

                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;//playpass:Ends
                                case "homepageotherbuttonfont": //playpass:Starts
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.HomePageOtherButtonFont = CustomFontConverter.ConvertStringToFont(Utilities.ExecutionContext, nameValue[1].Trim());
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;//playpass:Ends         
                                case "homepagebottomtextfont": //playpass:Starts
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.HomePageBottomTextFont = CustomFontConverter.ConvertStringToFont(Utilities.ExecutionContext, nameValue[1].Trim());
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;//playpass:Ends   
                                case "productbuttonfont": //playpass:Starts
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.ProductButtonFont = CustomFontConverter.ConvertStringToFont(Utilities.ExecutionContext, nameValue[1].Trim());
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;//playpass:Ends 
                                case "firstlablefont": //playpass:Starts
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.FirstLableFont = CustomFontConverter.ConvertStringToFont(Utilities.ExecutionContext, nameValue[1].Trim());
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;//playpass:Ends  
                                case "secondlablefont": //playpass:Starts
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.SecondLableFont = CustomFontConverter.ConvertStringToFont(Utilities.ExecutionContext, nameValue[1].Trim());

                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;//playpass:Ends 
                                case "thirdlablefont": //playpass:Starts
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.ThirdLableFont = CustomFontConverter.ConvertStringToFont(Utilities.ExecutionContext, nameValue[1].Trim());
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;//playpass:Ends 
                                case "fourthlablefont": //playpass:Starts
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.FourthLableFont = CustomFontConverter.ConvertStringToFont(Utilities.ExecutionContext, nameValue[1].Trim());
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;//playpass:Ends 

                                case "creditcardbuttonfont": //playpass:Starts
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.CreditCardButtonFont = CustomFontConverter.ConvertStringToFont(Utilities.ExecutionContext, nameValue[1].Trim());
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;//playpass:Ends 
                                case "activitybuttonfont": //playpass:Starts
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.ActivityButtonFont = CustomFontConverter.ConvertStringToFont(Utilities.ExecutionContext, nameValue[1].Trim());
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;//playpass:Ends 
                                case "buttonfont": //playpass:Starts
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.ButtonFont = CustomFontConverter.ConvertStringToFont(Utilities.ExecutionContext, nameValue[1].Trim());
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;//playpass:Ends 
                                case "griddatafont": //playpass:Starts
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.GridDataFont = CustomFontConverter.ConvertStringToFont(Utilities.ExecutionContext, nameValue[1].Trim());
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;//playpass:Ends 
                                case "textboxfont": //playpass:Starts
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.TextBoxFont = CustomFontConverter.ConvertStringToFont(Utilities.ExecutionContext, nameValue[1].Trim());
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;//playpass:Ends 
                                case "textbackgroundcolor":
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        if (nameValue[1].Contains("#"))
                                        {
                                            CurrentTheme.TextBackGroundColor = System.Drawing.ColorTranslator.FromHtml(nameValue[1].Trim());
                                        }
                                        else
                                        {
                                            CurrentTheme.TextBackGroundColor = Color.FromName(nameValue[1].Trim());
                                        }
                                    }
                                    catch
                                    {
                                        CurrentTheme.TextBackGroundColor = Color.White;
                                    }
                                    break;//Ends:Modification on 17-Dec-2015 for introducing new theme
                                case "displaygroupbuttonfont":
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.DisplayGroupButtonFont = CustomFontConverter.ConvertStringToFont(Utilities.ExecutionContext, nameValue[1].Trim());
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;
                                case "homebigbuttontextalignment":
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.HomeBigButtonTextAlignment = GetTextAligmentValue(nameValue[1]);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;
                                case "homemediumbuttontextalignment":
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.HomeMediumButtonTextAlignment = GetTextAligmentValue(nameValue[1]);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;
                                case "homesmallbuttontextalignment":
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.HomeSmallButtonTextAlignment = GetTextAligmentValue(nameValue[1]);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;
                                case "directcashbuttontextalignment":
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.DirectCashButtonTextAlignment = GetTextAligmentValue(nameValue[1]);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;
                                case "displaygroupbuttontextalignment":
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.DisplayGroupButtonTextAlignment = GetTextAligmentValue(nameValue[1]);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;
                                case "displayproductbuttontextalignment":
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.DisplayProductButtonTextAlignment = GetTextAligmentValue(nameValue[1]);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;
                                case "paymentmodebuttontextalignment":
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.PaymentModeButtonTextAlignment = GetTextAligmentValue(nameValue[1]);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;
                                case "preselectpaymentmodebuttontextalignment":
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.PreSelectPaymentModeButtonTextAlignment = GetTextAligmentValue(nameValue[1]);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;
                                case "redeemtokenbuttontextalignment":
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.RedeemTokenButtonTextAlignment = GetTextAligmentValue(nameValue[1]);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;
                                case "custoptionbtntextalignment":
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.CustOptionBtnTextAlignment = GetTextAligmentValue(nameValue[1]);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;
                                case "fsksalestextalignment":
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.FSKSalesTextAlignment = GetTextAligmentValue(nameValue[1]);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;
                                case "basiccardcountbuttontextalignment":
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        CurrentTheme.BasicCardCountButtonTextAlignment = GetTextAligmentValue(nameValue[1]);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;
                                case "displayproductsgreetingoption":
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        if (string.IsNullOrWhiteSpace(nameValue[1]) == false)
                                        {
                                            CurrentTheme.DisplayProductsGreetingOption = nameValue[1].Trim().ToUpper();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;
                                case "kioskcalendarwidth":
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        if (string.IsNullOrWhiteSpace(nameValue[1]) == false)
                                        {
                                            int.TryParse(nameValue[1].Trim(), out CurrentTheme.KioskCalendarWidth);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;
                                case "kioskcalendarheight":
                                    try
                                    {
                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                        if (string.IsNullOrWhiteSpace(nameValue[1]) == false)
                                        {

                                            int.TryParse(nameValue[1].Trim(), out CurrentTheme.KioskCalendarHeight);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                    break;
                                default:
                                    if (CustomizedTagList != null && CustomizedTagList.Any())
                                    {
                                        foreach (string str in CustomizedTagList)
                                        {
                                            if (str.Trim().ToLower() == nameValue[0].Trim().ToLower())
                                            {
                                                System.Reflection.FieldInfo fieldInfo = CurrentTheme.GetType().GetField(str);
                                                if (fieldInfo != null)
                                                {
                                                    try
                                                    {
                                                        nameValue[1] = nameValue[1].Replace('~', '=');
                                                        if (nameValue[1].Contains("#"))
                                                        {
                                                            fieldInfo.SetValue(CurrentTheme, System.Drawing.ColorTranslator.FromHtml(nameValue[1].Trim()));
                                                        }
                                                        else
                                                        {
                                                            fieldInfo.SetValue(CurrentTheme, Color.FromName(nameValue[1].Trim()));
                                                        }
                                                    }
                                                    catch
                                                    {
                                                        fieldInfo.SetValue(CurrentTheme, Color.White);
                                                    }
                                                }
                                                SetDefaultColorsForSimilarTags(str.Trim().ToLower());
                                                break;
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    }
                }
                if (themeEntryFound == false)
                {
                    KioskStatic.logToFile("Configuration file has no entry for theme " + theme + ". Proceeding with in built kiosk theme.");
                }
                if (themeEntryFound == true && defaultbackgroundimageIsProvided == false)
                {
                    KioskStatic.logToFile("Theme " + theme + " is not having entry for DefaultBackgroundImage tag. Proceeding with in built images.");
                }
            }
            else
            {
                KioskStatic.logToFile("Kiosk Configuration text file not found in " + Application.StartupPath + ". Proceeding with in built kiosk theme.");
                SetDefaultThemeForeColor(source);
                setDefaultTextForeColor();
                SetDefaultSiteNameTextForeColor();
            }
            log.LogMethodExit();
        }

        private static string GetTextAligmentValue(string inputValue)
        {
            log.LogMethodEntry();
            string retValue = MIDDLELEFT;
            string finalInputValue = (string.IsNullOrWhiteSpace(inputValue) == false ? inputValue.Trim().ToLower() : MIDDLELEFT);
            switch (finalInputValue)
            {
                case "topleft":
                    retValue = TOPLEFT;
                    break;
                case "topcenter":
                    retValue = TOPCENTER;
                    break;
                case "topright":
                    retValue = TOPRIGHT;
                    break;
                case "middleleft":
                    retValue = TOPLEFT;
                    break;
                case "middlecenter":
                    retValue = MIDDLECENTER;
                    break;
                case "middleright":
                    retValue = MIDDLERIGHT;
                    break;
                case "bottomleft":
                    retValue = BOTTOMLEFT;
                    break;
                case "bottomcenter":
                    retValue = BOTTOMCENTER;
                    break;
                case "bottomright":
                    retValue = BOTTOMRIGHT;
                    break;
            }
            log.LogMethodExit(retValue);
            return retValue;
        }

        private static void SetDefaultForeColors(ExecutionContext machineUserContext, string[] lines, string theme, string source)
        {
            log.LogMethodEntry(lines, theme, source);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.Replace(" ", "").ToLower().Equals(theme))
                {
                    for (int j = i + 1; j < lines.Length; j++)
                    {
                        string themeLine = lines[j];
                        if (themeLine.Replace(" ", "").ToLower().Contains("[theme"))
                            break;

                        if (themeLine.Trim() == "")
                            continue;

                        logToFile(themeLine);
                        string[] nameValue = themeLine.Split('=');
                        switch (nameValue[0].Trim().ToLower())
                        {
                            case "themeforecolor"://Starts:Modification on 17-Dec-2015 for introducing new theme
                                try
                                {
                                    nameValue[1] = nameValue[1].Replace('~', '=');
                                    if (nameValue[1].Contains("#"))
                                    {
                                        CurrentTheme.ThemeForeColor = System.Drawing.ColorTranslator.FromHtml(nameValue[1].Trim());
                                    }
                                    else
                                    {
                                        CurrentTheme.ThemeForeColor = Color.FromName(nameValue[1].Trim());
                                    }
                                }

                                catch
                                {
                                    CurrentTheme.ThemeForeColor = Color.White;
                                }
                                break;
                            case "textforecolor":
                                try
                                {
                                    nameValue[1] = nameValue[1].Replace('~', '=');
                                    if (nameValue[1].Contains("#"))
                                    {
                                        CurrentTheme.TextForeColor = System.Drawing.ColorTranslator.FromHtml(nameValue[1].Trim());
                                    }
                                    else
                                    {
                                        CurrentTheme.TextForeColor = Color.FromName(nameValue[1].Trim());
                                    }
                                }

                                catch
                                {
                                    CurrentTheme.TextForeColor = Color.White;
                                }
                                break;
                            case "siteheadingforecolor":
                                try
                                {
                                    nameValue[1] = nameValue[1].Replace('~', '=');
                                    if (nameValue[1].Contains("#"))
                                    {
                                        CurrentTheme.TextForeColor = System.Drawing.ColorTranslator.FromHtml(nameValue[1].Trim());
                                    }
                                    else
                                    {
                                        CurrentTheme.TextForeColor = Color.FromName(nameValue[1].Trim());
                                    }
                                }

                                catch
                                {
                                    CurrentTheme.TextForeColor = Color.White;
                                }
                                break;
                            case "applicationversionforecolor":
                                try
                                {
                                    nameValue[1] = nameValue[1].Replace('~', '=');
                                    if (nameValue[1].Contains("#"))
                                    {
                                        CurrentTheme.TextForeColor = System.Drawing.ColorTranslator.FromHtml(nameValue[1].Trim());
                                    }
                                    else
                                    {
                                        CurrentTheme.TextForeColor = Color.FromName(nameValue[1].Trim());
                                    }
                                }

                                catch
                                {
                                    CurrentTheme.TextForeColor = Color.White;
                                }
                                break;
                            case "screenheadingforecolor":
                                try
                                {
                                    nameValue[1] = nameValue[1].Replace('~', '=');
                                    if (nameValue[1].Contains("#"))
                                    {
                                        CurrentTheme.TextForeColor = System.Drawing.ColorTranslator.FromHtml(nameValue[1].Trim());
                                    }
                                    else
                                    {
                                        CurrentTheme.TextForeColor = Color.FromName(nameValue[1].Trim());
                                    }
                                }

                                catch
                                {
                                    CurrentTheme.TextForeColor = Color.White;
                                }
                                break;
                            case "fieldlabelforecolor":
                                try
                                {
                                    nameValue[1] = nameValue[1].Replace('~', '=');
                                    if (nameValue[1].Contains("#"))
                                    {
                                        CurrentTheme.TextForeColor = System.Drawing.ColorTranslator.FromHtml(nameValue[1].Trim());
                                    }
                                    else
                                    {
                                        CurrentTheme.TextForeColor = Color.FromName(nameValue[1].Trim());
                                    }
                                }

                                catch
                                {
                                    CurrentTheme.TextForeColor = Color.White;
                                }
                                break;
                        }
                    }
                }
            }
            SetDefaultThemeForeColor(source);
            setDefaultTextForeColor();
            SetDefaultSiteNameTextForeColor();
            log.LogMethodExit();
        }

        private static void SetDefaultSiteNameTextForeColor()
        {
            log.LogMethodEntry();
            KioskStatic.CurrentTheme.FrmPrintTransLblSiteNameTextForeColor =
            KioskStatic.CurrentTheme.FrmCashTransactionLblSiteNameTextForeColor =
            KioskStatic.CurrentTheme.FskExecuteOnlineLblSiteNameTextForeColor =
            KioskStatic.CurrentTheme.FrmSignWaiversLblSiteNameTextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardLblSiteNameTextForeColor =
            KioskStatic.CurrentTheme.FrmKioskTransactionViewSiteTextForeColor = CurrentTheme.SiteHeadingForeColor;
            log.LogMethodExit();
        }

        private static void SetDefaultThemeForeColor(string source)
        {
            log.LogMethodEntry();
            KioskStatic.CurrentTheme.HomeScreenOptionsTextForeColor =
            KioskStatic.CurrentTheme.HomeScreenBtnLanguageTextForeColor =
            KioskStatic.CurrentTheme.HomeScreenBtnTermsTextForeColor =
            KioskStatic.CurrentTheme.HomeScreenFooterTextForeColor =
            KioskStatic.CurrentTheme.HomeScreenBtnRegisterTextForeColor =
            KioskStatic.CurrentTheme.HomeScreenBtnCheckBalanceTextForeColor =
             KioskStatic.CurrentTheme.HomeScreenBtnPurchaseTextForeColor =
            KioskStatic.CurrentTheme.HomeScreenBtnNewCardTextForeColor =
            KioskStatic.CurrentTheme.HomeScreenBtnRechargeTextForeColor =
            KioskStatic.CurrentTheme.HomeScreenBtnFoodAndBeveragesTextForeColor =
            KioskStatic.CurrentTheme.HomeScreenBtnTransferTextForeColor =
            KioskStatic.CurrentTheme.HomeScreenBtnRedeemTokensTextForeColor =
            KioskStatic.CurrentTheme.HomeScreenBtnPauseTimeTextForeColor =
            KioskStatic.CurrentTheme.HomeScreenBtnPointsToTimeTextForeColor =
            KioskStatic.CurrentTheme.HomeScreenBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.HomeScreenBtnSignWaiverTextForeColor =
            KioskStatic.CurrentTheme.HomeScreenBtnPlaygroundEntryTextForeColor =
            KioskStatic.CurrentTheme.HomeScreenBtnAttractionsTextForeColor =

            KioskStatic.CurrentTheme.DisplayGroupBtnTextForeColor =
            KioskStatic.CurrentTheme.DisplayGroupCancelBtnTextForeColor =
            KioskStatic.CurrentTheme.DisplayGroupBackBtnTextForeColor =
            KioskStatic.CurrentTheme.DisplayGroupFooterTextForeColor =
            KioskStatic.CurrentTheme.DisplayGroupGreetingsTextForeColor =

            KioskStatic.CurrentTheme.ChooseProductsBtnTextForeColor =
            KioskStatic.CurrentTheme.ChooseProductsCancelBtnTextForeColor =
            KioskStatic.CurrentTheme.ChooseProductVariableBtnTextForeColor =
            KioskStatic.CurrentTheme.ChooseProductsBackBtnTextForeColor =
            KioskStatic.CurrentTheme.ChooseProductsVariableBtnTextForeColor =
            KioskStatic.CurrentTheme.ChooseProductsFooterTextForeColor =
            KioskStatic.CurrentTheme.ChooseProductsGreetingsTextForeColor =
            KioskStatic.CurrentTheme.ChooseProductsBtnHomeTextForeColor =

            KioskStatic.CurrentTheme.TapCardScreenHeaderTextForeColor =
            KioskStatic.CurrentTheme.TapCardScreenNewCardTextForeColor =
            KioskStatic.CurrentTheme.TapCardScreenYesBtnTextForeColor =
            KioskStatic.CurrentTheme.TapCardScreenCloseBtnTextForeColor =
            KioskStatic.CurrentTheme.TapCardScreenNoBtnTextForeColor =
            KioskStatic.CurrentTheme.TapCardScreenButton1TextForeColor =
            KioskStatic.CurrentTheme.TapCardScreenNoteTextForeColor =
            KioskStatic.CurrentTheme.TapCardScreenLblMessageTextForeColor =

            KioskStatic.CurrentTheme.PaymentModeBtnTextForeColor =
            KioskStatic.CurrentTheme.PaymentModeSummaryHeaderTextForeColor =
            KioskStatic.CurrentTheme.PaymentModeDepositeHeaderTextForeColor =
            KioskStatic.CurrentTheme.PaymentModeDiscountHeaderTextForeColor =
            KioskStatic.CurrentTheme.PaymentModeSummaryInfoTextForeColor =
            KioskStatic.CurrentTheme.PaymentModeDepositeInfoTextForeColor =
            KioskStatic.CurrentTheme.PaymentModeDiscountInfoTextForeColor =
            KioskStatic.CurrentTheme.PaymentModeTotalToPayHeaderTextForeColor =
            KioskStatic.CurrentTheme.PaymentModeTotalToPayInfoTextForeColor =
            KioskStatic.CurrentTheme.PaymentModeDiscountBtnTextForeColor =
            KioskStatic.CurrentTheme.PaymentModeCPValidityTextForeColor =
            KioskStatic.CurrentTheme.PaymentModeBackButtonTextForeColor =
            KioskStatic.CurrentTheme.PaymentModeCancelButtonTextForeColor =
            KioskStatic.CurrentTheme.PaymentModeButtonsTextForeColor =
            KioskStatic.CurrentTheme.PaymentModeLblHeadingTextForeColor =
            KioskStatic.CurrentTheme.PaymentModeLblCardnumberTextForeColor =
            KioskStatic.CurrentTheme.PaymentModeLblPackageTextForeColor =
            KioskStatic.CurrentTheme.PaymentModeDiscountLabelTextForeColor =
            KioskStatic.CurrentTheme.PaymentModeLblPriceOfCardDepositTextForeColor =
            KioskStatic.CurrentTheme.PaymentModeTextMessageTextForeColor =
            KioskStatic.CurrentTheme.PaymentModeBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.PaymentModeLblPassportCouponTextForeColor =

            KioskStatic.CurrentTheme.YesNoScreenMessageTextForeColor =
            KioskStatic.CurrentTheme.YesNoScreenBtnYesTextForeColor =
            KioskStatic.CurrentTheme.YesNoScreenBtnNoTextForeColor =
            KioskStatic.CurrentTheme.YesNoScreenLblAdditionalMessageTextForeColor =

            KioskStatic.CurrentTheme.TransferFormTapMessageTextForeColor =
            KioskStatic.CurrentTheme.TransferFormFooterTextForeColor =
            KioskStatic.CurrentTheme.TransferFormBtnPrevTextForeColor =
            KioskStatic.CurrentTheme.TransferFormBtnNextTextForeColor =
            KioskStatic.CurrentTheme.TransferFormCardDetailsHeaderTextForeColor =
            KioskStatic.CurrentTheme.TransferFormCardDetailsInfoTextForeColor =
            KioskStatic.CurrentTheme.TransferFormTimeRemainingTextForeColor =

            KioskStatic.CurrentTheme.FAQScreenHeaderTextForeColor =
            KioskStatic.CurrentTheme.FAQScreenBtnTermsTextForeColor =
            KioskStatic.CurrentTheme.FAQScreenBtnBackTextForeColor =

            KioskStatic.CurrentTheme.PauseTimeLblMessageTextForeColor =
            KioskStatic.CurrentTheme.PauseTimeCardNumberHeaderTextForeColor =
            KioskStatic.CurrentTheme.PauseTimeTimeHeaderTextForeColor =
            KioskStatic.CurrentTheme.PauseTimeETicketHeaderTextForeColor =
            KioskStatic.CurrentTheme.PauseTimeETicketInfoTextForeColor =
            KioskStatic.CurrentTheme.PauseTimeBackBtnTextForeColor =
            KioskStatic.CurrentTheme.PauseTimeOkBtnTextForeColor =

            KioskStatic.CurrentTheme.CardCountScreenHeader1TextForeColor =
            KioskStatic.CurrentTheme.CardCountScreenHeader2TextForeColor =
            KioskStatic.CurrentTheme.CardCountScreenCardBtnsTextForeColor =
            KioskStatic.CurrentTheme.CardCountScreenBackBtnTextForeColor =
            KioskStatic.CurrentTheme.CardCountScreenCancelBtnTextForeColor =

            KioskStatic.CurrentTheme.AdminScreenBtnCancelTextForeColor =
            KioskStatic.CurrentTheme.AdminScreenBtnExitTextForeColor =
            KioskStatic.CurrentTheme.AdminScreenBtnSetupTextForeColor =
            KioskStatic.CurrentTheme.AdminScreenBtnLoadBonusTextForeColor =
            KioskStatic.CurrentTheme.AdminScreenBtnPrintSummaryTextForeColor =
            KioskStatic.CurrentTheme.AdminScreenBtnKioskActivityTextForeColor =
            KioskStatic.CurrentTheme.AdminScreenBtnRebootComputerTextForeColor =
            KioskStatic.CurrentTheme.AdminScreenBtnTrxViewTextForeColor =

            KioskStatic.CurrentTheme.CashInsertScreenHeader1TextForeColor =
            KioskStatic.CurrentTheme.CashInsertScreenHeader2TextForeColor =
            KioskStatic.CurrentTheme.CashInsertScreenCashInsertedInfoTextForeColor =
            KioskStatic.CurrentTheme.CashInsertScreenBtnNewCardTextForeColor =
            KioskStatic.CurrentTheme.CashInsertScreenBtnRechargeTextForeColor =
            KioskStatic.CurrentTheme.CashInsertScreenHeader3TextForeColor =
            KioskStatic.CurrentTheme.CashInsertScreenDenominationHeaderTextForeColor =
            KioskStatic.CurrentTheme.CashInsertScreenQuantityHeaderTextForeColor =
            KioskStatic.CurrentTheme.CashInsertScreenPointsHeaderTextForeColor =
            KioskStatic.CurrentTheme.CashInsertScreenCancelButtonTextForeColor =

            KioskStatic.CurrentTheme.AgeGateScreenHeader1TextForeColor =
            KioskStatic.CurrentTheme.AgeGateScreenHeader2TextForeColor =
            KioskStatic.CurrentTheme.AgeGateScreenHeader3TextForeColor =
            KioskStatic.CurrentTheme.AgeGateScreenDateFormatTextForeColor =
            KioskStatic.CurrentTheme.AgeGateScreenReadConfirmTextForeColor =
            KioskStatic.CurrentTheme.AgeGateScreenlnkTermsTextForeColor =
            KioskStatic.CurrentTheme.AgeGateScreenBtnCancelTextForeColor =
            KioskStatic.CurrentTheme.AgeGateScreenBtnNextTextForeColor =

            KioskStatic.CurrentTheme.CustomerScreenHeader1TextForeColor =
            KioskStatic.CurrentTheme.CustomerScreenHeader2TextForeColor =
            KioskStatic.CurrentTheme.CustomerScreenDetailsHeaderTextForeColor =
            KioskStatic.CurrentTheme.CustomerScreenInfoTextForeColor =
            KioskStatic.CurrentTheme.CustomerScreenOptInTextForeColor =
            KioskStatic.CurrentTheme.CustomerScreenTermsAndConditionsTextForeColor =
            KioskStatic.CurrentTheme.CustomerScreenWhatsAppOptOutTextForeColor =
            KioskStatic.CurrentTheme.CustomerScreenPhotoTextForeColor =
            KioskStatic.CurrentTheme.CustomerScreenPrivacyTextForeColor =
            KioskStatic.CurrentTheme.CustomerScreenInkTermsTextForeColor =
            KioskStatic.CurrentTheme.CustomerScreenBtnPrevTextForeColor =
            KioskStatic.CurrentTheme.CustomerScreenBtnSaveTextForeColor =
            KioskStatic.CurrentTheme.CustomerScreenFooterTextForeColor =
            KioskStatic.CurrentTheme.CustomerScreenLblTimeRemainingTextForeColor =
            KioskStatic.CurrentTheme.CustomerScreenBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.CustomerScreenDgvLinkedRelationsTextForeColor =
            KioskStatic.CurrentTheme.CustomerScreenLblLinkedRelationsTextForeColor =
            KioskStatic.CurrentTheme.CustomerScreenBtnAddRelationTextForeColor =

            KioskStatic.CurrentTheme.CheckBalanceHeader1TextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceCardNumerHeaderTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceCardNumberInfoTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceTicketModeHeaderTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceTicketModeInfoTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceBtnChangeTextForeColor =
            KioskStatic.CurrentTheme.CheckBalancePlayValueHeaderTextForeColor =
            KioskStatic.CurrentTheme.CheckBalancePlayValueInfoTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceBonusHeaderTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceBonusInfoTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceVirtualPointInfoTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceVirtualPointHeaderTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceCourtesyHeaderTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceCourtesyInfoTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceTimeHeaderTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceTimeInfoTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceLPHeaderTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceLPInfoTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceGameHeaderTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceGameInfoTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceETicketHeaderTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceETicketInfoTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceBackButtonTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceTopUpButtonTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceActivityButtonTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceFooterTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceTimeOutTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceBtnViewSignedWaiversTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceBtnHomeTextForeColor =

            KioskStatic.CurrentTheme.CustomerDashboardCardNumberTextForeColor =
            KioskStatic.CurrentTheme.CustomerDashboardCardNumberInfoTextForeColor =
            KioskStatic.CurrentTheme.CustomerDashboardTicketModeTextForeColor =
            KioskStatic.CurrentTheme.CustomerDashboardTicketModeInfoTextForeColor =
            KioskStatic.CurrentTheme.CustomerDashboardChangeButtonTextForeColor =
            KioskStatic.CurrentTheme.CustomerDashboardHeader1TextForeColor =
            KioskStatic.CurrentTheme.CustomerDashboardHeader1DetailsTextForeColor =
            KioskStatic.CurrentTheme.CustomerDashboardHeader2TextForeColor =
            KioskStatic.CurrentTheme.CustomerDashboardHeader2DetailsTextForeColor =
            KioskStatic.CurrentTheme.CustomerDashboardBackBtnTextForeColor =
            KioskStatic.CurrentTheme.CustomerDashboardTopUpBtnTextForeColor =
            KioskStatic.CurrentTheme.CustomerDashboardRegisterBtnTextForeColor =
            KioskStatic.CurrentTheme.CustomerDashboardButton1TextForeColor =
            KioskStatic.CurrentTheme.CustomerDashboardLblPlayValueLabelTextForeColor =
            KioskStatic.CurrentTheme.CustomerDashboardLblCreditsTextForeColor =
            KioskStatic.CurrentTheme.CustomerDashboardLblTimeOutTextForeColor =
            KioskStatic.CurrentTheme.CustomerDashboardBtnHomeTextForeColor =


            KioskStatic.CurrentTheme.CreditsToTimeHeaderTextForeColor =
            KioskStatic.CurrentTheme.CreditsToTimeDetailsHeaderTextForeColor =
            KioskStatic.CurrentTheme.CreditsToTimeBtnPrevTextForeColor =
            KioskStatic.CurrentTheme.CreditsToTimeBtnSaveTextForeColor =
            KioskStatic.CurrentTheme.CreditsToTimeAfterSaveHeaderTextForeColor =
            KioskStatic.CurrentTheme.CreditsToTimeFooterTextForeColor =
            KioskStatic.CurrentTheme.CreditsToTimeTimeRemainingTextForeColor =
            KioskStatic.CurrentTheme.CreditsToTimeBtnHomeTextForeColor =

            KioskStatic.CurrentTheme.RedeemTokensScreenHeaderTextForeColor =
            KioskStatic.CurrentTheme.RedeemTokensScreenTokenInsertedTextForeColor =
            KioskStatic.CurrentTheme.RedeemTokensScreenAvialableTokensTextForeColor =
            KioskStatic.CurrentTheme.RedeemTokensScreenBtnNewCardTextForeColor =
            KioskStatic.CurrentTheme.RedeemTokensScreenBtnLoadTextForeColor =
            KioskStatic.CurrentTheme.RedeemTokensScreenBtnBackTextForeColor =
            KioskStatic.CurrentTheme.RedeemTokensScreenFooterTextForeColor =
            KioskStatic.CurrentTheme.RedeemTokensScreenDenominationTextForeColor =
            KioskStatic.CurrentTheme.RedeemTokensScreenQuantityTextForeColor =
            KioskStatic.CurrentTheme.RedeemTokensScreenPointsTextForeColor =
            KioskStatic.CurrentTheme.RedeemTokensScreenTimeOutTextForeColor =
            KioskStatic.CurrentTheme.RedeemTokensScreenButton1extForeColor =
            KioskStatic.CurrentTheme.RedeemTokensScreenBtnHomeTextForeColor =

            KioskStatic.CurrentTheme.TransferToScreenHeaderTextForeColor =
            KioskStatic.CurrentTheme.TransferToScreenTimeRemainingTextForeColor =
            KioskStatic.CurrentTheme.TransferToScreenCardsHeaderTextForeColor =
            KioskStatic.CurrentTheme.TransferToScreenCreditsHeaderTextForeColor =
            KioskStatic.CurrentTheme.TransferToScreenPointsHeaderTextForeColor =
            KioskStatic.CurrentTheme.TransferToScreenCardsInfoTextForeColor =
            KioskStatic.CurrentTheme.TransferToScreenAvlTokensInfoTextForeColor =
            KioskStatic.CurrentTheme.TransferToScreenTransTokensInfoTextForeColor =
            KioskStatic.CurrentTheme.TransferToScreenBtnPrevTextForeColor =
            KioskStatic.CurrentTheme.TransferToScreenBtnNextTextForeColor =
            KioskStatic.CurrentTheme.TransferToScreenTransfererHeaderTextForeColor =
            KioskStatic.CurrentTheme.TransferToScreenTransfereeHeaderTextForeColor =
            KioskStatic.CurrentTheme.TransferToScreenCardsHeader2TextForeColor =
            KioskStatic.CurrentTheme.TransferToScreenCreditsHeader2TextForeColor =
            KioskStatic.CurrentTheme.TransferToScreenCardsInfo2TextForeColor =
            KioskStatic.CurrentTheme.TransferToScreenCreditsInfo2TextForeColor =
            KioskStatic.CurrentTheme.TransferToScreenFooterTextForeColor =

            KioskStatic.CurrentTheme.CardGamesPrevTextForeColor =

            KioskStatic.CurrentTheme.RegisterTnCHeaderTextForeColor =
            KioskStatic.CurrentTheme.RegisterTnCBtnYesTextForeColor =
            KioskStatic.CurrentTheme.RegisterTnCBtnCancelTextForeColor =
            KioskStatic.CurrentTheme.RegisterTnCBtnPrevTextForeColor =

            KioskStatic.CurrentTheme.TicketTypeHeaderTextForeColor =
            KioskStatic.CurrentTheme.TicketTypeBtnOkTextForeColor =
            KioskStatic.CurrentTheme.TicketTypeBtnCancelTextForeColor =

            KioskStatic.CurrentTheme.CardCountBasicsHeader1TextForeColor =
            KioskStatic.CurrentTheme.CardCountBasicsHeader2TextForeColor =
            KioskStatic.CurrentTheme.CardCountBasicsHeader3TextForeColor =
            KioskStatic.CurrentTheme.CardCountBasicsBtnPrevTextForeColor =

            KioskStatic.CurrentTheme.KioskActivityHeaderTextForeColor =
            KioskStatic.CurrentTheme.KioskActivityDetailsHeaderTextForeColor =
            KioskStatic.CurrentTheme.KioskActivityBtnPrevTextForeColor =
            KioskStatic.CurrentTheme.KioskActivityTxtMessageTextForeColor =

            KioskStatic.CurrentTheme.CardTransactionTimeOutTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionSummaryHeaderTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionSummaryInfoTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionDepositeHeaderTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionDepositeInfoTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionDiscountHeaderTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionDiscountInfoTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionFundHeaderTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionFundInfoTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionDonationHeaderTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionDonationInfoTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionTotalToPayHeaderTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionTotalToPayInfoTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionAmountPaidHeaderTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionAmountPaidInfoTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionBalanceToPayHeaderTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionBalanceToPayInfoTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionCPVlaiditTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionCancelButtonTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionBtnCreditTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionBtnDebitTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionFooterTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionPaymentButtonsTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionLblTimeOutTextForeColor =

            KioskStatic.CurrentTheme.LoadBonusHeaderTextForeColor =
            KioskStatic.CurrentTheme.LoadBonusCardHeaderTextForeColor =
            KioskStatic.CurrentTheme.LoadBonusCreditsHeaderTextForeColor =
            KioskStatic.CurrentTheme.LoadBonusBonusHeaderTextForeColor =
            KioskStatic.CurrentTheme.LoadBonusCardInfoTextForeColor =
            KioskStatic.CurrentTheme.LoadBonusCreditsInfoTextForeColor =
            KioskStatic.CurrentTheme.LoadBonusBonusInfoTextForeColor =
            KioskStatic.CurrentTheme.LoadBonusFooterTextForeColor =
            KioskStatic.CurrentTheme.LoadBonusBtnSaveTextForeColor =
            KioskStatic.CurrentTheme.LoadBonusBtnPrevTextForeColor =

            KioskStatic.CurrentTheme.ThankYouScreenHeader1TextForeColor =
            KioskStatic.CurrentTheme.ThankYouScreenHeader2TextForeColor =
            KioskStatic.CurrentTheme.ThankYouScreenBtnPrevTextForeColor =

            KioskStatic.CurrentTheme.UpsellProductScreenTimeOutTextForeColor =
            KioskStatic.CurrentTheme.UpsellProductScreenHeader1TextForeColor =
            KioskStatic.CurrentTheme.UpsellProductScreenHeader2TextForeColor =
            KioskStatic.CurrentTheme.UpsellProductScreenDesc1TextForeColor =
            KioskStatic.CurrentTheme.UpsellProductScreenDesc2TextForeColor =
            KioskStatic.CurrentTheme.UpsellProductScreenDesc3TextForeColor =
            KioskStatic.CurrentTheme.UpsellProductScreenBtnYesTextForeColor =
            KioskStatic.CurrentTheme.UpsellProductScreenBtnNoTextForeColor =
            KioskStatic.CurrentTheme.UpsellProductScreenFooterTextForeColor =
            KioskStatic.CurrentTheme.UpsellProductScreenBtnHomeTextForeColor =

            KioskStatic.CurrentTheme.ChooseEntitlementHeaderTextForeColor =
            KioskStatic.CurrentTheme.ChooseEntitlementBtnTimeTextForeColor =
            KioskStatic.CurrentTheme.ChooseEntitlemenBtnPointsTextForeColor =
            KioskStatic.CurrentTheme.ChooseEntitlementBtnOkTextForeColor =

            KioskStatic.CurrentTheme.OkMsgScreenHeaderTextForeColor =
            KioskStatic.CurrentTheme.OkMsgScreenBtnCloseTextForeColor =

            KioskStatic.CurrentTheme.ScanCouponScreenHeaderTextForeColor =
            KioskStatic.CurrentTheme.ScanCouponScreenCouponHeaderTextForeColor =
            KioskStatic.CurrentTheme.ScanCouponScreenBtnApplyTextForeColor =
            KioskStatic.CurrentTheme.ScanCouponScreenBtnCloseTextForeColor =

            KioskStatic.CurrentTheme.TimeOutCounterHeader1TextForeColor =
            KioskStatic.CurrentTheme.TimeOutCounterTimerTextForeColor =
            KioskStatic.CurrentTheme.TimeOutCounterHeader2TextForeColor =

            KioskStatic.CurrentTheme.FrmKioskTransViewLblGreetingTextForeColor =
            KioskStatic.CurrentTheme.FrmKioskTransViewLblFromDateTextForeColor =
            KioskStatic.CurrentTheme.FrmKioskTransViewLabel1TextForeColor =
            KioskStatic.CurrentTheme.FrmKioskTransViewLblPosMachinesTextForeColor =
            KioskStatic.CurrentTheme.FrmKioskTransViewLblTrxIdTextForeColor =
            KioskStatic.CurrentTheme.FrmKioskTransViewBtnSearchTextForeColor =
            KioskStatic.CurrentTheme.FrmKioskTransViewBtnClearTextForeColor =
            KioskStatic.CurrentTheme.FrmKioskTransViewLblTransactionTextForeColor =

            KioskStatic.CurrentTheme.FrmKioskTransViewTextBtnPrevForeColor =
            KioskStatic.CurrentTheme.FrmKioskTransViewTextBtnPrintReceiptForeColor =
            KioskStatic.CurrentTheme.FrmKioskTransViewTextBtnRefundForeColor =
            KioskStatic.CurrentTheme.FrmKioskTransViewTextBtnPrintPendingForeColor =
            KioskStatic.CurrentTheme.FrmKioskTransViewTextBtnIssueTempCardForeColor =
            KioskStatic.CurrentTheme.FrmKioskTransViewTextTxtMessageForeColor =
            KioskStatic.CurrentTheme.FrmKioskTransactionViewSiteTextForeColor =

            KioskStatic.CurrentTheme.FrmPrintTransLblGreetingTextForeColor =
            KioskStatic.CurrentTheme.FrmPrintTransLblCardsPendingPrintTextForeColor =
            KioskStatic.CurrentTheme.FrmPrintTransLblPrintedCardsTextForeColor =
            KioskStatic.CurrentTheme.FrmPrintTransLblTransactionDetailsTextForeColor =
            KioskStatic.CurrentTheme.FrmPrintTransTxtMessageTextForeColor =
            KioskStatic.CurrentTheme.FrmPrintTransDgvTransactionHeaderTextForeColor =
            KioskStatic.CurrentTheme.FrmPrintTransDgvPendingPrintTransactionLinesTextForeColor =
            KioskStatic.CurrentTheme.FrmPrintTransDgvPrintedTransactionLinesTextForeColor =
            KioskStatic.CurrentTheme.FrmPrintTransLblPrintReasonTextForeColor =
            KioskStatic.CurrentTheme.FrmPrintTransCmbPrintReasonTextForeColor =
            KioskStatic.CurrentTheme.FrmPrintTransBtnCancelTextForeColor =
            KioskStatic.CurrentTheme.FrmPrintTransBtnPrintPendingTextForeColor =
            KioskStatic.CurrentTheme.FrmPrintTransLblSiteNameTextForeColor =
            KioskStatic.CurrentTheme.FrmPrintTransDgvTransactionHeaderControlsTextForeColor =
            KioskStatic.CurrentTheme.FrmPrintTransDgvPrintedTransactionLinesControlsTextForeColor =
            KioskStatic.CurrentTheme.FrmPrintTransDgvPendingPrintTransactionLinesControlsTextForeColor =

            KioskStatic.CurrentTheme.FrmCashTransactionBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.FrmCashTransactionButton1TextForeColor =
            KioskStatic.CurrentTheme.FrmCashTransactionLblTimeOutTextForeColor =
            KioskStatic.CurrentTheme.FrmCashTransactionLabel6TextForeColor =
            KioskStatic.CurrentTheme.FrmCashTransactionLabel1TextForeColor =
            KioskStatic.CurrentTheme.FrmCashTransactionLblTotalToPayTextForeColor =
            KioskStatic.CurrentTheme.FrmCashTransactionLabel8TextForeColor =
            KioskStatic.CurrentTheme.FrmCashTransactionLabel9TextForeColor =
            KioskStatic.CurrentTheme.FrmCashTransactionLblPaidTextForeColor =
            KioskStatic.CurrentTheme.FrmCashTransactionLblBalTextForeColor =
            KioskStatic.CurrentTheme.FrmCashTransactionLblProductCPValidityMsgTextForeColor =
            KioskStatic.CurrentTheme.FrmCashTransactionLblNoChangeTextForeColor =
            KioskStatic.CurrentTheme.FrmCashTransactionBtnCreditCardTextForeColor =
            KioskStatic.CurrentTheme.FrmCashTransactionBtnDebitCardTextForeColor =
            KioskStatic.CurrentTheme.FrmCashTransactionLblSiteNameTextForeColor =
            KioskStatic.CurrentTheme.FrmCashTransactionTxtMessageTextForeColor =

            KioskStatic.CurrentTheme.FskExecutePnlPurchaseDetailsHeadersTextForeColor =
            KioskStatic.CurrentTheme.FskExecuteInfoTextForeColor =
            KioskStatic.CurrentTheme.FskExecutePnlPurchaseHeaderTextForeColor =
            KioskStatic.CurrentTheme.FskExecuteOnlineLblSiteNameTextForeColor =
            KioskStatic.CurrentTheme.FskExecuteOnlineLblTransactionOTPTextForeColor =
            KioskStatic.CurrentTheme.FskExecuteOnlineTxtTransactionOTPTextForeColor =
            KioskStatic.CurrentTheme.FskExecuteOnlineLblTransactionIdTextForeColor =
            KioskStatic.CurrentTheme.FskExecuteOnlinePnlTrasactionIdTextForeColor =
            KioskStatic.CurrentTheme.FskExecuteOnlineBtnGetTransactionDetailsTextForeColor =
            KioskStatic.CurrentTheme.FskExecuteOnlineBtnCancelTextForeColor =
            KioskStatic.CurrentTheme.FskExecuteOnlineTxtMessageTextForeColor =

            KioskStatic.CurrentTheme.FrmCustWaiverHeadersTextForeColor =
            KioskStatic.CurrentTheme.FrmCustWaiverBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.FrmCustWaiverLblSignatoryCustomerNameTextForeColor =
            KioskStatic.CurrentTheme.FrmCustWaiverLblWaiverSetTextForeColor =
            KioskStatic.CurrentTheme.FrmCustWaiverLabel2TextForeColor =
            KioskStatic.CurrentTheme.FrmCustWaiverBtnCancelTextForeColor =
            KioskStatic.CurrentTheme.FrmCustWaiverBtnAddNewRelationsTextForeColor =
            KioskStatic.CurrentTheme.FrmCustWaiverBtnProceedTextForeColor =
            KioskStatic.CurrentTheme.FrmCustWaiverTxtMessageTextForeColor =

            KioskStatic.CurrentTheme.FrmCustOptionBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.FrmCustOptionBtnCancelTextForeColor =
            KioskStatic.CurrentTheme.FrmCustOptionTxtMessageTextForeColor =

            KioskStatic.CurrentTheme.FrmCustOTPLblOTPTextForeColor =
            KioskStatic.CurrentTheme.FrmCustOTPLblOTPmsgTextForeColor =
            KioskStatic.CurrentTheme.FrmCustOTPLblTimeRemainingTextForeColor =
            KioskStatic.CurrentTheme.FrmCustOTPTxtOTPTextForeColor =
            KioskStatic.CurrentTheme.FrmCustOTPLinkLblResendOTPTextForeColor =
            KioskStatic.CurrentTheme.FrmCustOTPBtnOkayTextForeColor =
            KioskStatic.CurrentTheme.FrmCustOTPBtnCancelTextForeColor =
            KioskStatic.CurrentTheme.FrmCustOTPTxtMessageTextForeColor =

            KioskStatic.CurrentTheme.FrmCustSignConfirmationDgvCustomerSignedWaiverTextForeColor =
            KioskStatic.CurrentTheme.FrmCustSignConfirmationLblCustomerNameTextForeColor =
            KioskStatic.CurrentTheme.FrmCustSignConfirmationChkSignConfirmTextForeColor =
            KioskStatic.CurrentTheme.FrmCustSignConfirmationPbCheckBoxTextForeColor =
            KioskStatic.CurrentTheme.FrmCustSignConfirmationBtnOkayTextForeColor =
            KioskStatic.CurrentTheme.FrmCustSignConfirmationBtnCancelTextForeColor =

            KioskStatic.CurrentTheme.FrmGetCustInputLabel1TextForeColor =
            KioskStatic.CurrentTheme.FrmGetCustInputLabel2TextForeColor =
            KioskStatic.CurrentTheme.FrmGetCustInputLabel3TextForeColor =
            KioskStatic.CurrentTheme.FrmGetCustInputLblmsgTextForeColor =
            KioskStatic.CurrentTheme.FrmGetCustInputLmlEmailTextForeColor =
            KioskStatic.CurrentTheme.FrmGetCustInputTxtEmailTextForeColor =
            KioskStatic.CurrentTheme.FrmGetCustInputBtnOkTextForeColor =
            KioskStatic.CurrentTheme.FrmGetCustInputBtnCancelTextForeColor =
            KioskStatic.CurrentTheme.FrmGetCustInputTxtMessageTextForeColor =

            KioskStatic.CurrentTheme.FrmLinkCustLblCustomerTextForeColor =
            KioskStatic.CurrentTheme.FrmLinkCustLblRelatedCustomerTextForeColor =
            KioskStatic.CurrentTheme.FrmLinkCustLblCustomerValueTextForeColor =
            KioskStatic.CurrentTheme.FrmLinkCustLblRelatedCustomerValueTextForeColor =
            KioskStatic.CurrentTheme.FrmLinkCustLabel2TextForeColor =
            KioskStatic.CurrentTheme.FrmLinkCustCmbRelationTextForeColor =
            KioskStatic.CurrentTheme.FrmLinkCustBtnSaveTextForeColor =
            KioskStatic.CurrentTheme.FrmLinkCustBtnCancelTextForeColor =
            KioskStatic.CurrentTheme.FrmLinkCustTxtMessageTextForeColor =

            KioskStatic.CurrentTheme.FrmSelectWaiverBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.FrmSelectWaiverLabel1TextForeColor =
            KioskStatic.CurrentTheme.FrmSelectWaiverLblCustomerTextForeColor =
            KioskStatic.CurrentTheme.FrmSelectWaiverLblReservationCodeTextForeColor =
            KioskStatic.CurrentTheme.FrmSelectWaiverTxtReservationCodeTextForeColor =
            KioskStatic.CurrentTheme.FrmSelectWaiverLblReservationCodeORTextForeColor =
            KioskStatic.CurrentTheme.FrmSelectWaiverLblTrxOTPTextForeColor =
            KioskStatic.CurrentTheme.FrmSelectWaiverTxtTrxOTPTextForeColor =
            KioskStatic.CurrentTheme.FrmSelectWaiverLabel4TextForeColor =
            KioskStatic.CurrentTheme.FrmSelectWaiverLabel7TextForeColor =
            KioskStatic.CurrentTheme.FrmSelectWaiverLblWaiverTextForeColor =
            KioskStatic.CurrentTheme.FrmSelectWaiverLblSelectionTextForeColor =
            KioskStatic.CurrentTheme.FrmSelectWaiverDgvWaiverSetTextForeColor =
            KioskStatic.CurrentTheme.FrmSelectWaiverBtnProceedTextForeColor =
            KioskStatic.CurrentTheme.FrmSelectWaiverBtnCancelTextForeColor =
            KioskStatic.CurrentTheme.FrmSelectWaiverTxtMessageTextForeColor =

            KioskStatic.CurrentTheme.FrmSignWaiverFilesBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.FrmSignWaiverFilesLblSignWaiverFileTextForeColor =
            KioskStatic.CurrentTheme.FrmSignWaiverFilesLblCustomerNameTextForeColor =
            KioskStatic.CurrentTheme.FrmSignWaiverFilesLblCustomerContactTextForeColor =
            KioskStatic.CurrentTheme.FrmSignWaiverFilesPnlWaiverTextForeColor =
            KioskStatic.CurrentTheme.FrmSignWaiverFilesPnlWaiverDisplayTextForeColor =
            KioskStatic.CurrentTheme.FrmSignWaiverFilesBtnOkayTextForeColor =
            KioskStatic.CurrentTheme.FrmSignWaiverFilesBtnCancelTextForeColor =
            KioskStatic.CurrentTheme.FrmSignWaiverFilesTxtMessageTextForeColor =
            KioskStatic.CurrentTheme.FrmSignWaiverFilesChkSignConfirmTextForeColor =

            KioskStatic.CurrentTheme.FrmSignWaiversBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.FrmSignWaiversLblSiteNameTextForeColor =
            KioskStatic.CurrentTheme.FrmSignWaiversLblCustomerTextForeColor =
            KioskStatic.CurrentTheme.FrmSignWaiversLblGreeting1TextForeColor =
            KioskStatic.CurrentTheme.FrmSignWaiversLblWaiverTextForeColor =
            KioskStatic.CurrentTheme.FrmSignWaiversLblSelectionTextForeColor =
            KioskStatic.CurrentTheme.FrmSignWaiversLabel1TextForeColor =
            KioskStatic.CurrentTheme.FrmSignWaiversFpnlWaiverSetTextForeColor =
            KioskStatic.CurrentTheme.FrmSignWaiversBtnCancelTextForeColor =
            KioskStatic.CurrentTheme.FrmSignWaiversTxtMessageTextForeColor =

            KioskStatic.CurrentTheme.FrmViewWaiverUIBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.FrmViewWaiverUILblWaiverNameTextForeColor =
            KioskStatic.CurrentTheme.FrmViewWaiverUIWebBrowserTextForeColor =
            KioskStatic.CurrentTheme.FrmViewWaiverUIBtnCloseTextForeColor =

            KioskStatic.CurrentTheme.FrmEntitlementLblMsgTextForeColor =

            KioskStatic.CurrentTheme.FrmSplashScreenLabel1TextForeColor =

            KioskStatic.CurrentTheme.FrmSuccessMsgBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.FrmSuccessMsgLblHeadingTextForeColor =
            KioskStatic.CurrentTheme.FrmSuccessMsgLblmsgTextForeColor =
            KioskStatic.CurrentTheme.FrmSuccessMsgLblBalanceMsgTextForeColor =
            KioskStatic.CurrentTheme.FrmSuccessMsgLblPointTextForeColor =
            KioskStatic.CurrentTheme.FrmSuccessMsgLblPasNoTextForeColor =
            KioskStatic.CurrentTheme.FrmSuccessMsgBtnCloseTextForeColor =
            KioskStatic.CurrentTheme.FrmSuccessMsgLblTrxNumberTextForeColor =

            KioskStatic.CurrentTheme.PaymentGameCardLblSiteNameTextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardLblPaymentTextTextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardLblTotaltoPayTextTextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardLblTotalToPayTextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardLabel8TextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardLabel7TextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardLblTapCardTextTextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardLblCardNumberTextTextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardLblCardNumberTextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardLblAvailableCreditsTextTextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardLblAvailableCreditsTextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardLblPurchaseValueTextTextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardLblPurchaseValueTextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardLblBalanceCreditsTextTextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardLblBalanceCreditsTextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardBtnApplyTextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardBtnCancelTextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardTxtMessageTextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardTxtMessageErrorBackColor =
            KioskStatic.CurrentTheme.PaymentGameCardLblAvailableFreeEntriesTextTextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardLblAvailableFreeEntriesTextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardLblBalanceFreeEntriesTextTextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardLblBalanceFreeEntriesTextForeColor =

            KioskStatic.CurrentTheme.FrmLoyaltyLblLoyaltyTextTextForeColor =
            KioskStatic.CurrentTheme.FrmLoyaltyBtnLoyaltyYesTextForeColor =
            KioskStatic.CurrentTheme.FrmLoyaltyBtnLoyaltyNoTextForeColor =
            KioskStatic.CurrentTheme.FrmLoyaltyLblPhoneNoTextForeColor =
            KioskStatic.CurrentTheme.FrmLoyaltyTxtPhoneNoTextForeColor =
            KioskStatic.CurrentTheme.FrmLoyaltyBtnGoTextForeColor =
            KioskStatic.CurrentTheme.FrmLoyaltyListboxNamesTextForeColor =
            KioskStatic.CurrentTheme.FrmLoyaltyTxtFirstNameTextForeColor =
            KioskStatic.CurrentTheme.FrmLoyaltyBtnOkTextForeColor =
            KioskStatic.CurrentTheme.FrmLoyaltyBtnProceedWithoutLoyaltyTextForeColor =
            KioskStatic.CurrentTheme.FrmLoyaltyBtnCancelTextForeColor =
            KioskStatic.CurrentTheme.FrmLoyaltyTxtMessageTextForeColor =

            KioskStatic.CurrentTheme.AddCustomerRelationBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.AddCustomerRelationLblGreetingTextForeColor =
            KioskStatic.CurrentTheme.AddCustomerRelationLblCardNumberTextForeColor =
            KioskStatic.CurrentTheme.AddCustomerRelationLblCustomerNameTextForeColor =
            KioskStatic.CurrentTheme.AddCustomerRelationTxtCardNumberTextForeColor =
            KioskStatic.CurrentTheme.AddCustomerRelationTxtCustomerNameTextForeColor =
            KioskStatic.CurrentTheme.AddCustomerRelationLblAddParticipantsTextForeColor =
            KioskStatic.CurrentTheme.AddCustomerRelationBtnBackTextForeColor =
            KioskStatic.CurrentTheme.AddCustomerRelationBtnConfirmTextForeColor =
            KioskStatic.CurrentTheme.AddCustomerRelationTxtboxMessageLineTextForeColor =
            KioskStatic.CurrentTheme.AddCustomerRelationGridTextForeColor =
            KioskStatic.CurrentTheme.AddCustomerRelationLblTimeRemainingTextForeColor =
            KioskStatic.CurrentTheme.AddCustomerRelationLblCardNumberForeColor =
            KioskStatic.CurrentTheme.AddCustomerRelationLblCustomerNameForeColor =
            KioskStatic.CurrentTheme.AddCustomerRelationLblGridFooterTextForeColor =

            KioskStatic.CurrentTheme.ComboChildProductsQtyGreetingLblTextForeColor =
            KioskStatic.CurrentTheme.ComboChildProductsQtyBackButtonTextForeColor =
            KioskStatic.CurrentTheme.ComboChildProductsQtyCancelButtonTextForeColor =
            KioskStatic.CurrentTheme.ComboChildProductsQtyProceedButtonTextForeColor =
            KioskStatic.CurrentTheme.ComboChildProductsQtyFooterTxtMsgTextForeColor =
            KioskStatic.CurrentTheme.ComboChildProductsQtyHomeButtonTextForeColor =
            KioskStatic.CurrentTheme.ComboChildProductsQtyLblAgeCriteriaTextForeColor =

            KioskStatic.CurrentTheme.SelectChildGreetingLblTxtForeColor =
            KioskStatic.CurrentTheme.SelectChildYourfamilyLblTextForeColor =
            KioskStatic.CurrentTheme.SelectChildFooterTxtMsgTextForeColor =
            KioskStatic.CurrentTheme.SelectChildProceedButtonTextForeColor =
            KioskStatic.CurrentTheme.SelectChildSkipButtonTextForeColor =
            KioskStatic.CurrentTheme.SelectChildBackButtonTextForeColor =
            KioskStatic.CurrentTheme.SelectChildHomeButtonTextForeColor =
            KioskStatic.CurrentTheme.SelectChildMemberDetailsLblTextForeColor =

            KioskStatic.CurrentTheme.EnterChildDetailsGreetingLblTextForeColor =
            KioskStatic.CurrentTheme.EnterChildDetailsFooterTextMsgTextForeColor =
            KioskStatic.CurrentTheme.EnterChildDetailsProceedButtonTextForeColor =
            KioskStatic.CurrentTheme.EnterChildDetailsChildAddedLabelTextForeColor =
            KioskStatic.CurrentTheme.EnterChildDetailsBackButtonTextForeColor =
            KioskStatic.CurrentTheme.EnterChildDetailsHomeButtonTextForeColor =
            KioskStatic.CurrentTheme.EnterChildDetailsTimeRemainingLblTextForeColor =

            KioskStatic.CurrentTheme.ChildSummaryGreetingLblTextForeColor =
            KioskStatic.CurrentTheme.ChildSummaryFooterTxtMsgTextForeColor =
            KioskStatic.CurrentTheme.ChildSummaryProceedButtonTextForeColor =
            KioskStatic.CurrentTheme.ChildSummaryBackButtonTextForeColor =
            KioskStatic.CurrentTheme.ChildSummaryHomeButtonTextForeColor =

            KioskStatic.CurrentTheme.SelectAdultGreetingLblTxtForeColor =
            KioskStatic.CurrentTheme.SelectAdultYourfamilyLblTextForeColor =
            KioskStatic.CurrentTheme.SelectAdultFooterTxtMsgTextForeColor =
            KioskStatic.CurrentTheme.SelectAdultProceedButtonTextForeColor =
            KioskStatic.CurrentTheme.SelectAdultSkipButtonTextForeColor =
            KioskStatic.CurrentTheme.SelectAdultBackButtonTextForeColor =
            KioskStatic.CurrentTheme.SelectAdultHomeButtonTextForeColor =
            KioskStatic.CurrentTheme.SelectAdultMemberDetailsLblTextForeColor =
            KioskStatic.CurrentTheme.SelectAdultGridFooterMsgLblTextForeColor =
            KioskStatic.CurrentTheme.SelectAdultNoRelationMsg1LblTextForeColor =
            KioskStatic.CurrentTheme.SelectAdultNoRelationMsg2LblTextForeColor =
            KioskStatic.CurrentTheme.SelectAdultNoRelationMsg3LblTextForeColor =

            KioskStatic.CurrentTheme.EnterAdultDetailsGreetingLblTextForeColor =
            KioskStatic.CurrentTheme.EnterAdultDetailsFooterTextMsgTextForeColor =
            KioskStatic.CurrentTheme.EnterAdultDetailsProceedButtonTextForeColor =
            KioskStatic.CurrentTheme.EnterAdultDetailsChildAddedLabelTextForeColor =
            KioskStatic.CurrentTheme.EnterAdultDetailsBackButtonTextForeColor =
            KioskStatic.CurrentTheme.EnterAdultDetailsHomeButtonTextForeColor =
            KioskStatic.CurrentTheme.EnterAdultDetailsTimeRemainingLblTextForeColor =

            KioskStatic.CurrentTheme.AdultSummaryGreetingLblTextForeColor =
            KioskStatic.CurrentTheme.AdultSummaryFooterTxtMsgTextForeColor =
            KioskStatic.CurrentTheme.AdultSummaryProceedButtonTextForeColor =
            KioskStatic.CurrentTheme.AdultSummaryBackButtonTextForeColor =
            KioskStatic.CurrentTheme.AdultSummaryHomeButtonTextForeColor =

            KioskStatic.CurrentTheme.CheckInSummaryDetailsTextForeColor =
            KioskStatic.CurrentTheme.CheckInSummaryTotalToPayTextForeColor =
            KioskStatic.CurrentTheme.CheckInSummaryHomeButtonTextForeColor =
            KioskStatic.CurrentTheme.CheckInSummaryDiscountLabelTextForeColor =
            KioskStatic.CurrentTheme.CheckInSummaryApplyDiscountBtnTextForeColor =
            KioskStatic.CurrentTheme.CheckInSummaryCPValidityTextForeColor =
            KioskStatic.CurrentTheme.CheckInSummaryBackButtonTextForeColor =
            KioskStatic.CurrentTheme.CheckInSummaryProceedButtonTextForeColor =
            KioskStatic.CurrentTheme.CheckInSummaryFooterMessageTextForeColor =
            KioskStatic.CurrentTheme.CheckInSummaryLblPassportCouponTextForeColor =
            KioskStatic.CurrentTheme.CheckInSummaryPanelUsrCtrlLblTextForeColor =
            KioskStatic.CurrentTheme.CheckInSummaryPanelTextForeColor =

            KioskStatic.CurrentTheme.CalenderMsgTextForeColor =
            KioskStatic.CurrentTheme.CalenderBtnCancelTextForeColor =
            KioskStatic.CurrentTheme.CalenderBtnSaveTextForeColor =
            KioskStatic.CurrentTheme.CalenderItemHeaderTextForeColor =
            KioskStatic.CurrentTheme.CalenderGreetingTextForeColor =

            KioskStatic.CurrentTheme.PackageDetailsLblHeaderTextForeColor =
            KioskStatic.CurrentTheme.PackageDetailsLblTextForeColor =
            KioskStatic.CurrentTheme.GuestCountLblTextForeColor =
            KioskStatic.CurrentTheme.ScreenDetailsLblTextForeColor =
            KioskStatic.CurrentTheme.LblGridFooterMsgTextForeColor =

            KioskStatic.CurrentTheme.FundsDonationsBtnProductTextForeColor =
            KioskStatic.CurrentTheme.FundsDonationsBtnTextForeColor =
            KioskStatic.CurrentTheme.FundsDonationsGreetingTextForeColor =
            KioskStatic.CurrentTheme.FundsDonationsLblFundDonationMessageTextForeColor =
            KioskStatic.CurrentTheme.FundsDonationsFooterTextForeColor =

            KioskStatic.CurrentTheme.PurchaseMenuGreetingTextForeColor =
            KioskStatic.CurrentTheme.PurchaseMenuBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.PurchaseMenuFooterTextForeColor =
            KioskStatic.CurrentTheme.PurchaseMenuLblSiteNameTextForeColor =
            KioskStatic.CurrentTheme.PurchaseMenuBtnTextForeColor =

            KioskStatic.CurrentTheme.FrmGetEmailDetailsLblGreeting1TextForeColor =
            KioskStatic.CurrentTheme.FrmGetEmailDetailsLabel1TextForeColor =
            KioskStatic.CurrentTheme.FrmGetEmailDetailsLabel2TextForeColor =
            KioskStatic.CurrentTheme.FrmGetEmailDetailsLabel3TextForeColor =
            KioskStatic.CurrentTheme.FrmGetEmailDetailsBtnCancelTextForeColor =
            KioskStatic.CurrentTheme.FrmGetEmailDetailsBtnOkTextForeColor =
            KioskStatic.CurrentTheme.FrmGetEmailDetailsFooterTxtMsgTextForeColor =
            KioskStatic.CurrentTheme.FrmGetEmailDetailsLblSkipDetailsTextForeColor =
            KioskStatic.CurrentTheme.FrmGetEmailDetailsLblNoteTextForeColor =

            KioskStatic.CurrentTheme.FrmReceiptDeliveryModeOptionsLblGreeting1TextForeColor =
            KioskStatic.CurrentTheme.FrmReceiptDeliveryModeOptionsBtnPrintTextForeColor =
            KioskStatic.CurrentTheme.FrmReceiptDeliveryModeOptionsBtnEmailTextForeColor =
            KioskStatic.CurrentTheme.FrmReceiptDeliveryModeOptionsBtnNoneTextForeColor =

            KioskStatic.CurrentTheme.FrmKioskCartLblGreeting1TextForeColor =
            KioskStatic.CurrentTheme.FrmKioskCartButtonTextForeColor =
            KioskStatic.CurrentTheme.FrmKioskCartDiscountButtonTextForeColor =
            KioskStatic.CurrentTheme.FrmKioskCartLblHeaderTextForeColor =
            KioskStatic.CurrentTheme.FrmKioskCartLblAmountTextForeColor =
            KioskStatic.CurrentTheme.FrmKioskCartFooterTxtMsgTextForeColor =
            KioskStatic.CurrentTheme.KioskCartQuantityTextForeColor =
            KioskStatic.CurrentTheme.FrmKioskCartLblProductHeaderTextForeColor =

            KioskStatic.CurrentTheme.PreSelectPaymentModeLblHeadingTextForeColor =
            KioskStatic.CurrentTheme.PreSelectPaymentModeBackButtonTextForeColor =
            KioskStatic.CurrentTheme.PreSelectPaymentModeCancelButtonTextForeColor =
            KioskStatic.CurrentTheme.PreSelectPaymentModeBtnHomeTextForeColor =

            KioskStatic.CurrentTheme.PrintSummarySiteTextForeColor =
            KioskStatic.CurrentTheme.PrintSummaryLblGreetingTextForeColor =
            KioskStatic.CurrentTheme.PrintSummaryLblFromDateTextForeColor =
            KioskStatic.CurrentTheme.PrintSummaryLblToDateTextForeColor =
            KioskStatic.CurrentTheme.PrintSummaryBtnPrevTextForeColor =
            KioskStatic.CurrentTheme.PrintSummaryBtnPrintTextForeColor =
            KioskStatic.CurrentTheme.PrintSummaryBtnEmailTextForeColor =
            KioskStatic.CurrentTheme.PrintSummaryNewTabTextForeColor =
            KioskStatic.CurrentTheme.PrintSummaryRePrintTabTextForeColor =
            KioskStatic.CurrentTheme.PrintSummaryLblTabTextForeColor =

            KioskStatic.CurrentTheme.frmAddToCartAlertBtnCheckOutTextForeColor =
            KioskStatic.CurrentTheme.frmAddToCartAlertBtnCloseTextForeColor =
            KioskStatic.CurrentTheme.frmAddToCartAlertLblMsgTextForeColor =

            KioskStatic.CurrentTheme.TextBoxLblMsgTextForeColor =

            KioskStatic.CurrentTheme.ComboDetailsBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.ComboDetailsProductNameTextForeColor =
            KioskStatic.CurrentTheme.ComboDetailsPackageMsgTextForeColor =
            KioskStatic.CurrentTheme.ComboDetailsBackButtonTextForeColor =
            KioskStatic.CurrentTheme.ComboDetailsCancelButtonTextForeColor =
            KioskStatic.CurrentTheme.ComboDetailsProceedButtonTextForeColor =
            KioskStatic.CurrentTheme.ComboDetailsFooterTextForeColor =

            KioskStatic.CurrentTheme.AttractionQtyHomeButtonTextForeColor =
            KioskStatic.CurrentTheme.AttractionQtyBackButtonTextForeColor =
            KioskStatic.CurrentTheme.AttractionQtyCancelButtonTextForeColor =
            KioskStatic.CurrentTheme.AttractionQtyProceedButtonTextForeColor =
            KioskStatic.CurrentTheme.AttractionQtyFooterTextForeColor =
            KioskStatic.CurrentTheme.AttractionQtyLblHowManyMsgTextForeColor =
            KioskStatic.CurrentTheme.AttractionQtyLblEnterQtyMsgTextForeColor =
            KioskStatic.CurrentTheme.AttractionQtyLblBuyTextTextForeColor =
            KioskStatic.CurrentTheme.AttractionQtyLblProductNameTextForeColor =
            KioskStatic.CurrentTheme.AttractionQtyLblComboDetailsTextForeColor =
            KioskStatic.CurrentTheme.AttractionQtyLblProductDescriptionTextForeColor =

            KioskStatic.CurrentTheme.ProcessingAttractionsLblProcessingMsgTextForeColor =
            KioskStatic.CurrentTheme.ProcessingAttractionsFooterTxtMsgTextForeColor =
            KioskStatic.CurrentTheme.ProcessingAttractionsProceedButtonTextForeColor =
            KioskStatic.CurrentTheme.ProcessingAttractionsCancelButtonTextForeColor =
            KioskStatic.CurrentTheme.ProcessingAttractionsBackButtonTextForeColor =
            KioskStatic.CurrentTheme.ProcessingAttractionsHomeButtonTextForeColor =

            KioskStatic.CurrentTheme.CardSaleOptionHeaderTextForeColor =
            KioskStatic.CurrentTheme.CardSaleOptionCancelBtnTextForeColor =
            KioskStatic.CurrentTheme.CardSaleOptionConfirmBtnTextForeColor =

            KioskStatic.CurrentTheme.SelectSlotHomeButtonTextForeColor =
            KioskStatic.CurrentTheme.SelectSlotLblBookingSlotTextForeColor =
            KioskStatic.CurrentTheme.SelectSlotLblProductNameTextForeColor =
            KioskStatic.CurrentTheme.SelectSlotBackButtonTextForeColor =
            KioskStatic.CurrentTheme.SelectSlotCancelButtonTextForeColor =
            KioskStatic.CurrentTheme.SelectSlotProceedButtonTextForeColor =
            KioskStatic.CurrentTheme.SelectSlotFooterTextForeColor =
            KioskStatic.CurrentTheme.SelectSlotLblDateTextForeColor =
            KioskStatic.CurrentTheme.SelectSlotLblSelectSlotTextForeColor =
            KioskStatic.CurrentTheme.SelectSlotLblNoSchedulesTextForeColor =

            KioskStatic.CurrentTheme.AttractionSummaryLblBookingInfoForeColor =
            KioskStatic.CurrentTheme.AttractionSummaryLblProductNameForeColor =
            KioskStatic.CurrentTheme.AttractionSummaryLblQtyForeColor =
            KioskStatic.CurrentTheme.AttractionSummaryBtnPrevForeColor =
            KioskStatic.CurrentTheme.AttractionSummaryBtnCancelForeColor =
            KioskStatic.CurrentTheme.AttractionSummaryBtnProceedForeColor =
            KioskStatic.CurrentTheme.AttractionSummaryTxtMessageForeColor =
            KioskStatic.CurrentTheme.AttractionSummaryBtnHomeForeColor =
            KioskStatic.CurrentTheme.SearchCustomerSearchBtnTextForeColor =
            KioskStatic.CurrentTheme.SearchCustomerNewRegistrationBtnTextForeColor =
            KioskStatic.CurrentTheme.SearchCustomerPrevBtnTextForeColor =
            KioskStatic.CurrentTheme.SearchCustomerFooterTextForeColor =
            KioskStatic.CurrentTheme.SearchCustomerGreetingForeColor =
            KioskStatic.CurrentTheme.SearchCustomerLblNewCustomerHeaderForeColor =
            KioskStatic.CurrentTheme.SearchCustomerLblNewCustomerForeColor =
            KioskStatic.CurrentTheme.SearchCustomerExistingCustomerHeaderForeColor =
            KioskStatic.CurrentTheme.SearchCustomerExistingCustomerForeColor =
            KioskStatic.CurrentTheme.SearchCustomerLblEnterPhoneHeaderForeColor =
            KioskStatic.CurrentTheme.SearchCustomerLblEnterEmailHeaderForeColor =


            KioskStatic.CurrentTheme.CustomerFoundBtnOKTextForeColor =
            KioskStatic.CurrentTheme.CustomerFoundBtnPrevTextForeColor =
            KioskStatic.CurrentTheme.CustomerFoundLblCustomerNameTextForeColor =
            KioskStatic.CurrentTheme.CustomerFoundLblWelcomeMsgTextForeColor =
            KioskStatic.CurrentTheme.CustomerFoundLblClickOKMsgTextForeColor =

            KioskStatic.CurrentTheme.SelectCustomerLblGreetingTextForeColor =
            KioskStatic.CurrentTheme.SelectCustomerBtnProceedTextForeColor =
            KioskStatic.CurrentTheme.SelectCustomerBtnCancelTextForeColor =
            KioskStatic.CurrentTheme.SelectCustomerBtnPrevTextForeColor =
            KioskStatic.CurrentTheme.SelectCustomerFooterTextForeColor =
            KioskStatic.CurrentTheme.SelectCustomerBackButtonTextForeColor =

            KioskStatic.CurrentTheme.WaiverSigningAlertLblGreetingTextForeColor =
            KioskStatic.CurrentTheme.WaiverSigningAlertLblMsgTextForeColor =
            KioskStatic.CurrentTheme.WaiverSigningAlertBackButtonTextForeColor =
            KioskStatic.CurrentTheme.WaiverSigningAlertCancelButtonTextForeColor =
            KioskStatic.CurrentTheme.WaiverSigningAlertSignButtonTextForeColor =

            KioskStatic.CurrentTheme.UsrCtrlWaiverSigningAlertLblProductNameTextForeColor =
            KioskStatic.CurrentTheme.UsrCtrlWaiverSigningAlertLblParticipantsTextForeColor =

            KioskStatic.CurrentTheme.MapAttendeesLblGreetingTextForeColor =
            KioskStatic.CurrentTheme.MapAttendeesLblProductNameTextForeColor =
            KioskStatic.CurrentTheme.MapAttendeesLblMsgTextForeColor =
            KioskStatic.CurrentTheme.MapAttendeestBackButtonTextForeColor =
            KioskStatic.CurrentTheme.MapAttendeesCancelButtonTextForeColor =
            KioskStatic.CurrentTheme.MapAttendeesAssignButtonTextForeColor =
            KioskStatic.CurrentTheme.UsrCtrlMapAttendeesToQuantityBtnProductQtySelectedTextForeColor = 
            KioskStatic.CurrentTheme.MapAttendeesMappedCustomerNameTextForeColor = 

            KioskStatic.CurrentTheme.UsrCtrlMapAttendeeToProductBtnProductTextForeColor =
            KioskStatic.CurrentTheme.UsrCtrlMapAttendeeToProductBtnProductHighlightTextForeColor = 

            KioskStatic.CurrentTheme.CustomerRelationsGreetingTextForeColor =
            KioskStatic.CurrentTheme.CustomerRelationsBackButtonTextForeColor =
            KioskStatic.CurrentTheme.CustomerRelationsAddNewMemberButtonTextForeColor = 
            KioskStatic.CurrentTheme.CustomerRelationsSearchAnotherCustomerButtonTextForeColor = 
            KioskStatic.CurrentTheme.CustomerRelationsProceedButtonTextForeColor = 
            KioskStatic.CurrentTheme.CustomerRelationsFooterTextForeColor =

            KioskStatic.CurrentTheme.FilteredCustomersGreetingTextForeColor =
            KioskStatic.CurrentTheme.FilteredCustomersCloseButtonTextForeColor =
            KioskStatic.CurrentTheme.FilteredCustomersLinkButtonTextForeColor = CurrentTheme.ThemeForeColor;

            if (CurrentTheme.ThemeForeColor == Color.White)
            {
                KioskStatic.CurrentTheme.CardTransactionPaymentButtonsTextForeColor = Color.DarkOrchid;

                if (source.ToString().ToLower().Equals("tabletop"))
                    KioskStatic.CurrentTheme.UpsellProductScreenBtnYesTextForeColor = Color.White;
                else
                    KioskStatic.CurrentTheme.UpsellProductScreenBtnYesTextForeColor = Color.DarkOrchid;

                KioskStatic.CurrentTheme.FundsDonationsBtnProductTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.AgeGateScreenlnkTermsTextForeColor = Color.Blue;
                KioskStatic.CurrentTheme.AddCustomerRelationTxtCardNumberTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.AddCustomerRelationTxtCustomerNameTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.PaymentGameCardTxtMessageErrorBackColor = Color.Red;
                KioskStatic.CurrentTheme.FrmLoyaltyBtnLoyaltyYesTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmLoyaltyBtnLoyaltyNoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmLoyaltyBtnProceedWithoutLoyaltyTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.UsrCtrlMapAttendeesToQuantityBtnProductQtySelectedTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.UsrCtrlMapAttendeeToProductBtnProductTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.UsrCtrlMapAttendeeToProductBtnProductHighlightTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.MapAttendeesMappedCustomerNameTextForeColor = Color.Black;
            }
            log.LogMethodExit();

        }

        private static void setDefaultTextForeColor()
        {
            log.LogMethodEntry();
            KioskStatic.CurrentTheme.SelectChildMemberDetailsGridTextForeColor =
            KioskStatic.CurrentTheme.SelectChildYourFamilyGridTextForeColor =
            KioskStatic.CurrentTheme.EnterChildDetailsGridTextForeColor =
            KioskStatic.CurrentTheme.EnterAdultDetailsGridTextForeColor =
            KioskStatic.CurrentTheme.CardGamesdgvCardGamesForeColor =
            KioskStatic.CurrentTheme.CardGamesdgvCardGamesHeaderForeColor =
            KioskStatic.CurrentTheme.CardCountBasicsBtnOneTextForeColor =
            KioskStatic.CurrentTheme.CardCountBasicsBtnTwoTextForeColor =
            KioskStatic.CurrentTheme.CardCountBasicsBtnThreeTextForeColor =
            KioskStatic.CurrentTheme.TicketTypeOption1TextForeColor =
            KioskStatic.CurrentTheme.TicketTypeOption2TextForeColor =
            KioskStatic.CurrentTheme.CalenderGridTextForeColor =
            KioskStatic.CurrentTheme.ComboChildProductsQtyQuantityTextForeColor =
            KioskStatic.CurrentTheme.ComboChildProductsQtyProductButtonTextForeColor =
            KioskStatic.CurrentTheme.FrmGetEmailDetailsTxtCustomerEmailTextForeColor =
            KioskStatic.CurrentTheme.FrmGetEmailDetailsTxtUserEntryEmailTextForeColor =
            KioskStatic.CurrentTheme.FSKCoverPageBtnExecuteOnlineTransactionTextForeColor =
            KioskStatic.CurrentTheme.PreSelectPaymentModeBtnTextForeColor =
            KioskStatic.CurrentTheme.HomeScreenLanguageListForeColor =
            KioskStatic.CurrentTheme.CustomerScreenInfoTextForeColor =
            KioskStatic.CurrentTheme.CustomerScreenDgvLinkedRelationsHeaderTextForeColor =

            KioskStatic.CurrentTheme.RedeemTokensScreenAvialableTokensTextForeColor =

            KioskStatic.CurrentTheme.frmUsrCtltPnlWaiversTextForeColor =
            KioskStatic.CurrentTheme.frmUsrCtltPnlWaiverSetTextForeColor =
            KioskStatic.CurrentTheme.FrmEntitlementBtnMsgTextForeColor =
            KioskStatic.CurrentTheme.FrmEntitlementBtnPointsTextForeColor =

            KioskStatic.CurrentTheme.SelectAdultYourFamilyGridTextForeColor =
            KioskStatic.CurrentTheme.SelectAdultMemberDetailsGridTextForeColor =
            KioskStatic.CurrentTheme.SelectAdultGridHeaderTextForeColor =
            KioskStatic.CurrentTheme.EnterAdultDetailsGridHeaderTextForeColor =
            KioskStatic.CurrentTheme.SelectAdultYourFamilyGridInfoTextForeColor =
            KioskStatic.CurrentTheme.SelectAdultMemberDetailsGridInfoTextForeColor =

            KioskStatic.CurrentTheme.SelectChildGridHeaderTextForeColor =
            KioskStatic.CurrentTheme.EnterChildDetailsGridHeaderTextForeColor =
            KioskStatic.CurrentTheme.SelectChildYourFamilyGridInfoTextForeColor =
            KioskStatic.CurrentTheme.SelectChildMemberDetailsGridInfoTextForeColor =

            KioskStatic.CurrentTheme.KioskActivityDgvKioskActivityTextForeColor =
            KioskStatic.CurrentTheme.KioskActivityDgvKioskActivityHeaderTextForeColor =
            KioskStatic.CurrentTheme.KioskActivityDgvKioskActivityInfoTextForeColor =

            KioskStatic.CurrentTheme.CalenderItemTextForeColor =

            KioskStatic.CurrentTheme.ScanCouponScreenCouponInfoTextForeColor =

            KioskStatic.CurrentTheme.FrmPrintTransDgvTransactionHeaderTextForeColor =
            KioskStatic.CurrentTheme.FrmPrintTransDgvPendingPrintTransactionLinesTextForeColor =
            KioskStatic.CurrentTheme.FrmPrintTransDgvPrintedTransactionLinesTextForeColor =
            KioskStatic.CurrentTheme.FrmPrintTransCmbPrintReasonTextForeColor =
            KioskStatic.CurrentTheme.FrmPrintTransDgvTransactionHeaderControlsTextForeColor =
            KioskStatic.CurrentTheme.FrmPrintTransDgvPrintedTransactionLinesControlsTextForeColor =
            KioskStatic.CurrentTheme.FrmPrintTransDgvPendingPrintTransactionLinesControlsTextForeColor =
            KioskStatic.CurrentTheme.FrmPrintTransDgvTransactionHeaderHeaderTextForeColor =
            KioskStatic.CurrentTheme.FrmPrintTransDgvPendingPrintTransactionLinesHeaderTextForeColor =
            KioskStatic.CurrentTheme.FrmPrintTransDgvPrintedTransactionLinesHeaderTextForeColor =

            KioskStatic.CurrentTheme.CardTransactionSummaryInfoTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionDepositeInfoTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionDiscountInfoTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionFundInfoTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionDonationInfoTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionFooterTextForeColor =

            KioskStatic.CurrentTheme.FrmLoyaltyTxtPhoneNoTextForeColor =
            KioskStatic.CurrentTheme.FrmLoyaltyTxtFirstNameTextForeColor =
            KioskStatic.CurrentTheme.FrmLoyaltyListboxNamesTextForeColor =

            KioskStatic.CurrentTheme.PaymentModeDiscountInfoTextForeColor =
            KioskStatic.CurrentTheme.PaymentModeDepositeInfoTextForeColor =
            KioskStatic.CurrentTheme.PaymentModeSummaryInfoTextForeColor =

            KioskStatic.CurrentTheme.AddCustomerRelationGridTextForeColor =
            KioskStatic.CurrentTheme.AddCustomerRelationGridHeaderTextForeColor =

            KioskStatic.CurrentTheme.AgeGateScreenMonthTextBoxTextForeColor =
            KioskStatic.CurrentTheme.AgeGateScreenDateTextBoxTextForeColor =
            KioskStatic.CurrentTheme.AgeGateScreenYearTextBoxTextForeColor =

            KioskStatic.CurrentTheme.LoadBonusCardInfoTextForeColor =
            KioskStatic.CurrentTheme.LoadBonusBonusInfoTextForeColor =
            KioskStatic.CurrentTheme.PauseTimeCardNumberInfoTextForeColor =
            KioskStatic.CurrentTheme.PauseTimeTimeInfoTextForeColor =

            KioskStatic.CurrentTheme.FskExecuteInfoTextForeColor =
            KioskStatic.CurrentTheme.CreditsToTimeDetailsInfoTextForeColor =
            KioskStatic.CurrentTheme.CreditsToTimeAfterSaveInfoTextForeColor =

            KioskStatic.CurrentTheme.FrmKioskTransViewTxtFromTimeHrsTextForeColor =
            KioskStatic.CurrentTheme.FrmKioskTransViewTxtFromTimeMinsTextForeColor =
            KioskStatic.CurrentTheme.FrmKioskTransViewCmbFromTimeTTTextForeColor =
            KioskStatic.CurrentTheme.FrmKioskTransViewTxtToTimeHrsTextForeColor =
            KioskStatic.CurrentTheme.FrmKioskTransViewTxtToTimeMinsTextForeColor =
            KioskStatic.CurrentTheme.FrmKioskTransViewCmbToTimeTTTextForeColor =
            KioskStatic.CurrentTheme.FrmKioskTransViewTxtTrxIdTextForeColor =
            KioskStatic.CurrentTheme.FrmKioskTransViewCmbPosMachinesTextForeColor =
            KioskStatic.CurrentTheme.FrmKioskTransViewDgvKioskTransactionsTextForeColor =
            KioskStatic.CurrentTheme.FrmKioskTransViewDgvColumnHeadersTextForeColor =

            KioskStatic.CurrentTheme.FskExecuteOnlineTxtTransactionOTPTextForeColor =
            KioskStatic.CurrentTheme.FskExecuteOnlinePnlTrasactionIdTextForeColor =
            KioskStatic.CurrentTheme.FskExecuteOnlineTransactionHeaderInfoTextForeColor =
            KioskStatic.CurrentTheme.FskExecuteOnlineTransactionLinesInfoTextForeColor =
            KioskStatic.CurrentTheme.FrmCustWaiverDgvCustomerTextForeColor =
            KioskStatic.CurrentTheme.FrmCustOTPTxtOTPTextForeColor =
            KioskStatic.CurrentTheme.FrmCustSignConfirmationDgvCustomerSignedWaiverTextForeColor =
            KioskStatic.CurrentTheme.FrmCustSignConfirmationDgvCustomerSignedWaiverHeaderTextForeColor =
            KioskStatic.CurrentTheme.FrmGetCustInputTxtEmailTextForeColor =
            KioskStatic.CurrentTheme.FrmLinkCustCmbRelationTextForeColor =
            KioskStatic.CurrentTheme.FrmSelectWaiverTxtReservationCodeTextForeColor =
            KioskStatic.CurrentTheme.FrmSelectWaiverTxtTrxOTPTextForeColor =
            KioskStatic.CurrentTheme.FrmSelectWaiverDgvWaiverSetTextForeColor =
            KioskStatic.CurrentTheme.FrmSignWaiversFpnlWaiverSetTextForeColor =
            KioskStatic.CurrentTheme.FrmViewWaiverUIWebBrowserTextForeColor =
            KioskStatic.CurrentTheme.CustomerDashboardInfoTextForeColor =
            KioskStatic.CurrentTheme.LoadBonusCreditsInfoTextForeColor =

            KioskStatic.CurrentTheme.PauseTimeETicketInfoTextForeColor =

            KioskStatic.CurrentTheme.FrmCustOptionBtnNewRegistrationTextForeColor =
            KioskStatic.CurrentTheme.FrmCustOptionBtnExistingCustomerTextForeColor =

            KioskStatic.CurrentTheme.UsrCtrlCartLblTotalPriceTextForeColor =
            KioskStatic.CurrentTheme.UsrCtrlCartLblProductDescriptionTextForeColor =

            KioskStatic.CurrentTheme.PrintSummaryComboTextForeColor =
            KioskStatic.CurrentTheme.PrintSummaryLblFromDateTimeForeColor =
            KioskStatic.CurrentTheme.PrintSummaryLblToDateTimeForeColor =

            KioskStatic.CurrentTheme.TextBoxTxtDataTextForeColor =

            KioskStatic.CurrentTheme.AttractionQtyTxtQtyTextForeColor =
            KioskStatic.CurrentTheme.UsrCtrlComboDetailsProductNameTextForeColor =
            KioskStatic.CurrentTheme.UsrCtrlComboDetailsNumberTextForeColor =
            KioskStatic.CurrentTheme.AttractionQtyLblYourSelectionsTextForeColor =
            KioskStatic.CurrentTheme.AttractionQtyHeadersTextForeColor =
            KioskStatic.CurrentTheme.AttractionQtyValuesTextForeColor =
            KioskStatic.CurrentTheme.UsrCtrlSlotLblSlotInfo =
            KioskStatic.CurrentTheme.UsrCtrlSlotLblScheduleTime =
            KioskStatic.CurrentTheme.UsrCtrlAttrcationSummaryLblProductNameTextForeColor =
            KioskStatic.CurrentTheme.UsrCtrlAttrcationSummaryLblSlotDetailsTextForeColor =
            KioskStatic.CurrentTheme.CardSaleOptionLblNewCardTextForeColor =
            KioskStatic.CurrentTheme.CardSaleOptionLblExistingCardTextForeColor =
            KioskStatic.CurrentTheme.SelectSlotBtnDatesTextForeColor =

            KioskStatic.CurrentTheme.SearchCustomerTxtEmailIdForeColor =
            KioskStatic.CurrentTheme.SearchCustomerTxtPhoneNumForeColor =
            KioskStatic.CurrentTheme.UsrCtrlSelectCustomerLblFirstNameTextForeColor =
            KioskStatic.CurrentTheme.UsrCtrlSelectCustomerLblLastNameTextForeColor =
            KioskStatic.CurrentTheme.SelectCustomerLblLastNameTextForeColor = 
            KioskStatic.CurrentTheme.SelectCustomerLblFirstNameTextForeColor =
            KioskStatic.CurrentTheme.MapAttendeesLblQuantityTextForeColor =
            KioskStatic.CurrentTheme.MapAttendeesLblAssignParticipantsTextForeColor =

            KioskStatic.CurrentTheme.UsrCtrlMapAttendeesToQuantityLblQtyTextForeColor =
            KioskStatic.CurrentTheme.UsrCtrlMapAttendeesToQuantityLblQtySelectedTextForeColor = 

            KioskStatic.CurrentTheme.UsrCtrlCustomerNameTextForeColor =

            KioskStatic.CurrentTheme.CustomerRelationsRelatedCustomerNameTextForeColor = 
            KioskStatic.CurrentTheme.CustomerRelationsRelationshipTextForeColor = 
            KioskStatic.CurrentTheme.CustomerRelationsSignStatusTextForeColor = 
            KioskStatic.CurrentTheme.CustomerRelationsValidityTextForeColor =

            KioskStatic.CurrentTheme.UsrCtrlFilteredCustomersCustomerNameTextForeColor = CurrentTheme.TextForeColor;
            KioskStatic.CurrentTheme.SelectCustomerLblLastNameTextForeColor =
            KioskStatic.CurrentTheme.SelectCustomerLblFirstNameTextForeColor = CurrentTheme.TextForeColor;

            if (CurrentTheme.TextForeColor == Color.White)
            {
                KioskStatic.CurrentTheme.EnterChildDetailsGridTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.SelectChildYourFamilyGridTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.SelectChildMemberDetailsGridTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.EnterAdultDetailsGridTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.TicketTypeOption1TextForeColor = Color.Black;
                KioskStatic.CurrentTheme.TicketTypeOption2TextForeColor = Color.Black;
                KioskStatic.CurrentTheme.CalenderGridTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.ComboChildProductsQtyQuantityTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.ComboChildProductsQtyProductButtonTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.HomeScreenLanguageListForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmKioskTransViewTxtFromTimeHrsTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmKioskTransViewTxtFromTimeMinsTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmKioskTransViewCmbFromTimeTTTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmKioskTransViewTxtToTimeHrsTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmKioskTransViewTxtToTimeMinsTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmKioskTransViewCmbToTimeTTTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmKioskTransViewTxtTrxIdTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmKioskTransViewCmbPosMachinesTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmKioskTransViewDgvKioskTransactionsTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmKioskTransViewDgvColumnHeadersTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.SelectAdultYourFamilyGridTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.SelectAdultMemberDetailsGridTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.SelectAdultGridHeaderTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.SelectAdultYourFamilyGridInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.SelectAdultMemberDetailsGridInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.SelectChildGridHeaderTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.SelectChildYourFamilyGridInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.SelectChildMemberDetailsGridInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.EnterChildDetailsGridHeaderTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.EnterAdultDetailsGridHeaderTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.CreditsToTimeDetailsInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.CreditsToTimeAfterSaveInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.RedeemTokensScreenAvialableTokensTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmEntitlementBtnMsgTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmEntitlementBtnPointsTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.KioskActivityDgvKioskActivityTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.KioskActivityDgvKioskActivityHeaderTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.KioskActivityDgvKioskActivityInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.frmUsrCtltPnlWaiversTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.frmUsrCtltPnlWaiverSetTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmPrintTransDgvTransactionHeaderTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmPrintTransDgvPendingPrintTransactionLinesTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmPrintTransDgvPrintedTransactionLinesTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmPrintTransCmbPrintReasonTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmPrintTransDgvTransactionHeaderHeaderTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmPrintTransDgvPendingPrintTransactionLinesHeaderTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmPrintTransDgvPrintedTransactionLinesHeaderTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.CardTransactionSummaryInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmLoyaltyTxtPhoneNoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmLoyaltyTxtFirstNameTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmLoyaltyListboxNamesTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.CardTransactionDepositeInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.CardTransactionDiscountInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.CardTransactionFundInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.CardTransactionDonationInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.PaymentModeDepositeInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.PaymentModeDiscountInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.AddCustomerRelationGridTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.AddCustomerRelationGridHeaderTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.CustomerDashboardInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.LoadBonusCardInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.LoadBonusCreditsInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.LoadBonusBonusInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.PauseTimeCardNumberInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.PauseTimeTimeInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.PauseTimeETicketInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmCustWaiverDgvCustomerTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmCustOTPTxtOTPTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmCustSignConfirmationDgvCustomerSignedWaiverTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmCustSignConfirmationDgvCustomerSignedWaiverHeaderTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmGetCustInputTxtEmailTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmLinkCustCmbRelationTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmSelectWaiverTxtReservationCodeTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmSelectWaiverTxtTrxOTPTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmSelectWaiverDgvWaiverSetTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmViewWaiverUIWebBrowserTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.PaymentModeSummaryInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FskExecuteOnlineTxtTransactionOTPTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FskExecuteOnlinePnlTrasactionIdTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FskExecuteInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FskExecuteOnlineTransactionHeaderInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FskExecuteOnlineTransactionLinesInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.CustomerScreenInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.CustomerScreenDgvLinkedRelationsHeaderTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.ScanCouponScreenCouponInfoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmPrintTransDgvTransactionHeaderControlsTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmPrintTransDgvPrintedTransactionLinesControlsTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmPrintTransDgvPendingPrintTransactionLinesControlsTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.CalenderItemTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmLoyaltyTxtFirstNameTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmLoyaltyTxtPhoneNoTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.UsrCtrlCartLblTotalPriceTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.UsrCtrlCartLblProductDescriptionTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.AgeGateScreenMonthTextBoxTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.AgeGateScreenDateTextBoxTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.AgeGateScreenYearTextBoxTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmGetEmailDetailsTxtCustomerEmailTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.FrmGetEmailDetailsTxtUserEntryEmailTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.CardGamesdgvCardGamesForeColor = Color.Black;
                KioskStatic.CurrentTheme.CardGamesdgvCardGamesHeaderForeColor = Color.Black;
                KioskStatic.CurrentTheme.PrintSummaryLblFromDateTimeForeColor = Color.DarkOrchid;
                KioskStatic.CurrentTheme.PrintSummaryLblToDateTimeForeColor = Color.DarkOrchid;
                KioskStatic.CurrentTheme.PrintSummaryComboTextForeColor = Color.DarkOrchid;
                KioskStatic.CurrentTheme.TextBoxTxtDataTextForeColor = Color.DarkOrchid;
                KioskStatic.CurrentTheme.AttractionQtyTxtQtyTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.UsrCtrlComboDetailsNumberTextForeColor = Color.DarkOrchid;
                KioskStatic.CurrentTheme.AttractionQtyLblYourSelectionsTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.AttractionQtyHeadersTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.AttractionQtyValuesTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.UsrCtrlSlotLblSlotInfo = Color.DarkOrchid;
                KioskStatic.CurrentTheme.UsrCtrlSlotLblScheduleTime = Color.DarkOrchid;
                KioskStatic.CurrentTheme.CardSaleOptionLblNewCardTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.CardSaleOptionLblExistingCardTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.SelectSlotBtnDatesTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.SearchCustomerTxtEmailIdForeColor = Color.Black;
                KioskStatic.CurrentTheme.SearchCustomerTxtPhoneNumForeColor = Color.Black;
                KioskStatic.CurrentTheme.UsrCtrlSelectCustomerLblFirstNameTextForeColor =
                KioskStatic.CurrentTheme.UsrCtrlSelectCustomerLblLastNameTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.MapAttendeesLblQuantityTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.MapAttendeesLblAssignParticipantsTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.UsrCtrlMapAttendeesToQuantityLblQtyTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.UsrCtrlCustomerNameTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.CustomerRelationsRelatedCustomerNameTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.CustomerRelationsRelationshipTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.CustomerRelationsSignStatusTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.CustomerRelationsValidityTextForeColor = Color.Black;
                KioskStatic.CurrentTheme.UsrCtrlFilteredCustomersCustomerNameTextForeColor = Color.Black;
            }
            log.LogMethodExit();
        }

        public static Image GetImage(Image img, string file, string fileFolder)
        {
            log.LogMethodEntry(file, fileFolder);
            string fullName = string.Empty;
            try
            {
                DirectoryInfo imageDIR = new DirectoryInfo(string.IsNullOrWhiteSpace(fileFolder) ? Application.StartupPath + @"\Media\Images\" : fileFolder);
                FileInfo[] filesInDir = imageDIR.GetFiles(file + ".*", SearchOption.TopDirectoryOnly);

                if (filesInDir != null && filesInDir.Length > 0)
                {
                    fullName = filesInDir[0].FullName;
                    Console.WriteLine(fullName);
                    img = Image.FromFile(fullName);
                    if (filesInDir.Length > 1)
                    {
                        KioskStatic.logToFile("More than one file (" + filesInDir.Length + ") found with name " + file + " proceeding with first file.");
                        log.Info("More than one file (" + filesInDir.Length + ") found with name " + file + " proceeding with first file.");

                    }
                }
                else
                {
                    KioskStatic.logToFile("Image file " + file + " is not found in " + imageDIR.FullName + " folder. Proceeding with default image.");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error fetching image file " + (string.IsNullOrWhiteSpace(fullName) ? file : fullName) + ": " + ex.Message);
            }
            log.LogMethodExit();
            return img;
        }

        public static void setFieldLabelForeColor(Control c)
        {
            if (c.HasChildren)
            {
                foreach (Control chld in c.Controls)
                {
                    if (chld.GetType().ToString().ToLower().Contains("label"))
                        chld.ForeColor = KioskStatic.CurrentTheme.FieldLabelForeColor;
                }
            }
            else
                c.ForeColor = KioskStatic.CurrentTheme.FieldLabelForeColor;
        }

        public static object getProductExternalSystemReference(int ProductId)
        {
            return Utilities.executeScalar(@"select CustomDataNumber 
                                            from CustomDataView v, products p
                                            where p.CustomDataSetId = v.CustomDataSetId
                                            and v.Name = 'External System Identifier'
                                            and p.product_id = @productId",
                                        new SqlParameter("@productId", ProductId));
        }

        static string getConfigValue(string ConfigName)
        {
            return ParafaitDefaultContainerList.GetParafaitDefault(KioskStatic.Utilities.ExecutionContext, configDictionary[ConfigName]);
        }

        public static Color MessageLineBackColor = Color.Transparent;
        public static void formatMessageLine(Control inLabel, Font Font, Image BackgroundImage)
        {
            inLabel.BackColor = MessageLineBackColor;
            if (inLabel.GetType().ToString().ToLower().Contains("button"))
            {
                Button b = inLabel as Button;
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.MouseDownBackColor = b.FlatAppearance.MouseOverBackColor = MessageLineBackColor;

                if (Font != null)
                    b.Font = Font;

                if (BackgroundImage != null)
                {
                    b.BackgroundImage = BackgroundImage;
                    b.BackgroundImageLayout = ImageLayout.Stretch;
                }

                (inLabel as Button).AutoSize = true;
                int btnWidth = (inLabel as Button).Width;
                int btnHeight = (inLabel as Button).Height;
                (inLabel as Button).AutoEllipsis = true;
                (inLabel as Button).AutoSize = false;
                if ((inLabel as Button).Width != btnWidth || (inLabel as Button).Height != btnHeight)
                {
                    (inLabel as Button).Size = new Size(btnWidth, btnHeight);
                }
            }
        }

        public static void formatMessageLine(Control inLabel)
        {
            formatMessageLine(inLabel, null, null);
        }

        public static void formatMessageLine(Control inLabel, Font Font)
        {
            formatMessageLine(inLabel, Font, null);
        }

        public static void formatMessageLine(Control inLabel, float FontSize)
        {
            formatMessageLine(inLabel, new Font(inLabel.Font.Name, FontSize), null);
        }

        public static void formatMessageLine(Control inLabel, float FontSize, Image BackGroundImage)
        {
            formatMessageLine(inLabel, new Font(inLabel.Font.Name, FontSize), BackGroundImage);
        }

        public static void setDefaultFont(Control parentControl)
        {
            try
            {
                foreach (Control c in parentControl.Controls)
                {
                    if (c.HasChildren)
                    {
                        if (c.Font.Name.Equals(CurrentTheme.DefaultFont.Name, StringComparison.CurrentCultureIgnoreCase) == false)
                            c.Font = new Font(CurrentTheme.DefaultFont.Name, c.Font.Size, c.Font.Style);
                        c.ForeColor = CurrentTheme.ThemeForeColor;//Modification on 17-Dec-2015 for introducing new theme
                        setDefaultFont(c);
                    }
                    else
                    {
                        if (c.Font.Name.Equals(CurrentTheme.DefaultFont.Name, StringComparison.CurrentCultureIgnoreCase) == false)
                            c.Font = new Font(CurrentTheme.DefaultFont.Name, c.Font.Size, c.Font.Style);
                        c.ForeColor = CurrentTheme.ThemeForeColor;//Modification on 17-Dec-2015 for introducing new theme
                    }
                }
            }
            catch { }
        }

        public static double GetCreditsOnSplitVariableProduct(double Amount, string selectedEntitlementType, Card currentCard)
        {
            string message = "";
            try
            {
                Semnox.Parafait.Transaction.Transaction CurrentTrx = new Semnox.Parafait.Transaction.Transaction(KioskStatic.Utilities);

                string productType = currentCard == null ? "NEW" : "RECHARGE";

                if (currentCard == null)
                    currentCard = new Card("ABCDEFGH", "", KioskStatic.Utilities);

                if (selectedEntitlementType == "Time")
                {
                    productType = "GAMETIME";
                }
                message = "";

                if (!KioskStatic.SPLIT_AND_MAP_VARIABLE_PRODUCT)
                {
                    CurrentTrx.createTransactionLine(currentCard, KioskStatic.Utilities.ParafaitEnv.CardDepositProductId, 0, 1, ref message);
                    CurrentTrx.createTransactionLine(currentCard, KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId, Amount, 1, ref message);
                }
                else
                {
                    int newCardCount = 0;
                    string productTypeLocal = productType;
                    while (Amount > 0)
                    {
                        if (productType == "NEW")
                        {
                            if (newCardCount != 0)
                            {
                                productTypeLocal = "RECHARGE";
                            }
                            newCardCount = 1;
                        }
                        ProductsList productsList = new ProductsList(KioskStatic.Utilities.ExecutionContext);
                        List<ProductsDTO> productsDTOList = productsList.getSplitProductList(Amount, productTypeLocal);
                        if (productsDTOList != null && productsDTOList.Count > 0)
                        {
                            int prodId = productsDTOList[0].ProductId;
                            double price = Convert.ToInt32(productsDTOList[0].Price);
                            if (CurrentTrx.createTransactionLine(currentCard, prodId, price, 1, ref message) != 0)
                            {
                                log.LogMethodExit(-1);
                                return -1;
                            }
                            Amount -= price;
                        }
                        else
                        {
                            CurrentTrx.createTransactionLine(currentCard, KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId, Amount, 1, ref message);
                            break;
                        }
                    }
                }

                double credits = 0;
                foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine tl in CurrentTrx.TrxLines)
                {
                    credits += tl.Credits + tl.Bonus;
                }
                log.LogMethodExit(credits);
                return credits;
            }
            catch (Exception ex)
            {
                logToFile("GetCreditsOnSplitVariableProduct: " + ex.Message);
                log.LogMethodExit();
                return 0;
            }
        }

        public static void InitializeFiscalPrinter()
        {
            Device.Printer.FiscalPrint.FiscalPrinterFactory.GetInstance().Initialize(Utilities);
            string _FISCAL_PRINTER = Utilities.getParafaitDefaults("FISCAL_PRINTER");
            bool unAttendedMode = true;
            fiscalPrinter = Device.Printer.FiscalPrint.FiscalPrinterFactory.GetInstance().GetFiscalPrinter(_FISCAL_PRINTER, unAttendedMode);
            try
            {
                fiscalPrinter.OpenPort();
            }
            catch (Exception ex)
            {
                logToFile(ex.Message);
            }
        }
        public static List<LookupValuesDTO> LoadPromotionModeDropDown(ComboBox comboBox)
        {
            List<LookupValuesDTO> lookupValuesDTOList = null;
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(KioskStatic.Utilities.ExecutionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchMemberParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                searchMemberParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "PROMOTION_MODES"));
                searchMemberParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
                lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchMemberParams);
                if (lookupValuesDTOList == null)
                {
                    lookupValuesDTOList = new List<LookupValuesDTO>();
                }
                lookupValuesDTOList.Insert(0, new LookupValuesDTO());
                lookupValuesDTOList[0].LookupValue = string.Empty;
                lookupValuesDTOList[0].Description = "None";
                comboBox.DataSource = lookupValuesDTOList;
                comboBox.ValueMember = "LookupValue";
                comboBox.DisplayMember = "Description";

            }
            catch (Exception ex)
            {
                logToFile("Error occurred while loading lookupValues list" + ex.Message);
                log.Error(ex);
            }
            return lookupValuesDTOList;
        }
        public static void CloseFiscalPrinter()
        {
            if (fiscalPrinter != null)
                fiscalPrinter.ClosePort();

        }

        /// <summary>
        /// Returns the random card number 
        /// </summary>
        /// <returns></returns>
        public static string GetTagNumber()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Generating new card number");
            RandomTagNumber randomTagNumber = new RandomTagNumber(Utilities.ExecutionContext);
            string autoGeneratedCardNumber = randomTagNumber.Value;
            KioskStatic.logToFile("Generating new card number" + autoGeneratedCardNumber);
            log.LogMethodExit(autoGeneratedCardNumber);
            return autoGeneratedCardNumber;
        }

        /// <summary>
        /// Returns true for the product if auto generate card number is set to 'Y' or to be printed on RFID printer else returns false
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="rfidPrinterDTO"></param>
        /// <returns></returns>
        public static bool IsWristBandPrintTag(int productId, POSPrinterDTO rfidPrinterDTO)
        {
            log.LogMethodEntry(productId, rfidPrinterDTO);
            KioskStatic.logToFile("ProductId" + productId);
            bool isWristBandPrintTag = false;
            DataTable dt = getProductDetails(productId);
            if (dt.Rows.Count > 0)
            {
                isWristBandPrintTag = dt.Rows[0]["AutoGenerateCardNumber"].ToString().Trim() == "Y" ? true : false;

                if (isWristBandPrintTag == false && rfidPrinterDTO != null
                    && rfidPrinterDTO.PrinterDTO != null && rfidPrinterDTO.PrinterDTO.PrintableProductIds != null
                    && rfidPrinterDTO.PrinterDTO.PrintableProductIds.Any())
                {
                    isWristBandPrintTag = rfidPrinterDTO.PrinterDTO.PrintableProductIds.Exists(ppId => ppId == productId);
                }
            }
            log.LogMethodExit(isWristBandPrintTag);
            KioskStatic.logToFile("Is Wrist band Print Tag: " + isWristBandPrintTag);
            return isWristBandPrintTag;
        }

        /// <summary>
        /// Gets all the included PaymentModes
        /// </summary>
        public static void getPaymentModes()
        {
            log.LogMethodEntry();
            POSMachines pOSMachines = new POSMachines(Utilities.ExecutionContext, KioskStatic.Utilities.ParafaitEnv.POSMachineId, true, true);
            POSMachineDTO = pOSMachines.POSMachineDTO;
            if (POSMachineDTO != null && POSMachineDTO.POSPaymentModeInclusionDTOList != null && POSMachineDTO.POSPaymentModeInclusionDTOList.Any())
            {
                pOSPaymentModeInclusionDTOList = POSMachineDTO.POSPaymentModeInclusionDTOList.FindAll(p => p.PaymentModeDTO.IsCash
                                                                                      || p.PaymentModeDTO.IsDebitCard
                                                                                      || (p.PaymentModeDTO.IsCreditCard && p.PaymentModeDTO.Gateway > -1 && !(ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "FISCAL_PRINTER").Equals(FiscalPrinters.Smartro.ToString())))
                                                                                      || (p.PaymentModeDTO.IsCreditCard && p.PaymentModeDTO.Gateway == -1 && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "FISCAL_PRINTER").Equals(FiscalPrinters.Smartro.ToString())));
            }
            if (POSMachineDTO != null)
            {
                POSMachineDTO.PosPrinterDtoList = pOSMachines.PopulatePrinterDetails();
            }
            if (pOSPaymentModeInclusionDTOList == null)
            {
                pOSPaymentModeInclusionDTOList = new List<POSPaymentModeInclusionDTO>();
            }
            log.LogMethodExit(pOSPaymentModeInclusionDTOList);
        }

        /// <summary>
        /// initializes PaymentModes
        /// </summary>
        static void initializePaymentModes()
        {
            if (pOSPaymentModeInclusionDTOList != null && pOSPaymentModeInclusionDTOList.Any())
            {
                foreach (POSPaymentModeInclusionDTO pOSPaymentModeInclusionDTO in pOSPaymentModeInclusionDTOList)
                {
                    try
                    {
                        PaymentGatewayFactory.GetInstance().Initialize(Utilities, true, null, KioskStatic.KioskActivityWriteToLog);
                        PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(pOSPaymentModeInclusionDTO.PaymentModeDTO.GatewayLookUp);
                        paymentGateway.Initialize();
                    }
                    catch (Exception ex)
                    {
                        log.Error("Payment Gateway error:" + ex.Message);
                        errorPayentModeDTOList.Add(pOSPaymentModeInclusionDTO);
                    }
                }
            }
        }

        /// <summary>
        /// gets All PaymentModes
        /// </summary>
        public static void getAllPaymentModes()
        {
            log.LogMethodEntry();
            PaymentModeDatahandler paymentModeDH = new PaymentModeDatahandler();
            List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> PaymentModesParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
            PaymentModesParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
            PaymentModesParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
            paymentModeDTOList = paymentModeDH.GetPaymentModeList(PaymentModesParameters);
            log.LogMethodExit(paymentModeDTOList);
        }


        /// <summary>
        /// Get entitlement message details
        /// </summary>
        /// <param name="prowProduct"></param>
        /// <param name="cardCount"></param>
        /// <param name="selectedEntitlementType"></param>
        /// <param name="multipleCardsInSingleProduct"></param>
        /// <returns></returns>
        public static string GetEntitlementsMessage(DataRow prowProduct, int cardCount,
                                                        string selectedEntitlementType, bool multipleCardsInSingleProduct, Card currentCard = null)
        {
            log.LogMethodEntry(prowProduct, cardCount, selectedEntitlementType);
            double credits = 0;
            int finalCardCount = 0;
            string message = string.Empty;

            decimal ProductPrice = Math.Round(Convert.ToDecimal(prowProduct["price"] == DBNull.Value ? 0 : prowProduct["price"]), 4, MidpointRounding.AwayFromZero);
            if (multipleCardsInSingleProduct)
            {
                finalCardCount = (cardCount == 0 ? 1 : cardCount);
            }
            else
            {
                finalCardCount = 1;
            }
            if (prowProduct["product_type"].ToString().Equals("VARIABLECARD"))
            {
                credits = KioskStatic.GetCreditsOnSplitVariableProduct((double)ProductPrice, selectedEntitlementType, currentCard);
            }
            else
            {
                credits = Convert.ToInt32(prowProduct["Credits"]);
            }
            double timeCredits = 0;
            timeCredits = Convert.ToInt32(prowProduct["TimePoint"]);
            double loyaltyPoints = 0;
            loyaltyPoints = Convert.ToInt32(prowProduct["LoyaltyPoints"]);
            double tickets = 0;
            tickets = Convert.ToInt32(prowProduct["Tickets"]);
            if (selectedEntitlementType == "Credits")
            {
                message += GetEntitlementDetailsMessage(cardCount, credits, finalCardCount, timeCredits, loyaltyPoints, tickets);
            }
            else
            {
                double creditMultiplicationFactor = 1;
                if (KioskStatic.TIME_IN_MINUTES_PER_CREDIT > 0 && prowProduct["product_type"].ToString().Equals("VARIABLECARD"))
                {
                    creditMultiplicationFactor = KioskStatic.TIME_IN_MINUTES_PER_CREDIT;
                    double variableTime = 0;
                    variableTime = credits * creditMultiplicationFactor;
                    message += GetEntitlementDetailsMessage(cardCount, 0, finalCardCount, timeCredits + variableTime, loyaltyPoints, tickets);
                }
                else
                {
                    message += GetEntitlementDetailsMessage(cardCount, credits, finalCardCount, timeCredits, loyaltyPoints, tickets);
                }
            }
            log.LogMethodExit(message);
            return message;
        }

        private static string GetEntitlementDetailsMessage(int cardCount, double credits, int finalCardCount, double timeCredits,
                                                            double loyaltyPoints, double tickets)
        {
            log.LogMethodEntry(cardCount, credits, finalCardCount, timeCredits, loyaltyPoints, tickets);

            string message = string.Empty;
            string appendAND = " " + MessageContainerList.GetMessage(Utilities.ExecutionContext, "and") + " ";
            string appendComma = ", ";
            bool appendANDString = false;
            bool appendCommaString = false;
            message = (cardCount <= 1 ? "1 " + MessageContainerList.GetMessage(Utilities.ExecutionContext, "Card")
                                     : cardCount.ToString() + " " + MessageContainerList.GetMessage(Utilities.ExecutionContext, "Cards")) + Environment.NewLine; // + "(";
            if (credits > 0 || timeCredits > 0 || loyaltyPoints > 0 || tickets > 0)
            {
                message += "(";
                if (credits != 0)
                {
                    message += Math.Round((credits / finalCardCount), 0) + " " + MessageContainerList.GetMessage(Utilities.ExecutionContext, "points");
                    appendANDString = true;
                    int counter = 0;
                    if (timeCredits != 0)
                    {
                        counter++;
                    }
                    if (loyaltyPoints != 0)
                    {
                        counter++;
                    }
                    if (tickets != 0)
                    {
                        counter++;
                    }
                    if (counter > 1)
                    {
                        appendCommaString = true;
                    }
                    else { appendCommaString = false; }
                }
                if (timeCredits != 0)
                {
                    message += (appendCommaString ? MessageContainerList.GetMessage(Utilities.ExecutionContext, appendComma) :
                        (appendANDString ? MessageContainerList.GetMessage(Utilities.ExecutionContext, appendAND) : ""))
                        + Math.Round((timeCredits / finalCardCount), 0) + " " +
                        MessageContainerList.GetMessage(Utilities.ExecutionContext, "minutes");
                    appendANDString = true;
                    if (loyaltyPoints != 0 && tickets != 0)
                    {
                        appendCommaString = true;
                    }
                    else { appendCommaString = false; }
                }
                if (loyaltyPoints != 0)
                {
                    message += (appendCommaString ? MessageContainerList.GetMessage(Utilities.ExecutionContext, appendComma) :
                        (appendANDString ? MessageContainerList.GetMessage(Utilities.ExecutionContext, appendAND) : ""))
                        + Math.Round((loyaltyPoints / finalCardCount), 0) + " "
                        + MessageContainerList.GetMessage(Utilities.ExecutionContext, "loyalty points");
                    appendANDString = true;
                    appendCommaString = false;
                }
                if (tickets != 0)
                {
                    message += (appendCommaString ? MessageContainerList.GetMessage(Utilities.ExecutionContext, appendComma) :
                        (appendANDString ? MessageContainerList.GetMessage(Utilities.ExecutionContext, appendAND) : ""))
                        + Math.Round((tickets / finalCardCount), 0) + " "
                        + MessageContainerList.GetMessage(Utilities.ExecutionContext, "tickets");

                }
                message += MessageContainerList.GetMessage(Utilities.ExecutionContext, " per card)");
            }
            log.LogMethodExit(message);
            return message;
        }
        public static POSPrinterDTO GetRFIDPrinter(ExecutionContext executionContext, int posMacineId)
        {
            log.LogMethodEntry();
            POSPrinterDTO rfidPrinterDTO = null;
            if (POSMachineDTO == null || POSMachineDTO.PosPrinterDtoList == null || POSMachineDTO.PosPrinterDtoList.Any() == false)
            {
                POSMachines posMachine = new POSMachines(executionContext, posMacineId);
                POSMachineDTO.PosPrinterDtoList = posMachine.PopulatePrinterDetails();
            }
            if (POSMachineDTO != null && POSMachineDTO.PosPrinterDtoList != null && POSMachineDTO.PosPrinterDtoList.Any())
            {
                rfidPrinterDTO = POSMachineDTO.PosPrinterDtoList.Find(pp => pp.IsActive && pp.PrinterDTO != null && pp.PrinterDTO.PrinterType == Printer.PrinterDTO.PrinterTypes.RFIDWBPrinter);
            }
            log.LogMethodExit(rfidPrinterDTO);
            return rfidPrinterDTO;
        }
        public static void ValidateRFIDPrinter(Semnox.Core.Utilities.ExecutionContext executionContext, int posMachineId, int SelectedproductId)
        {
            log.LogMethodEntry(executionContext, posMachineId);
            List<POSPrinterDTO> posPrinterDTOList = null;
            if (KioskStatic.POSMachineDTO != null && KioskStatic.POSMachineDTO.PosPrinterDtoList != null && KioskStatic.POSMachineDTO.PosPrinterDtoList.Any())
            {
                posPrinterDTOList = new List<POSPrinterDTO>(KioskStatic.POSMachineDTO.PosPrinterDtoList);
            }
            else
            {
                POSMachines posMachine = new POSMachines(executionContext, posMachineId);
                posPrinterDTOList = posMachine.PopulatePrinterDetails();
            }

            if (posPrinterDTOList != null && posPrinterDTOList.Any())
            {
                List<POSPrinterDTO> rfidPrinterDTOList = posPrinterDTOList.Where(ppp => ppp.PrinterDTO != null && ppp.PrinterDTO.PrinterType == Printer.PrinterDTO.PrinterTypes.RFIDWBPrinter).ToList();
                log.LogVariableState("rfidPrinterDTOList", rfidPrinterDTOList);
                if (rfidPrinterDTOList != null && rfidPrinterDTOList.Any())
                {
                    for (int i = 0; i < rfidPrinterDTOList.Count; i++)
                    {
                        POSPrinterDTO posPrinterDTO = rfidPrinterDTOList[i];
                        log.LogVariableState("posPrinterDTO", posPrinterDTO);
                        if (posPrinterDTO.PrinterDTO != null && posPrinterDTO.PrinterDTO.PrintableProductIds.Contains(SelectedproductId))
                        {
                            string writsbandModel = string.Empty;
                            string cardNumber = string.Empty;
                            WristBandPrinter wristBandPrinter = null;
                            LookupsContainerDTO lookupsContainerDTO = LookupsContainerList.GetLookupsContainerDTO(executionContext.GetSiteId(), "RFID_WRISTBAND_MODELS");
                            log.LogVariableState("lookupsContainerDTO", lookupsContainerDTO);
                            if (lookupsContainerDTO != null &&
                                 lookupsContainerDTO.LookupValuesContainerDTOList != null &&
                                 lookupsContainerDTO.LookupValuesContainerDTOList.Any()
                                 && posPrinterDTO.PrinterDTO.WBPrinterModel > -1)
                            {
                                writsbandModel = lookupsContainerDTO.LookupValuesContainerDTOList.Where(x => x.LookupValueId == posPrinterDTO.PrinterDTO.WBPrinterModel).FirstOrDefault().LookupValue;
                            }
                            wristBandPrinter = WristbandPrinterFactory.GetInstance(executionContext).GetWristBandPrinter(writsbandModel);
                            if (string.IsNullOrWhiteSpace(writsbandModel))
                                writsbandModel = "STIMA";
                            log.LogVariableState("writsbandModel", writsbandModel);
                            switch (writsbandModel)
                            {
                                case "STIMA":
                                    {
                                        wristBandPrinter.SetIPAddress(posPrinterDTO.PrinterDTO.IpAddress);
                                        log.Debug("cardNumber  STIMA:" + cardNumber);
                                    }
                                    break;
                                case "BOCA":
                                    {
                                        wristBandPrinter.SetPrinterName(posPrinterDTO.PrinterDTO.PrinterName);
                                        log.Debug("cardNumber  BOCA:" + cardNumber);
                                    }
                                    break;
                            }
                            try
                            {
                                wristBandPrinter.Open();
                                wristBandPrinter.CanPrint(executionContext);
                                cardNumber = wristBandPrinter.ReadRFIDTag();
                                if (string.IsNullOrWhiteSpace(cardNumber))
                                {
                                    throw new Exception(MessageContainerList.GetMessage(executionContext, 2890));
                                }
                                Semnox.Parafait.Customer.Accounts.AccountBL CheckCard = new Customer.Accounts.AccountBL(executionContext, cardNumber, false, false);
                                if (CheckCard != null && CheckCard.AccountDTO != null && CheckCard.GetAccountId() != -1)
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 3008, cardNumber));
                                    //"Wrist band# &1 on RFID printer is already issued. Please contact staff"
                                }
                            }
                            catch (ValidationException ex)
                            {
                                log.Error(ex);
                                KioskStatic.logToFile(ex.Message);
                                logger.Monitor kioskMonitor = new logger.Monitor();
                                kioskMonitor.Post(logger.Monitor.MonitorLogStatus.ERROR, ex.Message, "WRIST_BAND_PRINTER", "");
                                throw;
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                KioskStatic.logToFile(ex.Message);
                                logger.Monitor kioskMonitor = new logger.Monitor();
                                kioskMonitor.Post(logger.Monitor.MonitorLogStatus.ERROR, ex.Message, "WRIST_BAND_PRINTER", "");
                                throw new Exception(MessageContainerList.GetMessage(executionContext, 2890));
                            }
                            finally
                            {
                                try
                                {
                                    if (wristBandPrinter != null)
                                    {
                                        wristBandPrinter.Close();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex);
                                    log.Error("Error while closing the printer");
                                }
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        public static void GetDateMonthFormat()
        {
            log.LogMethodEntry();

            string dateFormat = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "DATE_FORMAT");
            try
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "IGNORE_CUSTOMER_BIRTH_YEAR"))
                {
                    if (dateFormat.StartsWith("Y", StringComparison.CurrentCultureIgnoreCase))
                    {
                        dateFormat = dateFormat.TrimStart('y', 'Y');
                        dateFormat = dateFormat.Substring(1);
                    }
                    else
                    {
                        int pos = dateFormat.IndexOf("Y", StringComparison.CurrentCultureIgnoreCase);
                        if (pos > 0)
                            dateFormat = dateFormat.Substring(0, pos - 1);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile(ex.Message);
            }

            DateMonthFormat = dateFormat;
            log.LogMethodExit(dateFormat);
        } 
       
        private static RichContentDTO GetTermsAndConditionsFile(ApplicationContentModule ApplicationContentModule)
        {
            log.LogMethodEntry();
            try
            {
                int richcontentId = -1;
                List<RichContentDTO> richContentDTOList = new List<RichContentDTO>();
                List<ApplicationContentDTO> applicationContentDTOList;
                ApplicationContentListBL applicationContentListBL = new ApplicationContentListBL(KioskStatic.Utilities.ExecutionContext);
                List<KeyValuePair<ApplicationContentDTO.SearchByParameters, string>> searchApplicationParams = new List<KeyValuePair<ApplicationContentDTO.SearchByParameters, string>>();
                searchApplicationParams.Add(new KeyValuePair<ApplicationContentDTO.SearchByParameters, string>(ApplicationContentDTO.SearchByParameters.MODULE, ApplicationContentModule.ToString()));
                searchApplicationParams.Add(new KeyValuePair<ApplicationContentDTO.SearchByParameters, string>(ApplicationContentDTO.SearchByParameters.APPLICATION, "KIOSK"));
                searchApplicationParams.Add(new KeyValuePair<ApplicationContentDTO.SearchByParameters, string>(ApplicationContentDTO.SearchByParameters.SITE_ID, KioskStatic.Utilities.ExecutionContext.GetSiteId().ToString()));
                applicationContentDTOList = applicationContentListBL.GetApplicationContentDTOList(searchApplicationParams, true, true);
                if (applicationContentDTOList != null && applicationContentDTOList.Count > 0)
                {
                    if (applicationContentDTOList[0].ApplicationContentTranslatedDTOList != null && applicationContentDTOList[0].ApplicationContentTranslatedDTOList.Count > 0)
                    {
                        if (applicationContentDTOList[0].ApplicationContentTranslatedDTOList.Exists(x => (bool)(x.LanguageId == KioskStatic.Utilities.ParafaitEnv.LanguageId)))
                        {
                            richcontentId = applicationContentDTOList[0].ApplicationContentTranslatedDTOList.Where(x => (bool)(x.LanguageId == KioskStatic.Utilities.ParafaitEnv.LanguageId)).ToList<ApplicationContentTranslatedDTO>()[0].ContentId;
                        }
                    }
                    if (richcontentId == -1)
                    {
                        richcontentId = applicationContentDTOList[0].ContentId;
                    }
                }

                RichContentListBL richContentListBL = new RichContentListBL(KioskStatic.Utilities.ExecutionContext);
                List<KeyValuePair<RichContentDTO.SearchByParameters, string>> searchRichParams = new List<KeyValuePair<RichContentDTO.SearchByParameters, string>>();
                searchRichParams.Add(new KeyValuePair<RichContentDTO.SearchByParameters, string>(RichContentDTO.SearchByParameters.ID, richcontentId.ToString()));
                richContentDTOList = richContentListBL.GetRichContentDTOList(searchRichParams);

                if (richContentDTOList == null || (richContentDTOList != null && richContentDTOList.Count == 0))
                {
                    richContentDTOList = null;
                    log.LogMethodExit();
                    return null;
                }
                else
                {
                    log.LogMethodExit(richContentDTOList[0]);
                    return richContentDTOList[0];
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                logger.Monitor kioskMonitor = new logger.Monitor();
                kioskMonitor.Post(logger.Monitor.MonitorLogStatus.ERROR, ex.Message, "Error while fetching Rich Content", "");
                log.LogMethodExit();
                return null;
            }
        }
        public static void LoadTermsAndConditionsFiles()
        {
            log.LogMethodEntry();
            CheckInRichContentDTO = GetTermsAndConditionsFile(ApplicationContentModule.CHECKIN);
            RegistrationRichContentDTO = GetTermsAndConditionsFile(ApplicationContentModule.REGISTRATION);
            log.LogMethodExit();
        }

        public static int GetKioskGlobalTransactionId(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            AddToLogFile("Getting New kiosk gloabl tranasctionId");
            int kioskGlobalTransactionId = -1;
            try
            {
                ExecutionContext executionContext = KioskStatic.Utilities.ExecutionContext;
                SequencesDTO sequencesDTO = GetkioskTransactionSequenceDTO(sqlTransaction);
                SequencesBL sequenceBL = new SequencesBL(executionContext, sequencesDTO);
                string sequenceNo = sequenceBL.GetNextSequenceNo(sqlTransaction);
                if (string.IsNullOrWhiteSpace(sequenceNo) == false)
                {
                    kioskGlobalTransactionId = Convert.ToInt32(sequenceNo);
                    AddToLogFile("New kiosk gloabl tranasctionId " + kioskGlobalTransactionId.ToString());
                    log.LogVariableState("New kiosk gloabl tranasctionId ", kioskGlobalTransactionId.ToString());
                }
                else
                {
                    string sequenceName = "KioskTransaction";
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext,
                      "Setup is missing for Sequence Name &1", sequenceName));
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                AddToLogFile("Error in GetKioskGlobalTransactionId:" + ex.Message);
                throw;
            }
            log.LogMethodExit(kioskGlobalTransactionId);
            return kioskGlobalTransactionId;
        }

        private static SequencesDTO GetkioskTransactionSequenceDTO(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            AddToLogFile("In GetkioskTransactionSequenceDTO()");
            if (kioskTransactionSequenceDTO == null)
            {
                AddToLogFile("Getting KioskTransaction sequence details from DB");
                log.Info("Getting KioskTransaction sequence details from DB");
                string sequenceName = "KioskTransaction";
                ExecutionContext executionContext = KioskStatic.Utilities.ExecutionContext;
                SequencesListBL sequencesListBL = new SequencesListBL(executionContext);
                List<KeyValuePair<SequencesDTO.SearchByParameters, string>> searchBySeqParameters = new List<KeyValuePair<SequencesDTO.SearchByParameters, string>>();
                searchBySeqParameters.Add(new KeyValuePair<SequencesDTO.SearchByParameters, string>(SequencesDTO.SearchByParameters.SEQUENCE_NAME, sequenceName));
                searchBySeqParameters.Add(new KeyValuePair<SequencesDTO.SearchByParameters, string>(SequencesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<SequencesDTO> sequencesDTOList = sequencesListBL.GetAllSequencesList(searchBySeqParameters, sqlTransaction);
                if (sequencesDTOList != null && sequencesDTOList.Any())
                {
                    kioskTransactionSequenceDTO = sequencesDTOList[0];
                    if (kioskTransactionSequenceDTO == null)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Setup is missing for Sequence Name &1", sequenceName));
                        //Setup is missing for Sequence Name &1
                    }
                }
            }
            log.LogMethodExit(kioskTransactionSequenceDTO);
            return kioskTransactionSequenceDTO;
        }

        public static void AddToLogFile(string logText)
        {
            string file = LogDirectory + Environment.MachineName + " Kiosk " + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            lock (LogDirectory)
            {

                File.AppendAllText(file, DateTime.Now.ToString("h:mm:ss tt") + ": " + logText + Environment.NewLine);
            }
        }

        /// <summary>
        /// RestartPaymentGatewayService
        /// </summary>
        public static void RestartPaymentGatewayComponent(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            KioskStatic.logToFile("RestartPaymentGatewayComponent is triggerd");
            if (pOSPaymentModeInclusionDTOList != null && pOSPaymentModeInclusionDTOList.Any())
            {
                for (int i = 0; i < pOSPaymentModeInclusionDTOList.Count; i++)
                {
                    POSPaymentModeInclusionDTO pOSPaymentModeInclusionDTO = pOSPaymentModeInclusionDTOList[i];
                    try
                    {
                        PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(pOSPaymentModeInclusionDTO.PaymentModeDTO.GatewayLookUp);
                        if (paymentGateway.GatewayComponentNeedsRestart())
                        {
                            KioskStatic.logToFile("Restarting Payment component: " + pOSPaymentModeInclusionDTO.PaymentModeDTO.GatewayLookUp.ToString());
                            paymentGateway.RestartPaymentGatewayComponent(true);
                            if (paymentGateway.GatewayComponentNeedsRestart())
                            {
                                string msg = "Restart is not successful. Kiosk might need a restart";
                                KioskStatic.logToFile(msg);
                                KioskStatic.UpdateKioskActivityLog(executionContext, PAYMENT_GATEWAY_COMPONENT_RESTART_ERROR, msg);
                            }
                            else
                            {
                                KioskStatic.logToFile("Restart is successful");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        string msg = "Error while restarting Payment Gateway component: "
                                      + pOSPaymentModeInclusionDTO.PaymentModeDTO.GatewayLookUp.ToString() + " Error: " + ex.Message;
                        KioskStatic.logToFile(msg);
                        KioskStatic.UpdateKioskActivityLog(executionContext, PAYMENT_GATEWAY_COMPONENT_RESTART_ERROR, msg);
                    }
                }
            }
            log.LogMethodExit();
        }
        public static void UpdateKioskActivityLog(ExecutionContext machineUserContext, string activity, string msg, string cardNumber = null,
            int salesTrxId = -1, int kioskTrxId = -1, double? amountValue = null)
        {
            log.LogMethodEntry(machineUserContext, activity, msg, cardNumber, salesTrxId, kioskTrxId);
            KioskStatic.logToFile("Activity Log: Activity- " + activity + " : Msg- " + msg + " : CardNumber- " + cardNumber + " : SalesTrxId- " + salesTrxId.ToString() + " : KioskTrxId- " + kioskTrxId.ToString());
            try
            {
                KioskActivityLogDTO KioskActivityLogDTO = new KioskActivityLogDTO("", ServerDateTime.Now, activity, amountValue, cardNumber, msg,
                                       machineUserContext.GetMachineId(), machineUserContext.POSMachineName, "", false, salesTrxId, -1, kioskTrxId, -1);
                KioskActivityLogBL kioskActivityLogBL = new KioskActivityLogBL(machineUserContext, KioskActivityLogDTO);
                kioskActivityLogBL.Save();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while saving KioskActivityLog data: " + ex.Message);
            }
            log.LogMethodExit();
        }
        public static string GetTempTagNumber()
        {
            log.LogMethodEntry();
            string autoGeneratedCardNumber = GetTagNumber();
            string tempCardNumber = "T" + autoGeneratedCardNumber.Substring(1);
            log.LogMethodExit(tempCardNumber);
            return tempCardNumber;
        }
        public static void ReceivedDenominationToActivityLog(ExecutionContext executionContext, int transactionId, string cardNumber, int receivedDenomination,
            string activity, string message, string coinOrNoteOrToken)
        {
            log.LogMethodEntry(executionContext, transactionId, cardNumber, receivedDenomination, activity, message, coinOrNoteOrToken);
            double? moneyValue = null;
            string noteCoinFLag = coinOrNoteOrToken;
            string finalMsg = string.Empty;
            if (coinOrNoteOrToken == NOTE)
            {
                moneyValue = (double)config.Notes[receivedDenomination].Value;
                finalMsg = config.Notes[receivedDenomination].Name + " " + message;
            }
            else if (coinOrNoteOrToken == COIN || coinOrNoteOrToken == TOKEN)
            {
                noteCoinFLag = config.Coins[receivedDenomination].isToken ? TOKEN : COIN;
                moneyValue = (double)config.Coins[receivedDenomination].Value;
                finalMsg = config.Coins[receivedDenomination].Name + " " + message;
            }

            KioskActivityLogDTO KioskActivityLogDTO = new KioskActivityLogDTO(noteCoinFLag, ServerDateTime.Now, activity, moneyValue, cardNumber, finalMsg, executionContext.GetMachineId(),
                                                                             executionContext.POSMachineName, "", false, transactionId, -1, -1, -1);
            KioskActivityLogBL kioskActivityLogBL = new KioskActivityLogBL(executionContext, KioskActivityLogDTO);
            kioskActivityLogBL.Save();
            log.LogMethodExit();
        }

        private static void SetDefaultColorsForSimilarTags(string tagForFontColor)
        {
            log.LogMethodEntry(tagForFontColor);
            switch (tagForFontColor)
            {
                case "homescreenbtnhometextforecolor":
                    SetHomeButtonTextForeColor();
                    break;
                case "homescreenfootertextforecolor":
                    SetFooterTextForeColor();
                    break;
                default:
                    break;
            }
            log.LogMethodExit();
        }
        private static void SetHomeButtonTextForeColor()
        {
            log.LogMethodEntry();
            KioskStatic.CurrentTheme.PurchaseMenuBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.ChooseProductsBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.CreditsToTimeBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.PaymentModeBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.PreSelectPaymentModeBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.RedeemTokensScreenBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.UpsellProductScreenBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.FrmCustWaiverBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.FrmCustOptionBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.FrmSelectWaiverBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.FrmSignWaiverFilesBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.FrmSignWaiversBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.ComboChildProductsQtyHomeButtonTextForeColor =
            KioskStatic.CurrentTheme.AdultSummaryHomeButtonTextForeColor =
            KioskStatic.CurrentTheme.CheckInSummaryHomeButtonTextForeColor =
            KioskStatic.CurrentTheme.ChildSummaryHomeButtonTextForeColor =
            KioskStatic.CurrentTheme.EnterAdultDetailsHomeButtonTextForeColor =
            KioskStatic.CurrentTheme.EnterChildDetailsHomeButtonTextForeColor =
            KioskStatic.CurrentTheme.SelectAdultHomeButtonTextForeColor =
            KioskStatic.CurrentTheme.SelectChildHomeButtonTextForeColor =
            KioskStatic.CurrentTheme.AddCustomerRelationBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.CustomerScreenBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.CustomerDashboardBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.FrmSuccessMsgBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.FrmViewWaiverUIBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.ComboDetailsBtnHomeTextForeColor =
            KioskStatic.CurrentTheme.AttractionQtyHomeButtonTextForeColor =
            KioskStatic.CurrentTheme.ProcessingAttractionsHomeButtonTextForeColor =
            KioskStatic.CurrentTheme.SelectSlotHomeButtonTextForeColor =
            KioskStatic.CurrentTheme.AttractionSummaryBtnHomeForeColor = KioskStatic.CurrentTheme.HomeScreenBtnHomeTextForeColor;
            log.LogMethodExit();
        }

        private static void SetFooterTextForeColor()
        {
            log.LogMethodEntry();
            KioskStatic.CurrentTheme.DisplayGroupFooterTextForeColor =
            KioskStatic.CurrentTheme.ChooseProductsFooterTextForeColor =
            KioskStatic.CurrentTheme.TransferFormFooterTextForeColor =
            KioskStatic.CurrentTheme.CustomerScreenFooterTextForeColor =
            KioskStatic.CurrentTheme.CheckBalanceFooterTextForeColor =
            KioskStatic.CurrentTheme.CreditsToTimeFooterTextForeColor =
            KioskStatic.CurrentTheme.RedeemTokensScreenFooterTextForeColor =
            KioskStatic.CurrentTheme.TransferToScreenFooterTextForeColor =
            KioskStatic.CurrentTheme.KioskActivityTxtMessageTextForeColor =
            KioskStatic.CurrentTheme.CardTransactionFooterTextForeColor =
            KioskStatic.CurrentTheme.LoadBonusFooterTextForeColor =
            KioskStatic.CurrentTheme.UpsellProductScreenFooterTextForeColor =
            KioskStatic.CurrentTheme.FrmKioskTransViewTextTxtMessageForeColor =
            KioskStatic.CurrentTheme.FrmPrintTransTxtMessageTextForeColor =
            KioskStatic.CurrentTheme.FskExecuteOnlineTxtMessageTextForeColor =
            KioskStatic.CurrentTheme.FrmCustWaiverTxtMessageTextForeColor =
            KioskStatic.CurrentTheme.FrmCustOptionTxtMessageTextForeColor =
            KioskStatic.CurrentTheme.FrmCustOTPTxtMessageTextForeColor =
            KioskStatic.CurrentTheme.FrmGetCustInputTxtMessageTextForeColor =
            KioskStatic.CurrentTheme.FrmLinkCustTxtMessageTextForeColor =
            KioskStatic.CurrentTheme.FrmSelectWaiverTxtMessageTextForeColor =
            KioskStatic.CurrentTheme.FrmSignWaiverFilesTxtMessageTextForeColor =
            KioskStatic.CurrentTheme.FrmSignWaiversTxtMessageTextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardTxtMessageTextForeColor =
            KioskStatic.CurrentTheme.PaymentGameCardTxtMessageErrorBackColor =
            KioskStatic.CurrentTheme.FrmLoyaltyTxtMessageTextForeColor =
            KioskStatic.CurrentTheme.FundsDonationsFooterTextForeColor =
            KioskStatic.CurrentTheme.ComboChildProductsQtyFooterTxtMsgTextForeColor =
            KioskStatic.CurrentTheme.SelectChildFooterTxtMsgTextForeColor =
            KioskStatic.CurrentTheme.EnterChildDetailsFooterTextMsgTextForeColor =
            KioskStatic.CurrentTheme.ChildSummaryFooterTxtMsgTextForeColor =
            KioskStatic.CurrentTheme.SelectAdultFooterTxtMsgTextForeColor =
            KioskStatic.CurrentTheme.EnterAdultDetailsFooterTextMsgTextForeColor =
            KioskStatic.CurrentTheme.AdultSummaryFooterTxtMsgTextForeColor =
            KioskStatic.CurrentTheme.CheckInSummaryFooterMessageTextForeColor =
            KioskStatic.CurrentTheme.PurchaseMenuFooterTextForeColor =
            KioskStatic.CurrentTheme.FrmGetEmailDetailsFooterTxtMsgTextForeColor =
            KioskStatic.CurrentTheme.FrmKioskCartFooterTxtMsgTextForeColor =
            KioskStatic.CurrentTheme.AddCustomerRelationTxtboxMessageLineTextForeColor =
            KioskStatic.CurrentTheme.PaymentModeTextMessageTextForeColor =
            KioskStatic.CurrentTheme.ComboDetailsFooterTextForeColor =
            KioskStatic.CurrentTheme.AttractionQtyFooterTextForeColor =
            KioskStatic.CurrentTheme.ProcessingAttractionsFooterTxtMsgTextForeColor =
            KioskStatic.CurrentTheme.SelectSlotFooterTextForeColor =
            KioskStatic.CurrentTheme.AttractionSummaryTxtMessageForeColor =
            KioskStatic.CurrentTheme.SearchCustomerFooterTextForeColor =
            KioskStatic.CurrentTheme.SelectCustomerFooterTextForeColor =
            KioskStatic.CurrentTheme.WaiverSigningAlertFooterTextForeColor =
            KioskStatic.CurrentTheme.MapAttendeesFooterTextForeColor = KioskStatic.CurrentTheme.HomeScreenFooterTextForeColor;
            log.LogMethodExit();
        }
        /// <summary>
        /// UpdateThemeTextFile
        /// </summary>
        public static void UpdateThemeTextFile()
        {
            log.LogMethodEntry();
            string configFile = System.Windows.Forms.Application.StartupPath + @"\Parafait Kiosk Configuration.txt";
            string newTagFile = System.Windows.Forms.Application.StartupPath + @"\NewThemeConfigurationTags.txt";
            string newTagFilePart1 = System.Windows.Forms.Application.StartupPath + @"\NewTheme";
            string newTagFilePart2 = "ConfigurationTags.txt";
            string themeNo = getConfigValue("UIThemeNo");
            logToFile("Kiosk Config file: " + configFile);
            if (File.Exists(configFile))
            {
                ProcessNewTagFile(configFile, newTagFile);
                string specificThemeNewTagFile = newTagFilePart1 + themeNo + newTagFilePart2;
                ProcessNewTagFile(configFile, specificThemeNewTagFile);
            }
            log.LogMethodExit();
        }

        private static void ProcessNewTagFile(string configFile, string newTagFile)
        {
            log.LogMethodEntry(configFile, newTagFile);
            if (File.Exists(newTagFile))
            {
                string theme = string.Empty;
                try
                {
                    theme = "[theme" + getConfigValue("UIThemeNo") + "]";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                string[] lines = File.ReadAllLines(configFile);
                int lineCount = lines.Length;
                bool addedNewTags = false;
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (line.Replace(" ", "").ToLower().Equals(theme))
                    {
                        string[] newTagLines = File.ReadAllLines(newTagFile);
                        for (int newTagIndex = 0; newTagIndex < newTagLines.Length; newTagIndex++)
                        {
                            string newTagLine = newTagLines[newTagIndex];
                            newTagLine = newTagLine.Replace(" ", "").Trim();
                            if (newTagLine.Length > 1)
                            {
                                if (ThemeFileHasEntry(lines, i, newTagLine) == false)
                                {
                                    lines = AddNewTagLineToThneFile(lines, i, newTagLine);
                                    lineCount = lines.Length;
                                    addedNewTags = true;
                                }
                            }
                        }
                        break;
                    }
                }
                if (addedNewTags)
                {
                    File.WriteAllLines(configFile, lines);
                    string appendText1 = "Processed";
                    string appendText2 = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
                        + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                    string middlePart = Path.GetFileName(newTagFile);
                    File.Move(newTagFile, appendText1 + middlePart + appendText2 + ".txt");
                }
            }
            log.LogMethodExit();
        }
        private static bool ThemeFileHasEntry(string[] lines, int themeStartIndex, string newTagLine)
        {
            log.LogMethodEntry(themeStartIndex, newTagLine);
            bool themeFilehasEntry = false;
            string[] nameValue = newTagLine.Split('=');
            string newTagName = nameValue[0].Replace(" ", "").Trim().ToLower();
            for (int j = themeStartIndex + 1; j < lines.Length; j++)
            {
                string themeLine = lines[j];
                if (themeLine.Replace(" ", "").ToLower().Contains("[theme"))
                    break;

                if (themeLine.Trim() == "")
                    continue;

                string[] themeLineValue = themeLine.Split('=');
                if (themeLineValue[0].Replace(" ", "").Trim().ToLower() == newTagName)
                {
                    themeFilehasEntry = true;
                    break;
                }
            }
            log.LogMethodExit(themeFilehasEntry);
            return themeFilehasEntry;
        }
        private static string[] AddNewTagLineToThneFile(string[] lines, int themeStartIndex, string newTagLine)
        {
            log.LogMethodEntry(themeStartIndex, newTagLine);
            string[] newLines = lines;
            int lastEntryLineIndex = -1;
            List<string> listEntry = lines.ToList();
            for (int j = themeStartIndex + 1; j < lines.Length; j++)
            {
                string themeLine = lines[j];
                lastEntryLineIndex = j;
                if (themeLine.Replace(" ", "").ToLower().Contains("[theme"))
                {
                    break;
                }
            }
            if (lastEntryLineIndex == lines.Length - 1)
            {
                lastEntryLineIndex++;
            }
            if (lastEntryLineIndex > -1)
            {
                listEntry.Insert(lastEntryLineIndex, newTagLine);
                newLines = listEntry.ToArray();
            }
            log.LogMethodExit("newLines");
            return newLines;
        }

        private static void SetKioskTypeInAppConfig(string kioskType)
        {
            log.LogMethodEntry(kioskType);
            if (string.IsNullOrWhiteSpace(kioskType) == false && kioskType == TABLETOP_KIOSK_TYPE)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                XmlNode appSettingsNode = xmlDoc.SelectSingleNode("/configuration/appSettings");
                if (appSettingsNode == null)
                {
                    appSettingsNode = CreateAppSettingsNode(xmlDoc);
                }
                bool updated = false;
                if (IsAppSettingExists(appSettingsNode, "KIOSK_TYPE") == false)
                {
                    AddToAppSettings(xmlDoc, appSettingsNode, "KIOSK_TYPE", TABLETOP_KIOSK_TYPE);
                    updated = true;
                }
                if (updated)
                {
                    xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// GetImageNamesFromConfigFile
        /// </summary>  
        public static void GetImageNamesFromConfigFile(string themeName, out string homeScreenBkgImageFileNameFromConfigFile, out string defaultBkgImageFileNameFromConfigFile, out string splashScreenFileNameFromConfigFile)
        {
            log.LogMethodEntry(themeName);
            string configFile = System.Windows.Forms.Application.StartupPath + @"\Parafait Kiosk Configuration.txt";
            homeScreenBkgImageFileNameFromConfigFile = string.Empty;
            defaultBkgImageFileNameFromConfigFile = string.Empty;
            splashScreenFileNameFromConfigFile = string.Empty;
            if (File.Exists(configFile))
            {
                string[] lines = File.ReadAllLines(configFile);
                string themeWithBrackets = "[theme" + themeName + "]";
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (line.Replace(" ", "").ToLower().Equals(themeWithBrackets))
                    {
                        for (int j = i + 1; j < lines.Length; j++)
                        {
                            string themeLine = lines[j];
                            if (themeLine.Replace(" ", "").ToLower().Contains("[theme"))
                                break;

                            if (themeLine.Trim() == "")
                                continue;
                            string[] nameValue = themeLine.Split('=');
                            switch (nameValue[0].Trim().ToLower())
                            {
                                case "homescreenbackgroundimage":
                                    if (nameValue.Length > 1)
                                    {
                                        try
                                        {
                                            homeScreenBkgImageFileNameFromConfigFile = nameValue[1].Trim();
                                            KioskStatic.logToFile("homeScreenBkgImageFileNameFromConfigFile: " + homeScreenBkgImageFileNameFromConfigFile);
                                        }
                                        catch (Exception ex)
                                        {
                                            log.Error(ex);
                                            KioskStatic.logToFile(ex.Message);
                                        }
                                    }
                                    break;
                                case "defaultbackgroundimage":
                                    if (nameValue.Length > 1)
                                    {
                                        try
                                        {
                                            defaultBkgImageFileNameFromConfigFile = nameValue[1].Trim();
                                            KioskStatic.logToFile("defaultBkgImageFileNameFromConfigFile: " + defaultBkgImageFileNameFromConfigFile);
                                        }
                                        catch (Exception ex)
                                        {
                                            log.Error(ex);
                                            KioskStatic.logToFile(ex.Message);
                                        }
                                    }
                                    break;
                                case "splashscreenimage"://playpas1:starts
                                    if (nameValue.Length > 1)
                                    {
                                        try
                                        {
                                            splashScreenFileNameFromConfigFile = nameValue[1].Trim();
                                            KioskStatic.logToFile("splashScreenFileNameFromConfigFile: " + splashScreenFileNameFromConfigFile);
                                        }
                                        catch (Exception ex)
                                        {
                                            log.Error(ex);
                                            KioskStatic.logToFile(ex.Message);
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    }
                }
            }
            log.LogMethodExit();
        }
        public static void LoadMasterScheduleBLList()
        {
            log.LogMethodEntry();
            if (MasterScheduleBLList == null)
            {
                MasterScheduleList masterScheduleList = new MasterScheduleList(Utilities.ExecutionContext);
                MasterScheduleBLList = masterScheduleList.GetAllMasterScheduleBLList();
            }
            log.LogMethodExit();
        }

        public static void LoadAttractionSchedulesForTheDay()
        {
            int businessEndHour = ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME");
            DateTime currentTime = ServerDateTime.Now;
            if (currentTime.Hour >= 0 && currentTime.Hour < businessEndHour)
                currentTime = currentTime.AddDays(-1);
            AttractionBookingSchedulesBL attractionBookingScheduleBL = new AttractionBookingSchedulesBL(Utilities.ExecutionContext);
            List<ScheduleDetailsDTO> scheduleDetailsDTOList = attractionBookingScheduleBL.GetAttractionBookingSchedules(currentTime.Date.AddHours(businessEndHour), "", -1, null, 0, 24, true);
        }

        private static void SetCalendarSelection()
        {
            log.LogMethodEntry();
            bool ignoreYear = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "IGNORE_CUSTOMER_BIRTH_YEAR");
            string dateMonthFormat = KioskStatic.DateMonthFormat.ToLower();
            enableDaySelection = dateMonthFormat.Contains("d");
            enableMonthSelection = dateMonthFormat.Contains("m");
            if (ignoreYear == false)
            {
                if (dateMonthFormat.Contains("y") == false)
                {
                    enableYearSelection = false;
                }
            }
            else
            {
                enableYearSelection = false;
            }
            log.LogMethodExit();
        }
    }
}
