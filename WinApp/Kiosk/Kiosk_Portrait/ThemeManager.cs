/********************************************************************************************
* Project Name - Parafait_Kiosk - ThemeManager
* Description  - ThemeManager 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.4.0       28-Sep-2018      Guru S A           Modified for Online Transaction in Kiosk changes 
*2.70.2      09-Sep-2019      Deeksha            Added logger methods.
*2.70.2      12-Dec-2019      Guru S A           Waiver Phase 2 changes
*2.90.0      30-Jun-2020      Dakshakh raj       Dynamic Payment Modes based on set up
*2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
*2.155.0     20-Jun-2023      Sathyavathi        Attraction Sale in Kiosk
*2.150.7     10-Nov-2023      Sathyavathi        Customer Lookup Enhancement
********************************************************************************************/
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Semnox.Parafait.KioskCore;

namespace Parafait_Kiosk
{
    static class ThemeManager
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ThemeImages CurrentThemeImages;
        public class ThemeImages
        {
            public Image BackButtonImage = Properties.Resources.Back_button_box;
            public Image BirthDateEntryBox = Properties.Resources.birthdate_entry_box;
            public Image CardReaderArrowImage = Properties.Resources.card_reader;
            public Image ChangeButtonImage = Properties.Resources.ChangeButton;
            public Image CashButtonImage = Properties.Resources.cash_button;
            public Image CheckBalanceBox = Properties.Resources.check_balance_box;
            public Image CheckBalanceButtonBig = Properties.Resources.Check_Balance_Button_Big;
            public Image CheckBalanceButtonSmall = Properties.Resources.Check_Balance_Button_Small;
            public Image CloseButton = Properties.Resources.close_button_1;
            public Image NoThanksButton = Properties.Resources.close_button_1;
            public Image CountdownTimer = Properties.Resources.countdown_timer;
            public Image CreditCardButton = Properties.Resources.credit_Card_button;
            public Image HomeScreenClientLogo = null;
            public Image DebitCardButton = Properties.Resources.debit_card_button;
            public Image DoneButton = Properties.Resources.done_button;
            public Image ExchangeTokensButtonBig = Properties.Resources.Exchange_tokens_Button;
            public Image ExchangeTokensButtonSmall = Properties.Resources.Exchange_tokens_Button_Small;
            public Image GamePriceTable = Properties.Resources.game_price_table;
            public Image HomeButton = Properties.Resources.home_button;
            public Image InsertCash = Properties.Resources.insert_cash;
            public Image InsertCashAnimation = Properties.Resources.InsertCash_Animation;
            public Image KioskActivityTableImage = Properties.Resources.Table1;
            public Image LoadExistingImage = Properties.Resources.LoadExisting;
            public Image NewCardButtonSmall = Properties.Resources.New_Play_Pass_Button;
            public Image NewPlayPassButtonSmall = Properties.Resources.New_Play_Pass_Button_Small;
            public Image NewPlayPassButtonBig = Properties.Resources.New_Play_Pass_Button_big;
            public Image NoOfCardButton = Properties.Resources.No_of_Card_Button;
            public Image PassesTwo = Properties.Resources.Passes_Two;
            public Image PlainProductButton = Properties.Resources.plain_product_button;
            public Image ProductTableImage = Properties.Resources.product_table;
            public Image PurchaseSummaryTableImage = Properties.Resources.TablePurchaseSummary;
            public Image RechargePlayPassButtonBig = Properties.Resources.Recharge_Play_Pass_Big;
            public Image RechargePlayPassButtonSmall = Properties.Resources.Recharge_Play_Pass_Small;
            public Image RedeemTable = Properties.Resources.table3;
            public Image RegisterPassSmall = Properties.Resources.register_pass_Small;
            public Image RegisterPassBig = Properties.Resources.register_pass_big;
            public Image RegisteredCustomerBig = Properties.Resources.Registered_Customer_Big;
            public Image SpecialOfferLogo = Properties.Resources.special_offer_semnox_logo;
            public Image SucessAddImage = Properties.Resources.sucess_Add;
            public Image SucessRechargeImage = Properties.Resources.sucess_Recharge;
            public Image SucessRedeemImage = Properties.Resources.sucessRedeem;
            public Image SucessRegister = Properties.Resources.sucessRegister;
            public Image SucessCheckIn = Properties.Resources.sucessCheckIn;
            public Image SucessTransfer = Properties.Resources.sucessTransfer;
            public Image SureWhyNotButton = Properties.Resources.sure_button;
            public Image TermsButton = Properties.Resources.terms_button;
            public Image TimerBoxSmall = Properties.Resources.timer_SmallBox;
            public Image TextEntryBox = Properties.Resources.text_entry_box;
            public Image SplashScreenBackgroundImage = null;//Properties.Resources.Home_screen;
            public Image SplashScreenImage = null;//Properties.Resources.touch_screen_semnox_logo;
            public Image ScrollUpButtonImage = Properties.Resources.up_button;
            public Image ScrollDownButton = Properties.Resources.down_button;
            public Image TransferPointButtonBig = Properties.Resources.Transfer_Point;
            public Image TransferPointButtonSmall = Properties.Resources.Transfer_Point_Small;
            public Image PauseCardButtonSmall = Properties.Resources.Pause_Card_Button_Small;
            public Image PauseCardButtonBig = Properties.Resources.Pause_Card_Button_Big;
            public Image PointsToTimeButtonBig = Properties.Resources.Points_To_Time_Button_Big;
            public Image PointsToTimeButtonSmall = Properties.Resources.Points_To_Time_Button_Small;
            public Image HomeScreenSemnoxLogo = null;
            public Image ExecuteOnlineTrxButtonBig = Properties.Resources.Execute_Online_Trx_Button;
            public Image ExecuteOnlineTrxButtonSmall = Properties.Resources.Execute_Online_Trx_Button_Small;
            //public Image FSKSalesButton = Properties.Resources.FSK_Sales_Button;
            public Image GetTransactionDetailsButton = Properties.Resources.Search_Button;
            public Image GameCardButton = Properties.Resources.Game_Card_button;
            public Image AddRelationButtonImage = Properties.Resources.AddRelation_button_box;
            public Image DecreaseQtyButton = Properties.Resources.DecreaseQty;
            public Image IncreaseQtyButton = Properties.Resources.IncreaseQty;
            public Image DecreaseQtyDisabledButton = Properties.Resources.DecreaseQtyDisabled;
            public Image IncreaseQtyDisabledButton = Properties.Resources.IncreaseQtyDisabled;
            public Image ComboInformationIcon = Properties.Resources.ComboInformationIcon;
            public Image SucessFnBImage = Properties.Resources.SucessFnBImage;
            public Image SucessCartImage = Properties.Resources.SucessCartImage;
            public Image ScrollLeftButton = Properties.Resources.left_arrow;
            public Image ScrollRightButton = Properties.Resources.right_arrow;
            public Image PurchaseButtonBig = Properties.Resources.Purchase_Button_Big;
            public Image PurchaseButtonSmall = Properties.Resources.Purchase_Button_Small;
            public Image SignWaiverButtonBig = Properties.Resources.Sign_Waiver_Big;
            public Image SignWaiverButtonSmall = Properties.Resources.Sign_Waiver_Small;
            public Image FoodAndBeverageBig = Properties.Resources.FoodAndBeverageBig;
            public Image FoodAndBeverageSmall = Properties.Resources.FoodAndBeverageSmall;
            public Image PlaygroundEntrySmall = Properties.Resources.Playground_Entry_Small;
            public Image PlaygroundEntryBig = Properties.Resources.Playground_Entry_Big;
            public Image WaiverSigningInstructions = Properties.Resources.WaiverSigningInstructions;
            public Image LoyaltyFormLogoImage = null;
            public Image ScrollUpEnabled = Properties.Resources.Scroll_Up_Button;
            public Image ScrollUpDisabled = Properties.Resources.Scroll_Up_Button_Disabled;
            public Image ScrollDownEnabled = Properties.Resources.Scroll_Down_Button;
            public Image ScrollDownDisabled = Properties.Resources.Scroll_Down_Button_Disabled;
            public Image ScrollLeftEnabled = Properties.Resources.Scroll_Left_Button;
            public Image ScrollLeftDisabled = Properties.Resources.Scroll_Left_Button_Disabled;
            public Image ScrollRightEnabled = Properties.Resources.Scroll_Right_Button;
            public Image ScrollRightDisabled = Properties.Resources.Scroll_Right_Button_Disabled;
            public Image BottomMessageLineImage = Properties.Resources.bottom_bar;
            public Image DiscountButton = Properties.Resources.Back_button_box;
            public Image LanguageButton = Properties.Resources.Language_Btn;
            public Image CalenderIconImage = Properties.Resources.CalenderIcon;
            public Image ChooseProductButton = Properties.Resources.plain_product_button_1;
            public Image WaiverButtonBorderImage = Properties.Resources.WaiverButtonBorder;
            public Image TablePurchaseSummary = Properties.Resources.TablePurchaseSummary;
            public Image KioskCartIcon = Properties.Resources.KioskCartIcon;
            public Image FundRaiserPictureBoxLogo = null;
            public Image DonationsPictureBoxLogo = null;
            public Image YesNoButtonImage = Properties.Resources.btnYes_image;
            public Image ReceiptModeBtnBackgroundImage = Properties.Resources.close_button_1;
            public Image CartItemBackgroundImageSmall = Properties.Resources.WhiteBackgroundSmall;
            public Image CartItemBackgroundImage = Properties.Resources.WhiteBackground;
            public Image CartDeleteButtonBackgroundImage = Properties.Resources.NewDeleteButton;
            public Image AttractionsButtonBig = Properties.Resources.AttractionsBig;
            public Image AttractionsButtonSmall = Properties.Resources.AttractionsSmall;
            public Image SmallCircleImage = Properties.Resources.SmallCircle;
            public Image AttractionsComboProductBackgroundImage = Properties.Resources.ComboProductBackground;
            public Image PanelYourSelections = Properties.Resources.YourSelections;
            public Image QuantityTextBox = Properties.Resources.textbox_quantity;
            public Image QuantityImage = Properties.Resources.Quantity;
            public Image NewTickedCheckBox = Properties.Resources.NewTickedCheckBox;
            public Image NewUnTickedCheckBox = Properties.Resources.NewUnTickedCheckBox;
            public Image AttractionNewCard = Properties.Resources.AttractionNewCard;
            public Image AttractionExistingCard = Properties.Resources.AttractionExistingCard;
            public Image PanelCardSaleOption = Properties.Resources.WhiteBackground;
            public Image BigCircleSelected = Properties.Resources.BigCircleSelected;
            public Image BigCircleUnSelected = Properties.Resources.BigCircleUnSelected;
            public Image PanelTimeSection = Properties.Resources.TimePanelBackground;
            public Image PanelDateSection = Properties.Resources.DatePanelBackground;
            public Image SlotBackgroundImage = Properties.Resources.Slot;
            public Image SlotSelectedBackgroundImage = Properties.Resources.SlotSelected;
            public Image CheckboxSelected = Properties.Resources.selected_tick;
            public Image PickDate = Properties.Resources.pickDate;
            public Image VariableProductButtonImage = null;
            public Image CustomerFoundImage = Properties.Resources.CustomerFound;
            public Image SelectCustomerUsrCtrlBackgroundImage = Properties.Resources.Slot;
            public Image SelectCustomerUsrCtrlSelectedBackgroundImage = Properties.Resources.SlotSelected;

            //Background images
            public Image HomeScreenBackgroundImage = Properties.Resources.Home_screen;
            public Image DefaultBackgroundImage = Properties.Resources.Home_screen;
            public Image DefaultBackgroundImageTwo = Properties.Resources.Home_screen;

            //DefaultBackgroundImage
            public Image FundRaiserBackgroundImage = null;
            public Image TransferFromBackgroundImage = null;
            public Image TransferToBackgroundImage = null;
            public Image AdultSummaryBackgroundImage = null;
            public Image CheckInCheckOutQtyScreenBackgroundImage = null;
            public Image CheckInSummaryBackgroundImage = null;
            public Image ChildSummaryBackgroundImage = null;
            public Image ProcessingCheckinDetailsBackgroundImage = null;
            public Image AddCustomerRelationBackgroundImage = null;
            public Image AgeGateBackgroundImage = null;
            public Image CustomerBackgroundImage = null;
            public Image CustomerDashboardBackgroundImage = null;
            public Image TermsAndConditionsBackgroundImage = null;
            public Image CardCountBasicBackgroundImage = null;
            public Image CashInsertBackgroundImage = null;
            public Image IssueTempCardsBackgroundImage = null;
            public Image KioskActivityDetailsBackgroundImage = null;
            public Image PrintTransactionLinesBackgroundImage = null;
            public Image FSKCoverPageBackgroundImage = null;
            public Image CardCountBackgroundImage = null;
            public Image CardTransactionBackgroundImage = null;
            public Image ChooseProductBackgroundImage = null;
            public Image LoadBonusBackgroundImage = null;
            public Image PaymentGameCardBackgroundImage = null;
            public Image RedeemTokensBackgroundImage = null;
            public Image FSKExecuteOnlineTransactionBackgroundImage = null;
            public Image CustomerDetailsForWaiverBackgroundImage = null;
            public Image CustomerOptionsBackgroundImage = null;
            public Image CustomerSignedWaiversBackgroundImage = null;
            public Image GetCustomerInputBackgroundImage = null;
            public Image LinkRelatedCustomerBackgroundImage = null;
            public Image SignWaiverFilesBackgroundImage = null;
            public Image SignWaiversBackgroundImage = null;
            public Image ViewWaiverUIBackgroundImage = null;
            public Image BalanceBackgroundImage = null;
            public Image TransferBackgroundImage = null;
            public Image EmailDetailsBackgroundImage = null;
            public Image PurchaseMenuBackgroundImage = null;
            public Image UpsellBackgroundImage = null;
            public Image KioskPrintSummaryBackgroundImage = null;
            public Image AttractionQtyBackgroundImage = null;
            public Image ProcessingAttractionsBackgroundImage = null;
            public Image SelectSlotBackgroundImage = null;
            public Image AttractionSummaryBackgroundImage = null;

            //DefaultBackgroundImageTwo
            public Image SelectAdultBackgroundImage = null;
            public Image SelectChildBackgroundImage = null;
            public Image EnterAdultBackgroundImage = null;
            public Image EnterChildBackgroundImage = null;
            public Image FAQBackgroundImage = null;
            public Image TransactionViewBackgroundImage = null;
            public Image SelectWaiverOptionsBackgroundImage = null;
            public Image SuccessBackgroundImage = null;
            public Image CartScreenBackgroundImage = null;
            public Image PaymentModeBackgroundImage = null;
            public Image CustomerOTPBackgroundImage = null;

            //DefaultTapCardBox
            public Image TapCardBox = Properties.Resources.tap_card_box;
            public Image CalenderBackgroundImage = null;
            public Image TicketTypeBox = null;
            public Image CustomerSignatureConfirmationBox = null;
            public Image YesNoBox = null;
            public Image ReceiptModeBackgroundImage = null;
            public Image EntitlementBox = null;
            public Image ScanCouponBox = null;
            public Image OkMsgBox = null;
            public Image OkMsgBoxShort = Properties.Resources.OkMsgBoxShort;
            public Image CardSaleOptionBox = null;

            public Image LoyaltyBackGroundImage = Properties.Resources.frmloyalty_background;
            public Image PauseTimeBackgroundImage = Properties.Resources.pause_time_box;
            public Image PreSelectPaymentBackground = Properties.Resources.PreSelectPaymentBackground;
            public Image BalanceAnimationImage = null;
            public Image AdminBackgroundImage = null;
            public Image FilteredCustomersBackground = null;

            //DefaultTapCardButtons
            public Image TapCardButtons = Properties.Resources.close_button;
            public Image CalenderBtnBackgroundImage = null;
            public Image YesNoButtons = null;
            public Image CustomerSignatureConfirmationButtons = null;
            public Image OkMsgButtons = null;
            public Image ScanCouponButtons = null;
            public Image PauseTimeButtons = null;
            public Image CardSaleOptionButtons = null;
            public Image AddToCartAlertButtons = null;
            
            public Image WaiverSigningAlertScreenBackground;
            public Image WaiverSigningAlertUsrCtrlBackground = Properties.Resources.WaiverAlertBackgroundImage;
            public Image WaiverIconImage = Properties.Resources.WaiverIcon;
            public Image PersonIcon = Properties.Resources.PersonIcon;
            public Image PersonSelectedIcon = Properties.Resources.PersonSelectedIcon;
            public Image PersonTickIcon = Properties.Resources.PersonTickIcon;
            public Image SmallCircleSelected = Properties.Resources.SmallCircleSelected;
            public Image SmallCircleUnselected = Properties.Resources.SmallCircleUnselected;
            public Image Expand = Properties.Resources.Expand;
            public Image Collapse = Properties.Resources.Collapse;
        }
        /// <summary>
        /// Loads Theme Images based on the theme no passed
        /// </summary>
        /// <param name="Theme">String variable contains the theme detail, Such as number</param>
        public static void setThemeImages(string Theme)
        {
            log.LogMethodEntry(Theme);
            string homeScreenBkgImageFileNameFromConfigFile = string.Empty;
            string defaultBkgImageFileNameFromConfigFile = string.Empty;
            string defaultBkgImageTwoFileNameFromConfigFile = string.Empty;
            string splashScreenFileNameFromConfigFile = string.Empty;
            KioskStatic.GetImageNamesFromConfigFile(Theme, out homeScreenBkgImageFileNameFromConfigFile, out defaultBkgImageFileNameFromConfigFile,
                  out splashScreenFileNameFromConfigFile);

            CurrentThemeImages = new ThemeImages();
            string ThemeFolder = string.Empty;
            string themeFolderPath = Application.StartupPath + @"\Media\Images\Theme" + Theme;
            string bkpThemeFolderFullPath = Application.StartupPath + @"\Media\Images\DefaultThemes\Theme" + Theme;
            if (Directory.Exists(themeFolderPath))
            {
                KioskStatic.logToFile("Kiosk Theme folder " + themeFolderPath + " is found");
                ThemeFolder = themeFolderPath;
            }
            else
            {
                if (Directory.Exists(bkpThemeFolderFullPath))
                {
                    KioskStatic.logToFile("Backup Kiosk Theme folder " + bkpThemeFolderFullPath + " is found");
                    ThemeFolder = bkpThemeFolderFullPath;
                }
                else
                {
                    string backupFolder = Application.StartupPath + @"\Media\Images\";
                    KioskStatic.logToFile("Separate Kiosk Theme folder " + themeFolderPath + " is not defined. Kiosk will try use images available under root folder: " + backupFolder);
                }
            }

            if (Directory.Exists(ThemeFolder))
            {
                CurrentThemeImages.BackButtonImage = KioskStatic.GetImage(CurrentThemeImages.BackButtonImage, "Back_button_box", ThemeFolder);
                CurrentThemeImages.BirthDateEntryBox = KioskStatic.GetImage(CurrentThemeImages.BirthDateEntryBox, "birthdate_entry_box", ThemeFolder);
                CurrentThemeImages.CardReaderArrowImage = KioskStatic.GetImage(CurrentThemeImages.CardReaderArrowImage, "card_reader", ThemeFolder);
                CurrentThemeImages.ChangeButtonImage = KioskStatic.GetImage(CurrentThemeImages.ChangeButtonImage, "ChangeButton", ThemeFolder);
                CurrentThemeImages.CashButtonImage = KioskStatic.GetImage(CurrentThemeImages.CashButtonImage, "Cash_Button", ThemeFolder);
                CurrentThemeImages.CheckBalanceBox = KioskStatic.GetImage(CurrentThemeImages.CheckBalanceBox, "check_balance_box", ThemeFolder);
                CurrentThemeImages.CheckBalanceButtonBig = KioskStatic.GetImage(CurrentThemeImages.CheckBalanceButtonBig, "Check_Balance_Button_Big", ThemeFolder);
                CurrentThemeImages.CheckBalanceButtonSmall = KioskStatic.GetImage(CurrentThemeImages.CheckBalanceButtonSmall, "Check_Balance_Button_Small", ThemeFolder);
                CurrentThemeImages.CloseButton = KioskStatic.GetImage(CurrentThemeImages.CloseButton, "close_button", ThemeFolder);
                CurrentThemeImages.NoThanksButton = KioskStatic.GetImage(CurrentThemeImages.NoThanksButton, "NoThanksButton", ThemeFolder);
                CurrentThemeImages.CountdownTimer = KioskStatic.GetImage(CurrentThemeImages.CountdownTimer, "countdown_timer", ThemeFolder);
                CurrentThemeImages.CreditCardButton = KioskStatic.GetImage(CurrentThemeImages.CreditCardButton, "credit_Card_button", ThemeFolder);
                CurrentThemeImages.HomeScreenClientLogo = KioskStatic.GetImage(CurrentThemeImages.HomeScreenClientLogo, "HomeScreenClientLogo", ThemeFolder);
                CurrentThemeImages.DebitCardButton = KioskStatic.GetImage(CurrentThemeImages.DebitCardButton, "debit_card_button", ThemeFolder);
                CurrentThemeImages.DoneButton = KioskStatic.GetImage(CurrentThemeImages.DoneButton, "done_button", ThemeFolder);
                CurrentThemeImages.ExchangeTokensButtonBig = KioskStatic.GetImage(CurrentThemeImages.ExchangeTokensButtonBig, "Exchange_tokens_Button", ThemeFolder);
                CurrentThemeImages.ExchangeTokensButtonSmall = KioskStatic.GetImage(CurrentThemeImages.ExchangeTokensButtonSmall, "Exchange_tokens_Button_Small", ThemeFolder);
                CurrentThemeImages.GamePriceTable = KioskStatic.GetImage(CurrentThemeImages.GamePriceTable, "game_price_table", ThemeFolder);
                CurrentThemeImages.HomeButton = KioskStatic.GetImage(CurrentThemeImages.HomeButton, "home_button", ThemeFolder);

                if (string.IsNullOrWhiteSpace(defaultBkgImageFileNameFromConfigFile))
                {
                    defaultBkgImageFileNameFromConfigFile = "DefaultBackgroundImage";
                }
                CurrentThemeImages.DefaultBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.DefaultBackgroundImage,
                                                                       defaultBkgImageFileNameFromConfigFile, ThemeFolder);
                CurrentThemeImages.DefaultBackgroundImageTwo = KioskStatic.GetImage(CurrentThemeImages.DefaultBackgroundImageTwo,
                                                                       defaultBkgImageFileNameFromConfigFile, ThemeFolder);

                CurrentThemeImages.DefaultBackgroundImageTwo = KioskStatic.GetImage(CurrentThemeImages.DefaultBackgroundImageTwo,
                                                                       "DefaultBackgroundImage2", ThemeFolder);

                if (string.IsNullOrWhiteSpace(homeScreenBkgImageFileNameFromConfigFile))
                {
                    homeScreenBkgImageFileNameFromConfigFile = "Home_screen";
                }
                CurrentThemeImages.HomeScreenBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.HomeScreenBackgroundImage, homeScreenBkgImageFileNameFromConfigFile, ThemeFolder);
                CurrentThemeImages.InsertCash = KioskStatic.GetImage(CurrentThemeImages.InsertCash, "insert_cash", ThemeFolder);
                CurrentThemeImages.InsertCashAnimation = KioskStatic.GetImage(CurrentThemeImages.InsertCashAnimation, "InsertCash_Animation", ThemeFolder);
                CurrentThemeImages.KioskActivityTableImage = KioskStatic.GetImage(CurrentThemeImages.KioskActivityTableImage, "KioskActivityTable", ThemeFolder);
                CurrentThemeImages.LoadExistingImage = KioskStatic.GetImage(CurrentThemeImages.LoadExistingImage, "LoadExisting", ThemeFolder);
                CurrentThemeImages.NewCardButtonSmall = KioskStatic.GetImage(CurrentThemeImages.NewCardButtonSmall, "New_Play_Pass_Button", ThemeFolder);
                CurrentThemeImages.NewPlayPassButtonBig = KioskStatic.GetImage(CurrentThemeImages.NewPlayPassButtonBig, "New_Play_Pass_Button_big", ThemeFolder);
                CurrentThemeImages.NoOfCardButton = KioskStatic.GetImage(CurrentThemeImages.NoOfCardButton, "No_of_Card_Button", ThemeFolder);
                CurrentThemeImages.PassesTwo = KioskStatic.GetImage(CurrentThemeImages.PassesTwo, "Passes_Two", ThemeFolder);
                CurrentThemeImages.PlainProductButton = KioskStatic.GetImage(CurrentThemeImages.PlainProductButton, "plain_product_button", ThemeFolder);
                CurrentThemeImages.ProductTableImage = KioskStatic.GetImage(CurrentThemeImages.ProductTableImage, "product_table", ThemeFolder);
                CurrentThemeImages.PurchaseSummaryTableImage = KioskStatic.GetImage(CurrentThemeImages.PurchaseSummaryTableImage, "TablePurchaseSummary", ThemeFolder);
                CurrentThemeImages.RechargePlayPassButtonBig = KioskStatic.GetImage(CurrentThemeImages.RechargePlayPassButtonBig, "Recharge_Play_Pass_Big", ThemeFolder);
                CurrentThemeImages.RechargePlayPassButtonSmall = KioskStatic.GetImage(CurrentThemeImages.RechargePlayPassButtonSmall, "Recharge_Play_Pass_small", ThemeFolder);
                CurrentThemeImages.RedeemTable = KioskStatic.GetImage(CurrentThemeImages.RedeemTable, "CashInsertGrid", ThemeFolder);
                CurrentThemeImages.RegisterPassSmall = KioskStatic.GetImage(CurrentThemeImages.RegisterPassSmall, "register_pass_Small", ThemeFolder);
                CurrentThemeImages.RegisterPassBig = KioskStatic.GetImage(CurrentThemeImages.RegisterPassBig, "register_pass_big", ThemeFolder);
                CurrentThemeImages.SpecialOfferLogo = KioskStatic.GetImage(CurrentThemeImages.SpecialOfferLogo, "special_offer_semnox_logo", ThemeFolder);
                CurrentThemeImages.SucessAddImage = KioskStatic.GetImage(CurrentThemeImages.SucessAddImage, "sucess_Add", ThemeFolder);
                CurrentThemeImages.SucessRechargeImage = KioskStatic.GetImage(CurrentThemeImages.SucessRechargeImage, "sucess_Recharge", ThemeFolder);
                CurrentThemeImages.SucessRedeemImage = KioskStatic.GetImage(CurrentThemeImages.SucessRedeemImage, "sucess_Redeem", ThemeFolder);
                CurrentThemeImages.SucessCheckIn = KioskStatic.GetImage(CurrentThemeImages.SucessCheckIn, "sucess_CheckIn", ThemeFolder);
                CurrentThemeImages.SucessRegister = KioskStatic.GetImage(CurrentThemeImages.SucessRegister, "sucess_Register", ThemeFolder);
                CurrentThemeImages.SucessTransfer = KioskStatic.GetImage(CurrentThemeImages.SucessTransfer, "sucess_Transfer", ThemeFolder);
                CurrentThemeImages.SureWhyNotButton = KioskStatic.GetImage(CurrentThemeImages.SureWhyNotButton, "sure_why_not_button", ThemeFolder);
                CurrentThemeImages.TapCardBox = KioskStatic.GetImage(CurrentThemeImages.TapCardBox, "tap_card_box", ThemeFolder);
                CurrentThemeImages.TermsButton = KioskStatic.GetImage(CurrentThemeImages.TermsButton, "terms_button", ThemeFolder);
                CurrentThemeImages.TimerBoxSmall = KioskStatic.GetImage(CurrentThemeImages.TimerBoxSmall, "timer_SmallBox", ThemeFolder);
                CurrentThemeImages.TextEntryBox = KioskStatic.GetImage(CurrentThemeImages.TextEntryBox, "text_entry_box", ThemeFolder);
                CurrentThemeImages.SplashScreenBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.SplashScreenBackgroundImage, "Splash_Screen_Background_Image", ThemeFolder);
                if (string.IsNullOrWhiteSpace(splashScreenFileNameFromConfigFile))
                {
                    splashScreenFileNameFromConfigFile = "touch_screen_semnox_logo";
                }
                CurrentThemeImages.SplashScreenImage = KioskStatic.GetImage(CurrentThemeImages.SplashScreenImage, splashScreenFileNameFromConfigFile, ThemeFolder);
                CurrentThemeImages.ScrollUpButtonImage = KioskStatic.GetImage(CurrentThemeImages.ScrollUpButtonImage, "up_button", ThemeFolder);
                CurrentThemeImages.ScrollDownButton = KioskStatic.GetImage(CurrentThemeImages.ScrollDownButton, "down_button", ThemeFolder);
                CurrentThemeImages.TransferPointButtonBig = KioskStatic.GetImage(CurrentThemeImages.TransferPointButtonBig, "Transfer_Point", ThemeFolder);
                CurrentThemeImages.TransferPointButtonSmall = KioskStatic.GetImage(CurrentThemeImages.TransferPointButtonSmall, "Transfer_Point_Small", ThemeFolder);
                CurrentThemeImages.PauseCardButtonSmall = KioskStatic.GetImage(CurrentThemeImages.PauseCardButtonSmall, "Pause_Card_Button_Small", ThemeFolder);
                CurrentThemeImages.PauseCardButtonBig = KioskStatic.GetImage(CurrentThemeImages.PauseCardButtonBig, "Pause_Card_Button_Big", ThemeFolder);
                CurrentThemeImages.PointsToTimeButtonBig = KioskStatic.GetImage(CurrentThemeImages.PointsToTimeButtonBig, "Points_To_Time_Button_Big", ThemeFolder);
                CurrentThemeImages.PointsToTimeButtonSmall = KioskStatic.GetImage(CurrentThemeImages.PointsToTimeButtonSmall, "Points_To_Time_Button_Small", ThemeFolder);
                CurrentThemeImages.HomeScreenSemnoxLogo = KioskStatic.GetImage(CurrentThemeImages.HomeScreenSemnoxLogo, "semnox_logo", ThemeFolder);
                //CurrentThemeImages.FSKSalesButton = KioskStatic.GetImage(CurrentThemeImages.FSKSalesButton, "FSK_Sales_Button", ThemeFolder);
                CurrentThemeImages.ExecuteOnlineTrxButtonBig = KioskStatic.GetImage(CurrentThemeImages.ExecuteOnlineTrxButtonBig, "Execute_Online_Trx_Button", ThemeFolder);
                CurrentThemeImages.ExecuteOnlineTrxButtonSmall = KioskStatic.GetImage(CurrentThemeImages.ExecuteOnlineTrxButtonSmall, "Execute_Online_Trx_Button_Small", ThemeFolder);
                CurrentThemeImages.GetTransactionDetailsButton = KioskStatic.GetImage(CurrentThemeImages.GetTransactionDetailsButton, "Search_Button", ThemeFolder);
                CurrentThemeImages.RegisteredCustomerBig = KioskStatic.GetImage(CurrentThemeImages.RegisteredCustomerBig, "Registered_Customer_Big", ThemeFolder);
                CurrentThemeImages.GameCardButton = KioskStatic.GetImage(CurrentThemeImages.GameCardButton, "Game_Card_Button", ThemeFolder);
                CurrentThemeImages.FundRaiserBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.FundRaiserBackgroundImage, "FundRaiserBackgroundImage", ThemeFolder);
                CurrentThemeImages.DecreaseQtyButton = KioskStatic.GetImage(CurrentThemeImages.DecreaseQtyButton, "DecreaseQty", ThemeFolder);
                CurrentThemeImages.IncreaseQtyButton = KioskStatic.GetImage(CurrentThemeImages.IncreaseQtyButton, "IncreaseQty", ThemeFolder);
                CurrentThemeImages.DecreaseQtyDisabledButton = KioskStatic.GetImage(CurrentThemeImages.DecreaseQtyDisabledButton, "DecreaseQtyDisabled", ThemeFolder);
                CurrentThemeImages.IncreaseQtyDisabledButton = KioskStatic.GetImage(CurrentThemeImages.IncreaseQtyDisabledButton, "IncreaseQtyDisabled", ThemeFolder);
                CurrentThemeImages.TransferFromBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.TransferFromBackgroundImage, "TransferFromBackgroundImage", ThemeFolder);
                CurrentThemeImages.TransferToBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.TransferToBackgroundImage, "TransferToBackgroundImage", ThemeFolder);
                CurrentThemeImages.AdultSummaryBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.AdultSummaryBackgroundImage, "AdultSummaryBackgroundImage", ThemeFolder);
                CurrentThemeImages.CheckInCheckOutQtyScreenBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.CheckInCheckOutQtyScreenBackgroundImage, "CheckInCheckOutQtyScreenBackgroundImage", ThemeFolder);
                CurrentThemeImages.CheckInSummaryBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.CheckInSummaryBackgroundImage, "CheckInSummaryBackgroundImage", ThemeFolder);
                CurrentThemeImages.ChildSummaryBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.ChildSummaryBackgroundImage, "ChildSummaryBackgroundImage", ThemeFolder);
                CurrentThemeImages.ProcessingCheckinDetailsBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.ProcessingCheckinDetailsBackgroundImage, "ProcessingCheckinDetailsBackgroundImage", ThemeFolder);
                CurrentThemeImages.AddCustomerRelationBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.AddCustomerRelationBackgroundImage, "AddCustomerRelationBackgroundImage", ThemeFolder);
                CurrentThemeImages.AgeGateBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.AgeGateBackgroundImage, "AgeGateBackgroundImage", ThemeFolder);
                CurrentThemeImages.CustomerBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.CustomerBackgroundImage, "CustomerBackgroundImage", ThemeFolder);
                CurrentThemeImages.CustomerDashboardBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.CustomerDashboardBackgroundImage, "CustomerDashboardBackgroundImage", ThemeFolder);
                CurrentThemeImages.TermsAndConditionsBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.TermsAndConditionsBackgroundImage, "TermsAndConditionsBackgroundImage", ThemeFolder);
                CurrentThemeImages.CardCountBasicBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.CardCountBasicBackgroundImage, "CardCountBasicBackgroundImage", ThemeFolder);
                CurrentThemeImages.CashInsertBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.CashInsertBackgroundImage, "CashInsertBackgroundImage", ThemeFolder);
                CurrentThemeImages.IssueTempCardsBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.IssueTempCardsBackgroundImage, "IssueTempCardsBackgroundImage", ThemeFolder);
                CurrentThemeImages.KioskActivityDetailsBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.KioskActivityDetailsBackgroundImage, "KioskActivityDetailsBackgroundImage", ThemeFolder);
                CurrentThemeImages.PrintTransactionLinesBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.PrintTransactionLinesBackgroundImage, "PrintTransactionLinesBackgroundImage", ThemeFolder);
                CurrentThemeImages.FSKCoverPageBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.FSKCoverPageBackgroundImage, "FSKCoverPageBackgroundImage", ThemeFolder);
                CurrentThemeImages.CardCountBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.CardCountBackgroundImage, "CardCountBackgroundImage", ThemeFolder);
                CurrentThemeImages.CardTransactionBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.CardTransactionBackgroundImage, "CardTransactionBackgroundImage", ThemeFolder);
                CurrentThemeImages.ChooseProductBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.ChooseProductBackgroundImage, "ChooseProductBackgroundImage", ThemeFolder);
                CurrentThemeImages.LoadBonusBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.LoadBonusBackgroundImage, "LoadBonusBackgroundImage", ThemeFolder);
                CurrentThemeImages.PaymentGameCardBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.PaymentGameCardBackgroundImage, "PaymentGameCardBackgroundImage", ThemeFolder);
                CurrentThemeImages.PaymentModeBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.PaymentModeBackgroundImage, "PaymentModeBackgroundImage", ThemeFolder);
                CurrentThemeImages.RedeemTokensBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.RedeemTokensBackgroundImage, "RedeemTokensBackgroundImage", ThemeFolder);
                CurrentThemeImages.FSKExecuteOnlineTransactionBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.FSKExecuteOnlineTransactionBackgroundImage, "FSKExecuteOnlineTransactionBackgroundImage", ThemeFolder);
                CurrentThemeImages.CustomerDetailsForWaiverBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.CustomerDetailsForWaiverBackgroundImage, "CustomerDetailsForWaiverBackgroundImage", ThemeFolder);
                CurrentThemeImages.CustomerOptionsBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.CustomerOptionsBackgroundImage, "CustomerOptionsBackgroundImage", ThemeFolder);
                CurrentThemeImages.CustomerOTPBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.CustomerOTPBackgroundImage, "CustomerOTPBackgroundImage", ThemeFolder);
                CurrentThemeImages.CustomerSignedWaiversBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.CustomerSignedWaiversBackgroundImage, "CustomerSignedWaiversBackgroundImage", ThemeFolder);
                CurrentThemeImages.GetCustomerInputBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.GetCustomerInputBackgroundImage, "GetCustomerInputBackgroundImage", ThemeFolder);
                CurrentThemeImages.LinkRelatedCustomerBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.LinkRelatedCustomerBackgroundImage, "LinkRelatedCustomerBackgroundImage", ThemeFolder);
                CurrentThemeImages.SignWaiverFilesBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.SignWaiverFilesBackgroundImage, "SignWaiverFilesBackgroundImage", ThemeFolder);
                CurrentThemeImages.SignWaiversBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.SignWaiversBackgroundImage, "SignWaiversBackgroundImage", ThemeFolder);
                CurrentThemeImages.ViewWaiverUIBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.ViewWaiverUIBackgroundImage, "ViewWaiverUIBackgroundImage", ThemeFolder);
                CurrentThemeImages.CalenderIconImage = KioskStatic.GetImage(CurrentThemeImages.CalenderIconImage, "CalenderIcon", ThemeFolder);
                CurrentThemeImages.SucessFnBImage = KioskStatic.GetImage(CurrentThemeImages.SucessFnBImage, "SucessFnBImage", ThemeFolder);
                CurrentThemeImages.SucessCartImage = KioskStatic.GetImage(CurrentThemeImages.SucessCartImage, "SucessCartImage", ThemeFolder);
                CurrentThemeImages.ScrollLeftButton = KioskStatic.GetImage(CurrentThemeImages.ScrollLeftButton, "left_arrow", ThemeFolder);
                CurrentThemeImages.ScrollRightButton = KioskStatic.GetImage(CurrentThemeImages.ScrollRightButton, "right_arrow", ThemeFolder);
                CurrentThemeImages.PurchaseButtonSmall = KioskStatic.GetImage(CurrentThemeImages.PurchaseButtonSmall, "PurchaseButtonSmall", ThemeFolder);
                CurrentThemeImages.PurchaseButtonBig = KioskStatic.GetImage(CurrentThemeImages.PurchaseButtonBig, "PurchaseButtonBig", ThemeFolder);
                CurrentThemeImages.SignWaiverButtonBig = KioskStatic.GetImage(CurrentThemeImages.SignWaiverButtonBig, "Sign_Waiver_Button_Big", ThemeFolder);
                CurrentThemeImages.SignWaiverButtonSmall = KioskStatic.GetImage(CurrentThemeImages.SignWaiverButtonSmall, "Sign_Waiver_Button_Small", ThemeFolder);
                CurrentThemeImages.FoodAndBeverageBig = KioskStatic.GetImage(CurrentThemeImages.FoodAndBeverageBig, "FoodAndBeverageBig", ThemeFolder);
                CurrentThemeImages.FoodAndBeverageSmall = KioskStatic.GetImage(CurrentThemeImages.FoodAndBeverageSmall, "FoodAndBeverageSmall", ThemeFolder);
                CurrentThemeImages.PlaygroundEntryBig = KioskStatic.GetImage(CurrentThemeImages.PlaygroundEntryBig, "Playground_Entry_Big", ThemeFolder);
                CurrentThemeImages.PlaygroundEntrySmall = KioskStatic.GetImage(CurrentThemeImages.PlaygroundEntrySmall, "Playground_Entry_Small", ThemeFolder);
                CurrentThemeImages.WaiverSigningInstructions = KioskStatic.GetImage(CurrentThemeImages.WaiverSigningInstructions, "WaiverSigningInstructions", ThemeFolder);
                CurrentThemeImages.LoyaltyFormLogoImage = KioskStatic.GetImage(CurrentThemeImages.LoyaltyFormLogoImage, "LoyaltyFormLogo", ThemeFolder);
                CurrentThemeImages.LoyaltyBackGroundImage = KioskStatic.GetImage(CurrentThemeImages.LoyaltyBackGroundImage, "LoyaltyBackGroundImage", ThemeFolder);
                CurrentThemeImages.CalenderBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.CalenderBackgroundImage, "CalenderBackgroundImage", ThemeFolder);
                CurrentThemeImages.CalenderBtnBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.CalenderBtnBackgroundImage, "CalenderBtnBackgroundImage", ThemeFolder);
                CurrentThemeImages.ScrollUpEnabled = KioskStatic.GetImage(CurrentThemeImages.ScrollUpEnabled, "Scroll_Up_Button", ThemeFolder);
                CurrentThemeImages.ScrollUpDisabled = KioskStatic.GetImage(CurrentThemeImages.ScrollUpDisabled, "Scroll_Up_Button_Disabled", ThemeFolder);
                CurrentThemeImages.ScrollDownEnabled = KioskStatic.GetImage(CurrentThemeImages.ScrollDownEnabled, "Scroll_Down_Button", ThemeFolder);
                CurrentThemeImages.ScrollDownDisabled = KioskStatic.GetImage(CurrentThemeImages.ScrollDownDisabled, "Scroll_Down_Button_Disabled", ThemeFolder);
                CurrentThemeImages.ScrollLeftEnabled = KioskStatic.GetImage(CurrentThemeImages.ScrollLeftEnabled, "Scroll_Left_Button", ThemeFolder);
                CurrentThemeImages.ScrollLeftDisabled = KioskStatic.GetImage(CurrentThemeImages.ScrollLeftDisabled, "Scroll_Left_Button_Disabled", ThemeFolder);
                CurrentThemeImages.ScrollRightEnabled = KioskStatic.GetImage(CurrentThemeImages.ScrollRightEnabled, "Scroll_Right_Button", ThemeFolder);
                CurrentThemeImages.ScrollRightDisabled = KioskStatic.GetImage(CurrentThemeImages.ScrollRightDisabled, "Scroll_Right_Button_Disabled", ThemeFolder);
                CurrentThemeImages.BottomMessageLineImage = KioskStatic.GetImage(CurrentThemeImages.BottomMessageLineImage, "bottom_bar", ThemeFolder);
                CurrentThemeImages.DiscountButton = KioskStatic.GetImage(CurrentThemeImages.DiscountButton, "DiscountButton", ThemeFolder);
                CurrentThemeImages.SelectAdultBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.SelectAdultBackgroundImage, "SelectAdultBackground", ThemeFolder);
                CurrentThemeImages.SelectChildBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.SelectChildBackgroundImage, "SelectChildBackground", ThemeFolder);
                CurrentThemeImages.EnterAdultBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.EnterAdultBackgroundImage, "EnterAdultBackground", ThemeFolder);
                CurrentThemeImages.EnterChildBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.EnterChildBackgroundImage, "EnterChildBackground", ThemeFolder);
                CurrentThemeImages.ChooseProductButton = KioskStatic.GetImage(CurrentThemeImages.ChooseProductButton, "ChooseProductButton", ThemeFolder);
                CurrentThemeImages.BalanceBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.BalanceBackgroundImage, "BalanceBackground", ThemeFolder);
                CurrentThemeImages.BalanceAnimationImage = KioskStatic.GetImage(CurrentThemeImages.BalanceAnimationImage, "BalanceAnimation", ThemeFolder);
                CurrentThemeImages.FAQBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.FAQBackgroundImage, "FAQBackGround", ThemeFolder);
                CurrentThemeImages.TicketTypeBox = KioskStatic.GetImage(CurrentThemeImages.TicketTypeBox, "TicketTypeBox", ThemeFolder);
                CurrentThemeImages.YesNoButtons = KioskStatic.GetImage(CurrentThemeImages.YesNoButtons, "YesNoButtons", ThemeFolder);
                CurrentThemeImages.AdminBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.AdminBackgroundImage, "AdminBackGround", ThemeFolder);
                CurrentThemeImages.LanguageButton = KioskStatic.GetImage(CurrentThemeImages.LanguageButton, "LanguageButton", ThemeFolder);
                CurrentThemeImages.SelectWaiverOptionsBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.SelectWaiverOptionsBackgroundImage, "SelectWaiverOptionsBackground", ThemeFolder);
                CurrentThemeImages.CustomerSignatureConfirmationBox = KioskStatic.GetImage(CurrentThemeImages.CustomerSignatureConfirmationBox, "CustomerSignatureConfirmationBox", ThemeFolder);
                CurrentThemeImages.CustomerSignatureConfirmationButtons = KioskStatic.GetImage(CurrentThemeImages.CustomerSignatureConfirmationButtons, "CustomerSignatureConfirmationButtons", ThemeFolder);
                CurrentThemeImages.TransactionViewBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.TransactionViewBackgroundImage, "TransactionViewBackground", ThemeFolder);
                CurrentThemeImages.WaiverButtonBorderImage = KioskStatic.GetImage(CurrentThemeImages.WaiverButtonBorderImage, "WaiverButtonBorderImage", ThemeFolder);
                CurrentThemeImages.EntitlementBox = KioskStatic.GetImage(CurrentThemeImages.EntitlementBox, "EntitlementBox", ThemeFolder);
                CurrentThemeImages.OkMsgBox = KioskStatic.GetImage(CurrentThemeImages.OkMsgBox, "OkMsgBox", ThemeFolder);
                CurrentThemeImages.OkMsgBoxShort = KioskStatic.GetImage(CurrentThemeImages.OkMsgBoxShort, "OkMsgBoxShort", ThemeFolder);
                CurrentThemeImages.CardSaleOptionBox = KioskStatic.GetImage(CurrentThemeImages.CardSaleOptionBox, "CardSaleOptionBox", ThemeFolder);
                CurrentThemeImages.OkMsgButtons = KioskStatic.GetImage(CurrentThemeImages.OkMsgButtons, "OkMsgButtons", ThemeFolder);
                CurrentThemeImages.ScanCouponBox = KioskStatic.GetImage(CurrentThemeImages.ScanCouponBox, "ScanCouponBox", ThemeFolder);
                CurrentThemeImages.ScanCouponButtons = KioskStatic.GetImage(CurrentThemeImages.ScanCouponButtons, "ScanCouponButtons", ThemeFolder);
                CurrentThemeImages.TapCardButtons = KioskStatic.GetImage(CurrentThemeImages.TapCardButtons, "TapCardButtons", ThemeFolder);
                CurrentThemeImages.SuccessBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.SuccessBackgroundImage, "SuccessBackGround", ThemeFolder);
                CurrentThemeImages.YesNoBox = KioskStatic.GetImage(CurrentThemeImages.YesNoBox, "YesNoBox", ThemeFolder);
                CurrentThemeImages.TablePurchaseSummary = KioskStatic.GetImage(CurrentThemeImages.TablePurchaseSummary, "TablePurchaseSummary", ThemeFolder);
                CurrentThemeImages.KioskCartIcon = KioskStatic.GetImage(CurrentThemeImages.KioskCartIcon, "KioskCartIcon", ThemeFolder);
                CurrentThemeImages.TransferBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.TransferBackgroundImage, "TransferBackground", ThemeFolder);
                CurrentThemeImages.FundRaiserPictureBoxLogo = KioskStatic.GetImage(CurrentThemeImages.FundRaiserPictureBoxLogo, "FundRaiserPictureBoxLogo", ThemeFolder);
                CurrentThemeImages.DonationsPictureBoxLogo = KioskStatic.GetImage(CurrentThemeImages.DonationsPictureBoxLogo, "DonationsPictureBoxLogo", ThemeFolder);
                CurrentThemeImages.CartScreenBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.CartScreenBackgroundImage, "CartScreenBackground", ThemeFolder);
                CurrentThemeImages.EmailDetailsBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.EmailDetailsBackgroundImage, "EmailDetailsBackground", ThemeFolder);
                CurrentThemeImages.YesNoButtonImage = KioskStatic.GetImage(CurrentThemeImages.YesNoButtonImage, "YesNoButtonImage", ThemeFolder);
                CurrentThemeImages.PauseTimeBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.PauseTimeBackgroundImage, "PauseTimeBackGround", ThemeFolder);
                CurrentThemeImages.PauseTimeButtons = KioskStatic.GetImage(CurrentThemeImages.PauseTimeButtons, "PauseTimeButtons", ThemeFolder);
                CurrentThemeImages.CardSaleOptionButtons = KioskStatic.GetImage(CurrentThemeImages.CardSaleOptionButtons, "CardSaleOptionButtons", ThemeFolder);
                CurrentThemeImages.PreSelectPaymentBackground = KioskStatic.GetImage(CurrentThemeImages.PreSelectPaymentBackground, "PreSelectPaymentBackground", ThemeFolder);
                CurrentThemeImages.PurchaseMenuBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.PurchaseMenuBackgroundImage, "PurchaseMenuBackground", ThemeFolder);
                CurrentThemeImages.ReceiptModeBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.ReceiptModeBackgroundImage, "ReceiptModeBackgroundImage", ThemeFolder);
                CurrentThemeImages.ReceiptModeBtnBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.ReceiptModeBtnBackgroundImage, "ReceiptModeBtnBackgroundImage", ThemeFolder);
                CurrentThemeImages.UpsellBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.UpsellBackgroundImage, "UpsellBackground", ThemeFolder);
                CurrentThemeImages.CartItemBackgroundImageSmall = KioskStatic.GetImage(CurrentThemeImages.CartItemBackgroundImageSmall, "CartItemBackgroundImageSmall", ThemeFolder);
                CurrentThemeImages.CartItemBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.CartItemBackgroundImage, "CartItemBackgroundImage", ThemeFolder);
                CurrentThemeImages.CartDeleteButtonBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.CartDeleteButtonBackgroundImage, "CartDeleteButtonBackgroundImage", ThemeFolder);
                CurrentThemeImages.KioskPrintSummaryBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.KioskPrintSummaryBackgroundImage, "KioskPrintSummaryBackgroundImage", ThemeFolder);
                CurrentThemeImages.AttractionsButtonBig = KioskStatic.GetImage(CurrentThemeImages.AttractionsButtonBig, "AttractionsButtonBig", ThemeFolder);
                CurrentThemeImages.AttractionsButtonSmall = KioskStatic.GetImage(CurrentThemeImages.AttractionsButtonSmall, "AttractionsButtonSmall", ThemeFolder);
                CurrentThemeImages.AttractionQtyBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.AttractionQtyBackgroundImage, "AttractionQtyBackgroundImage", ThemeFolder);
                CurrentThemeImages.ProcessingAttractionsBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.ProcessingAttractionsBackgroundImage, "ProcessingAttractionsBackgroundImage", ThemeFolder);
                CurrentThemeImages.SelectSlotBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.SelectSlotBackgroundImage, "SelectSlotBackgroundImage", ThemeFolder);
                CurrentThemeImages.AttractionSummaryBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.AttractionSummaryBackgroundImage, "AttractionSummaryBackgroundImage", ThemeFolder);
                CurrentThemeImages.PanelYourSelections = KioskStatic.GetImage(CurrentThemeImages.PanelYourSelections, "PanelYourSelections", ThemeFolder);
                CurrentThemeImages.QuantityTextBox = KioskStatic.GetImage(CurrentThemeImages.QuantityTextBox, "QuantityTextBox", ThemeFolder);
                CurrentThemeImages.QuantityImage = KioskStatic.GetImage(CurrentThemeImages.QuantityImage, "QuantityImage", ThemeFolder);
                CurrentThemeImages.NewTickedCheckBox = KioskStatic.GetImage(CurrentThemeImages.NewTickedCheckBox, "NewTickedCheckBox", ThemeFolder);
                CurrentThemeImages.NewUnTickedCheckBox = KioskStatic.GetImage(CurrentThemeImages.NewUnTickedCheckBox, "NewUnTickedCheckBox", ThemeFolder);
                CurrentThemeImages.AttractionNewCard = KioskStatic.GetImage(CurrentThemeImages.AttractionNewCard, "AttractionNewCard", ThemeFolder);
                CurrentThemeImages.PanelCardSaleOption = KioskStatic.GetImage(CurrentThemeImages.PanelCardSaleOption, "PanelCardSaleOption", ThemeFolder);
                CurrentThemeImages.AttractionExistingCard = KioskStatic.GetImage(CurrentThemeImages.AttractionExistingCard, "AttractionExistingCard", ThemeFolder);
                CurrentThemeImages.SmallCircleImage = KioskStatic.GetImage(CurrentThemeImages.SmallCircleImage, "SmallCircleImage", ThemeFolder);
                CurrentThemeImages.AttractionsComboProductBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.AttractionsComboProductBackgroundImage, "AttractionsComboProductBackgroundImage", ThemeFolder);
                CurrentThemeImages.BigCircleSelected = KioskStatic.GetImage(CurrentThemeImages.BigCircleSelected, "BigCircleSelected", ThemeFolder);
                CurrentThemeImages.BigCircleUnSelected = KioskStatic.GetImage(CurrentThemeImages.BigCircleUnSelected, "BigCircleUnSelected", ThemeFolder);
                CurrentThemeImages.PanelTimeSection = KioskStatic.GetImage(CurrentThemeImages.PanelTimeSection, "PanelTimeSection", ThemeFolder);
                CurrentThemeImages.PanelDateSection = KioskStatic.GetImage(CurrentThemeImages.PanelDateSection, "PanelDateSection", ThemeFolder);
                CurrentThemeImages.SlotBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.SlotBackgroundImage, "SlotBackgroundImage", ThemeFolder);
                CurrentThemeImages.SlotSelectedBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.SlotSelectedBackgroundImage, "SlotSelectedBackgroundImage", ThemeFolder);
                CurrentThemeImages.CheckboxSelected = KioskStatic.GetImage(CurrentThemeImages.CheckboxSelected, "CheckboxSelected", ThemeFolder);
                CurrentThemeImages.PickDate = KioskStatic.GetImage(CurrentThemeImages.PickDate, "PickDate", ThemeFolder);
                CurrentThemeImages.VariableProductButtonImage = KioskStatic.GetImage(CurrentThemeImages.VariableProductButtonImage, "VariableProductButtonImage", ThemeFolder);
                CurrentThemeImages.AddToCartAlertButtons = KioskStatic.GetImage(CurrentThemeImages.AddToCartAlertButtons, "AddToCartAlertButtons", ThemeFolder);
                CurrentThemeImages.CustomerFoundImage = KioskStatic.GetImage(CurrentThemeImages.CustomerFoundImage, "CustomerFound", ThemeFolder);
                CurrentThemeImages.SelectCustomerUsrCtrlBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.SelectCustomerUsrCtrlBackgroundImage, "SelectCustomerUsrCtrlBackgroundImage", ThemeFolder);
                CurrentThemeImages.SelectCustomerUsrCtrlSelectedBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.SelectCustomerUsrCtrlSelectedBackgroundImage, "SelectCustomerUsrCtrlSelectedBackgroundImage", ThemeFolder);
                CurrentThemeImages.WaiverSigningAlertScreenBackground = KioskStatic.GetImage(CurrentThemeImages.WaiverSigningAlertScreenBackground, "WaiverSigningAlertScreenBackground", ThemeFolder);
                CurrentThemeImages.WaiverSigningAlertUsrCtrlBackground = KioskStatic.GetImage(CurrentThemeImages.WaiverSigningAlertUsrCtrlBackground, "WaiverAlertBackgroundImage", ThemeFolder);
                CurrentThemeImages.WaiverIconImage = KioskStatic.GetImage(CurrentThemeImages.WaiverIconImage, "WaiverIcon", ThemeFolder);
                CurrentThemeImages.PersonIcon = KioskStatic.GetImage(CurrentThemeImages.PersonIcon, "PersonIcon", ThemeFolder);
                CurrentThemeImages.PersonSelectedIcon = KioskStatic.GetImage(CurrentThemeImages.PersonSelectedIcon, "PersonSelectedIcon", ThemeFolder);
                CurrentThemeImages.PersonTickIcon = KioskStatic.GetImage(CurrentThemeImages.PersonTickIcon, "PersonTickIcon", ThemeFolder);
                CurrentThemeImages.FilteredCustomersBackground = KioskStatic.GetImage(CurrentThemeImages.FilteredCustomersBackground, "FilteredCustomersBackground", ThemeFolder);
                CurrentThemeImages.SmallCircleSelected = KioskStatic.GetImage(CurrentThemeImages.SmallCircleSelected, "SmallCircleSelected", ThemeFolder);
                CurrentThemeImages.SmallCircleUnselected = KioskStatic.GetImage(CurrentThemeImages.SmallCircleUnselected, "SmallCircleUnselected", ThemeFolder);
                CurrentThemeImages.Expand = KioskStatic.GetImage(CurrentThemeImages.Expand, "Expand", ThemeFolder);
                CurrentThemeImages.Collapse = KioskStatic.GetImage(CurrentThemeImages.Collapse, "Collapse", ThemeFolder);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method can be used for scroll up event in flowlayout Panel
        /// </summary>
        /// <param name="flp">FlowLayoutControl from the form</param>
        /// <param name="index">Variable value which will decreament in each Call of this function and passed as reference</param>
        public static void FlowLayoutScrollUp(FlowLayoutPanel flp, ref int index)
        {
            log.LogMethodEntry(flp, index);
            if (flp.Controls.Count > (flp.Height / (flp.Controls[0].Height + flp.Controls[0].Margin.Top + flp.Controls[0].Margin.Bottom)))//5 is the visible row count  without scrolling
            {
                if (index <= 0)
                    index = 1;
                else
                    index = Math.Max(index - (flp.Controls[0].Height + flp.Controls[0].Margin.Top + flp.Controls[0].Margin.Bottom), 0);

                flp.VerticalScroll.Value = index;
                flp.Refresh();
                log.LogMethodExit();
            }
        }

        /// <summary>
        /// This method can be used for scroll Down event in flowlayout Panel
        /// </summary>
        /// <param name="flp">FlowLayoutControl from the form</param>
        /// <param name="index">Variable value which will increament in each Call of this function and passed as reference</param>
        public static void FlowLayoutScrollDown(FlowLayoutPanel flp, ref int index)
        {
            log.LogMethodEntry(flp, index);
            if (flp.Controls.Count > (flp.Height / (flp.Controls[0].Height + flp.Controls[0].Margin.Top + flp.Controls[0].Margin.Bottom)))//5 is the visible row count  without scrolling
            {
                if (index <= 0)
                    index = 1;
                else
                    index = Math.Min(index + flp.Controls[0].Height + flp.Controls[0].Margin.Top + flp.Controls[0].Margin.Bottom, ((flp.Controls[0].Height + flp.Controls[0].Margin.Top + flp.Controls[0].Margin.Bottom) * (flp.Controls.Count + 1 - (flp.Height / (flp.Controls[0].Height + flp.Controls[0].Margin.Top + flp.Controls[0].Margin.Bottom)))) + 1);//5 is the visible row count  without scrolling

                flp.VerticalScroll.Value = index;
                flp.Refresh();

            }
            log.LogMethodExit();
        }


        public static void InitializeFlowLayoutVerticalScroll(FlowLayoutPanel flp, int Number_Of_Rows)
        {
            log.LogMethodEntry(flp, Number_Of_Rows);
            if (flp.Controls.Count > 0)
            {
                flp.VerticalScroll.Maximum = (Math.Max((flp.Controls.Count + 1), Number_Of_Rows) + 1) * (flp.Controls[0].Height + flp.Controls[0].Margin.Top + flp.Controls[0].Margin.Bottom);//((flp.Controls[0].Width + flp.Controls[0].Margin.Left + flp.Controls[0].Margin.Right) * ((flp.Controls.Count) + 1 - ((flp.Width * Number_Of_Rows) / (flp.Controls[0].Width + flp.Controls[0].Margin.Left + flp.Controls[0].Margin.Right)))) + 1;//NewTheme102015:Ends
                flp.VerticalScroll.SmallChange = flp.Controls[0].Height + flp.Controls[0].Margin.Top + flp.Controls[0].Margin.Bottom;
                flp.VerticalScroll.LargeChange = (flp.Controls[0].Height + flp.Controls[0].Margin.Top + flp.Controls[0].Margin.Bottom) * 2;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This function will add new line character by replacing space character.
        /// </summary>
        /// <param name="text">String variable, passing to add new line character </param>
        /// <param name="LineMaxSize">This is an integer value. This value should be the maximum number of character the container can hold in one line. </param>
        /// <returns>Returns New line character added string.</returns>
        public static string InsertNewline(string text, int LineMaxSize)
        {
            log.LogMethodEntry(text, LineMaxSize);
            string message = "";
            int count = 0;
            if (text.Length > 1)
            {
                count = text.IndexOf(' ', count + 1);
                if (count == -1)
                { message = text; }
                while (count < LineMaxSize && count != -1)
                {
                    message = text.Substring(0, count) + Environment.NewLine + text.Substring(count + 1, text.Length - count - 1);
                    count = text.IndexOf(' ', count + 1);
                }
            }
            log.LogMethodExit(message);
            return message;
        }

        public static Image GetBackgroundImage(Image backGroundImage)
        {
            log.LogMethodEntry("backGroundImage");
            if (backGroundImage == null)
            {
                log.LogMethodExit("DefaultBackgroundImage");
                return CurrentThemeImages.DefaultBackgroundImage;
            }
            else
            {
                log.LogMethodExit(backGroundImage);
                return backGroundImage;
            }
        }

        public static Image GetBackgroundImageTwo(Image backGroundImage)
        {
            log.LogMethodEntry("backGroundImage");
            if (backGroundImage == null)
            {
                log.LogMethodExit("DefaultBackgroundImageTwo");
                return CurrentThemeImages.DefaultBackgroundImageTwo;
            }
            else
            {
                log.LogMethodExit(backGroundImage);
                return backGroundImage;
            }
        }

        public static Image GetPopupBackgroundImage(Image backGroundImage)
        {
            log.LogMethodEntry("backGroundImage");
            if (backGroundImage == null)
            {
                log.LogMethodExit("Default TapCardBox");
                return CurrentThemeImages.TapCardBox;
            }
            else
            {
                log.LogMethodExit(backGroundImage);
                return backGroundImage;
            }
        }

        public static Image GetBackButtonBackgroundImage(Image backGroundImage)
        {
            log.LogMethodEntry("backGroundImage");
            if (backGroundImage == null)
            {
                log.LogMethodExit("Default TapCardButtons");
                return CurrentThemeImages.TapCardButtons;
            }
            else
            {
                log.LogMethodExit(backGroundImage);
                return backGroundImage;
            }
        }
    }
}
