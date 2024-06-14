/********************************************************************************************
* Project Name - Parafait_Kiosk - ThemeManager
* Description  - ThemeManager 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.80        4-Sep-2019      Deeksha             Added logger methods.
 *2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
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
            public Image BirthDateEntryBox;//= Properties.Resources.birthdate_entry_box;
            //public Image BottomMessageLineImage= Properties.Resources.bottom_bar;
            public Image CardReaderArrowImage = null;//Properties.Resources.card_reader;
            public Image CashButtonImage = Properties.Resources.Cash_Button;
            public Image CheckBalanceBox;//= Properties.Resources.check_balance_box;            
            public Image CheckBalanceButtonBig;//= Properties.Resources.Check_Balance_Button_Big;
            public Image CheckBalanceButtonSmall = Properties.Resources.Check_Balance;
            public Image CloseButton = Properties.Resources.close_button;
            public Image CountdownTimer = Properties.Resources.countdown_timer;
            public Image CreditCardButton = Properties.Resources.Credit_Card_Button;
            public Image HomeScreenClientLogo;//= null;
            public Image DebitCardButton = Properties.Resources.Debit_Card_Button;
            public Image DoneButton = Properties.Resources.done_button;
            public Image ExchangeTokensButtonBig;//= Properties.Resources.Exchange_tokens_Button;
            public Image ExchangeTokensButtonSmall = Properties.Resources.Redeem_Tokens;
            public Image GamePriceTable = Properties.Resources.game_price_table;
            public Image HomeButton = Properties.Resources.home_button;
            //public Image HomeScreenBackgroundImage= Properties.Resources.Home_Screen;
            public Image InsertCash;//= Properties.Resources.insert_cash;
            public Image InsertCashAnimation = Properties.Resources.InsertCash_Animation;
            public Image KioskActivityTableImage = Properties.Resources.KioskActivityTable;
            public Image LoadExistingImage = Properties.Resources.LoadExisting;
            public Image NewCardButtonSmall = Properties.Resources.New_Play_Pass_Button;
            public Image NewPlayPassButtonBig = Properties.Resources.New_Card_Big;
            public Image NoOfCardButton = Properties.Resources.No_of_Card_Button;
            public Image PassesTwo = Properties.Resources.Passes_Two;
            public Image PlainProductButton = Properties.Resources.plain_product_button;
            public Image ProductTableImage = Properties.Resources.product_table;
            public Image PurchaseSummaryTableImage = Properties.Resources.TablePurchaseSummary;
            public Image RechargePlayPassButtonBig = Properties.Resources.Recharge_Card_Big;
            public Image RedeemTable;//= Properties.Resources.table3;
            public Image RegisterPassSmall = Properties.Resources.Register_pass;
            public Image RegisterPassBig;//= Properties.Resources.register_pass_big;
            public Image SpecialOfferLogo;//= Properties.Resources.special_offer_semnox_logo;
            public Image SpecialOfferButton;//= Properties.Resources.special_offer_semnox_logo; 
            public Image SucessAddImage = Properties.Resources.sucess_Add;
            public Image SucessRechargeImage = Properties.Resources.sucess_Recharge;
            public Image SucessRedeemImage = Properties.Resources.sucess_Redeem;
            public Image SucessRegister = Properties.Resources.sucess_Register;
            public Image SucessCheckIn = Properties.Resources.sucessCheckIn;
            public Image SucessTransfer = Properties.Resources.sucess_Transfer;
            public Image SucessFnBImage = Properties.Resources.SucessFnBImage;
            public Image SucessCartImage = Properties.Resources.SucessCartImage;
            public Image SureWhyNotButton;//= Properties.Resources.sure_button;
            //public Image TapCardBox= Properties.Resources.tap_card_box;
            public Image TermsButton = Properties.Resources.terms_button;
            public Image TimerBoxSmall = Properties.Resources.timer_SmallBox;
            public Image TextEntryBox = Properties.Resources.text_entry_box;
            public Image SplashScreenBackgroundImage = null;//= Properties.Resources.Home_screen;
            public Image SplashScreenImage = null;//= Properties.Resources.touch_screen_semnox_logo;
            public Image ScrollUpButton = Properties.Resources.Up_Button;
            public Image ScrollDownButton = Properties.Resources.Down_Button;
            public Image ScrollLeftButton = Properties.Resources.Left_Button;
            public Image ScrollRightButton = Properties.Resources.Right_Button;
            public Image TransferPointButtonBig;//= Properties.Resources.Transfer_Point;
            public Image TransferPointButtonSmall = Properties.Resources.Transfer_Points;
            //public Image YesorNoFormBackgroundBox= Properties.Resources.YesorNoBackground;
            public Image NewPlayPassButtonSmall = Properties.Resources.New_Card_Small;
            public Image RechargePlayPassButtonSmall = Properties.Resources.Recharge_Card_Small;
            public Image PauseCardImage = Properties.Resources.Pause_Card;
            public Image PointsToTimeConvertionImage = Properties.Resources.Points_To_Time;
            public Image GameCardButton = Properties.Resources.Game_Card_Button;
            public Image HomeScreenSemnoxLogo = null;
            //public Image ExecuteOnlineTrxButtonBig = Properties.Resources.Execute_Online_Trx_Button;
            //public Image ExecuteOnlineTrxButtonSmall = Properties.Resources.Execute_Online_Trx_Button_Small;
            //public Image ExecuteOnlineTrxButtonMedium = Properties.Resources.Execute_Online_Trx_Button_Medium;
            //public Image FSKSalesButton = Properties.Resources.FSK_Sales_Button;
            //public Image GetTransactionDetailsButton = Properties.Resources.Search_Button; 
            //public Image AddRelationButtonImage = Properties.Resources.AddRelation_button_box;
            //public Image DecreaseQtyButton = Properties.Resources.DecreaseQty;
            //public Image IncreaseQtyButton = Properties.Resources.IncreaseQty;
            //public Image DecreaseQtyDisabledButton = Properties.Resources.DecreaseQtyDisabled;
            //public Image IncreaseQtyDisabledButton = Properties.Resources.IncreaseQtyDisabled;
            //public Image NewPlayPassButtonSmall = Properties.Resources.New_Play_Pass_Button;
            //public Image NewPlayPassButtonMedium = Properties.Resources.New_Play_Pass_Button_Medium;
            //public Image PurchaseButtonSmall = Properties.Resources.Purchase_Button_Small;
            //public Image PurchaseButtonMedium = Properties.Resources.Purchase_Button_Medium;
            //public Image PurchaseButtonBig = Properties.Resources.Purchase_Button_Big;
            //public Image SignWaiverButtonSmall = Properties.Resources.Sign_Waiver_Small;
            //public Image SignWaiverButtonMedium = Properties.Resources.Sign_Waiver_Medium;
            //public Image SignWaiverButtonBig = Properties.Resources.Sign_Waiver_Big;
            //public Image FoodAndBeverageSmall = Properties.Resources.FoodAndBeverageSmall;
            //public Image FoodAndBeverageMedium = Properties.Resources.FoodAndBeverageMedium;
            //public Image FoodAndBeverageBig = Properties.Resources.FoodAndBeverageBig;
            //public Image PlaygroundEntrySmall = Properties.Resources.Playground_Entry_Small;
            //public Image PlaygroundEntryMedium = Properties.Resources.Playground_Entry_Medium;
            //public Image PlaygroundEntryBig = Properties.Resources.Playground_Entry_Big;
            //public Image RechargePlayPassButtonMedium = Properties.Resources.Recharge_Play_Pass_Medium;
            //public Image TransferPointButtonMedium = Properties.Resources.Transfer_Point_Medium;
            //public Image RegisterPassMedium = Properties.Resources.register_pass_Medium;
            //public Image CheckBalanceButtonMedium = Properties.Resources.Check_Balance_Button_Medium;
            //public Image ExchangeTokensButtonMedium = Properties.Resources.Exchange_tokens_Button_Medium;
            //public Image PauseCardButtonMedium = Properties.Resources.Pause_Card_Button_Medium;
            //public Image PointsToTimeButtonMedium = Properties.Resources.Points_To_Time_Button_Medium;
            public Image WaiverSigningInstructions = Properties.Resources.WaiverSigningInstructions;
            //public Image LoyaltyFormLogoImage = Properties.Resources.LoyaltyFormLogoImage; 
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
            //public Image ChooseProductButton = Properties.Resources.plain_product_button_1;
            //public Image BalanceAnimationImage = null; 
            //public Image LanguageButton = Properties.Resources.Language_Btn; 
            //public Image WaiverButtonBorderImage = Properties.Resources.WaiverButtonBorder;  
            //public Image TapCardButtons = Properties.Resources.close_button;
            public Image TablePurchaseSummary = Properties.Resources.TablePurchaseSummary;
            //public Image KioskCartIcon = Properties.Resources.KioskCartIcon;
            //public Image FundRaiserPictureBoxLogo = null;
            //public Image DonationsPictureBoxLogo = null;
            //public Image YesNoButtonImage = Properties.Resources.btnYes_image; 
            //public Image ReceiptModeBtnBackgroundImage = Properties.Resources.ReceiptModeOption;
            public Image CartItemBackgroundImageSmall = Properties.Resources.WhiteBackgroundSmall;
            public Image CartItemBackgroundImage = Properties.Resources.WhiteBackground;
            public Image CartDeleteButtonBackgroundImage = Properties.Resources.NewDeleteButton;

            //Background images
            public Image HomeScreenBackgroundImage = Properties.Resources.Home_Screen;
            public Image DefaultBackgroundImage = Properties.Resources.Home_Screen;
            public Image DefaultBackgroundImageTwo = Properties.Resources.Home_Screen;
            //public Image FundRaiserBackgroundImage = null;
            //public Image TransferFromBackgroundImage = null;
            //public Image TransferToBackgroundImage = null;
            //public Image AdultSummaryBackgroundImage = null;
            //public Image CheckInCheckOutQtyScreenBackgroundImage = null;
            //public Image CheckInSummaryBackgroundImage = null;
            //public Image ChildSummaryBackgroundImage = null;
            //public Image ProcessingCheckinDetailsBackgroundImage = null;
            //public Image AddCustomerRelationBackgroundImage = null;
            //public Image AgeGateBackgroundImage = null;
            //public Image CustomerBackgroundImage = null;
            //public Image CustomerDashboardBackgroundImage = null;
            //public Image TermsAndConditionsBackgroundImage = null;
            //public Image CardCountBasicBackgroundImage = null;
            //public Image CashInsertBackgroundImage = null;
            //public Image IssueTempCardsBackgroundImage = null;
            //public Image KioskActivityDetailsBackgroundImage = null;
            //public Image PrintTransactionLinesBackgroundImage = null;
            //public Image FSKCoverPageBackgroundImage = null;
            //public Image CardCountBackgroundImage = null;
            //public Image CardTransactionBackgroundImage = null;
            //public Image ChooseProductBackgroundImage = null;
            //public Image LoadBonusBackgroundImage = null;
            //public Image PaymentGameCardBackgroundImage = null;
            //public Image PaymentModeBackgroundImage = null;
            //public Image RedeemTokensBackgroundImage = null;
            //public Image FSKExecuteOnlineTransactionBackgroundImage = null;
            //public Image CustomerDetailsForWaiverBackgroundImage = null;
            //public Image CustomerOptionsBackgroundImage = null;
            //public Image CustomerOTPBackgroundImage = null;
            //public Image CustomerSignedWaiversBackgroundImage = null;
            //public Image GetCustomerInputBackgroundImage = null;
            //public Image LinkRelatedCustomerBackgroundImage = null;
            //public Image SignWaiverFilesBackgroundImage = null;
            //public Image SignWaiversBackgroundImage = null;
            //public Image ViewWaiverUIBackgroundImage = null;
            //public Image SelectAdultBackgroundImage = null;
            //public Image SelectChildBackgroundImage = null;
            //public Image EnterAdultBackgroundImage = null;
            //public Image EnterChildBackgroundImage = null;
            //public Image BalanceBackgroundImage = null;
            //public Image FAQBackgroundImage = null;
            //public Image TransactionViewBackgroundImage = null;
            //public Image SelectWaiverOptionsBackgroundImage = null;
            public Image SuccessBackgroundImage = null;
            //public Image TransferBackgroundImage = null;
            //public Image EmailDetailsBackgroundImage = null;
            //public Image CartScreenBackgroundImage = null;
            //public Image PurchaseMenuBackgroundImage = null;
            //public Image UpsellBackgroundImage = null;
            //public Image KioskPrintSummaryBackgroundImage = null;

            public Image TapCardBox = Properties.Resources.tap_card_box;
            //public Image CalenderBackgroundImage = null;
            //public Image TicketTypeBox = null;
            //public Image CustomerSignatureConfirmationBox = null;
            //public Image EntitlementBox = null;
            //public Image OkMsgBox = null;
            //public Image ScanCouponBox = null;
            //public Image YesNoBox = null;
            //public Image ReceiptModeBackgroundImage = null;

            public Image YesorNoFormBackgroundBox = Properties.Resources.YesorNoBackground;
            //public Image AdminBackgroundImage = null;
            //public Image LoyaltyFormBackgroundImage = Properties.Resources.LoyaltyFormBackgroundImage;
            //public Image PauseTimeBackgroundImage = Properties.Resources.pause_time_box;
            //public Image PreSelectPaymentBackground = Properties.Resources.PreSelectPaymentBackground;

            // Popup Buttons
            public Image TapCardButtons = Properties.Resources.close_button;
            //public Image CalenderBtnBackgroundImage = null;
            //public Image YesNoButtons = null;
            //public Image CustomerSignatureConfirmationButtons = null;
            //public Image OkMsgButtons = null;
            //public Image ScanCouponButtons = null;
            //public Image PauseTimeButtons = null;
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
            KioskStatic.GetImageNamesFromConfigFile(Theme, out homeScreenBkgImageFileNameFromConfigFile, out defaultBkgImageFileNameFromConfigFile, out splashScreenFileNameFromConfigFile);

            CurrentThemeImages = new ThemeImages();
            string ThemeFolder = Application.StartupPath + @"\Media\Images\Theme" + Theme;

            if (Directory.Exists(ThemeFolder))
            {

                CurrentThemeImages.BackButtonImage = KioskStatic.GetImage(CurrentThemeImages.BackButtonImage, "Back_button_box", ThemeFolder); 
                CurrentThemeImages.BirthDateEntryBox = KioskStatic.GetImage(CurrentThemeImages.BirthDateEntryBox, "birthdate_entry_box", ThemeFolder); 
                CurrentThemeImages.BottomMessageLineImage = KioskStatic.GetImage(CurrentThemeImages.BottomMessageLineImage, "bottom_bar", ThemeFolder); 
                CurrentThemeImages.CardReaderArrowImage = KioskStatic.GetImage(CurrentThemeImages.CardReaderArrowImage, "card_reader", ThemeFolder); 
                CurrentThemeImages.CashButtonImage = KioskStatic.GetImage(CurrentThemeImages.CashButtonImage, "Cash_Button", ThemeFolder); 
                CurrentThemeImages.CheckBalanceBox = KioskStatic.GetImage(CurrentThemeImages.CheckBalanceBox, "check_balance_box", ThemeFolder); 
                CurrentThemeImages.CheckBalanceButtonBig = KioskStatic.GetImage(CurrentThemeImages.CheckBalanceButtonBig, "Check_Balance_Button_Big", ThemeFolder); 
                CurrentThemeImages.CheckBalanceButtonSmall = KioskStatic.GetImage(CurrentThemeImages.CheckBalanceButtonSmall, "Check_Balance_Button_Small", ThemeFolder); 
                CurrentThemeImages.CloseButton = KioskStatic.GetImage(CurrentThemeImages.CloseButton, "close_button", ThemeFolder); 
                CurrentThemeImages.CountdownTimer = KioskStatic.GetImage(CurrentThemeImages.CountdownTimer, "countdown_timer", ThemeFolder); 
                CurrentThemeImages.CreditCardButton = KioskStatic.GetImage(CurrentThemeImages.CreditCardButton, "credit_Card_button", ThemeFolder); 
                CurrentThemeImages.HomeScreenClientLogo = KioskStatic.GetImage(CurrentThemeImages.HomeScreenClientLogo, "HomeScreenClientLogo", ThemeFolder); 
                CurrentThemeImages.DebitCardButton = KioskStatic.GetImage(CurrentThemeImages.DebitCardButton, "debit_card_button", ThemeFolder); 
                CurrentThemeImages.DoneButton = KioskStatic.GetImage(CurrentThemeImages.DoneButton, "done_button", ThemeFolder); 
                CurrentThemeImages.ExchangeTokensButtonBig = KioskStatic.GetImage(CurrentThemeImages.ExchangeTokensButtonBig, "Exchange_tokens_Button", ThemeFolder); 
                CurrentThemeImages.ExchangeTokensButtonSmall = KioskStatic.GetImage(CurrentThemeImages.ExchangeTokensButtonSmall, "Redeem_Tokens", ThemeFolder); 
                CurrentThemeImages.GamePriceTable = KioskStatic.GetImage(CurrentThemeImages.GamePriceTable, "game_price_table", ThemeFolder); 
                CurrentThemeImages.HomeButton = KioskStatic.GetImage(CurrentThemeImages.HomeButton, "home_button", ThemeFolder); 
                CurrentThemeImages.HomeScreenBackgroundImage = KioskStatic.GetImage(CurrentThemeImages.HomeScreenBackgroundImage, "Home_screen", ThemeFolder); 
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
                CurrentThemeImages.RedeemTable = KioskStatic.GetImage(CurrentThemeImages.RedeemTable, "CashInsertGrid", ThemeFolder); 
                CurrentThemeImages.RegisterPassSmall = KioskStatic.GetImage(CurrentThemeImages.RegisterPassSmall, "register_pass_Small", ThemeFolder); 
                CurrentThemeImages.RegisterPassBig = KioskStatic.GetImage(CurrentThemeImages.RegisterPassBig, "register_pass_big", ThemeFolder); 
                CurrentThemeImages.SpecialOfferLogo = KioskStatic.GetImage(CurrentThemeImages.SpecialOfferLogo, "special_offer_semnox_logo", ThemeFolder); 
                CurrentThemeImages.SpecialOfferButton = KioskStatic.GetImage(CurrentThemeImages.SpecialOfferButton, "Special_Offer_Button", ThemeFolder); 
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
                CurrentThemeImages.SplashScreenImage = KioskStatic.GetImage(CurrentThemeImages.SplashScreenImage, "Splash_Screen_Image", ThemeFolder); 
                CurrentThemeImages.ScrollUpButton = KioskStatic.GetImage(CurrentThemeImages.ScrollUpButton, "Up_Button", ThemeFolder); 
                CurrentThemeImages.ScrollDownButton = KioskStatic.GetImage(CurrentThemeImages.ScrollDownButton, "Down_Button", ThemeFolder); 
                CurrentThemeImages.ScrollLeftButton = KioskStatic.GetImage(CurrentThemeImages.ScrollLeftButton, "Left_Button", ThemeFolder); 
                CurrentThemeImages.ScrollRightButton = KioskStatic.GetImage(CurrentThemeImages.ScrollRightButton, "Right_Button", ThemeFolder); 
                CurrentThemeImages.TransferPointButtonBig = KioskStatic.GetImage(CurrentThemeImages.TransferPointButtonBig, "Transfer_Point", ThemeFolder); 
                CurrentThemeImages.TransferPointButtonSmall = KioskStatic.GetImage(CurrentThemeImages.TransferPointButtonSmall, "Transfer_Point_Small", ThemeFolder); 
                CurrentThemeImages.YesorNoFormBackgroundBox = KioskStatic.GetImage(CurrentThemeImages.YesorNoFormBackgroundBox, "YesorNoBackground", ThemeFolder); 
                CurrentThemeImages.NewPlayPassButtonSmall = KioskStatic.GetImage(CurrentThemeImages.NewPlayPassButtonSmall, "New_Play_Pass_Button_Small", ThemeFolder); 
                CurrentThemeImages.RechargePlayPassButtonSmall = KioskStatic.GetImage(CurrentThemeImages.RechargePlayPassButtonSmall, "Recharge_Play_Pass_Button_Small", ThemeFolder);
                CurrentThemeImages.PauseCardImage = KioskStatic.GetImage(CurrentThemeImages.PauseCardImage, "Pause_Card_Image", ThemeFolder);
                CurrentThemeImages.PointsToTimeConvertionImage = KioskStatic.GetImage(CurrentThemeImages.PointsToTimeConvertionImage, "Points_To_Time_Convertion_Image", ThemeFolder);
                CurrentThemeImages.HomeScreenSemnoxLogo = KioskStatic.GetImage(CurrentThemeImages.HomeScreenSemnoxLogo, "semnox_logo", ThemeFolder);
                CurrentThemeImages.GameCardButton = KioskStatic.GetImage(CurrentThemeImages.GameCardButton, "Game_Card_Button", ThemeFolder);
            }
            log.LogMethodExit();
        }

        private static bool FileExists(string themeFolder, string fileName)
        {
            log.LogMethodEntry(themeFolder, fileName);
            bool isFileExists = false;
            DirectoryInfo imageDIR = new DirectoryInfo(string.IsNullOrWhiteSpace(themeFolder) ? Application.StartupPath + @"\Media\Images\" : themeFolder);
            FileInfo[] filesInDir = imageDIR.GetFiles(fileName + ".*", SearchOption.TopDirectoryOnly);

            if (filesInDir != null && filesInDir.Length > 0)
            {
                isFileExists = true;
            }
            log.LogMethodExit(isFileExists);
            return isFileExists;
        }

        //private static void SetDefaultBackgroundImage()
        //{
        //    CurrentThemeImages.TransferFromBackgroundImage =
        //        CurrentThemeImages.TransferToBackgroundImage =
        //        CurrentThemeImages.AdultSummaryBackgroundImage =
        //        CurrentThemeImages.CheckInCheckOutQtyScreenBackgroundImage =
        //        CurrentThemeImages.CheckInSummaryBackgroundImage =
        //        CurrentThemeImages.ChildSummaryBackgroundImage =
        //        CurrentThemeImages.ProcessingCheckinDetailsBackgroundImage =
        //        CurrentThemeImages.AddCustomerRelationBackgroundImage =
        //        CurrentThemeImages.AgeGateBackgroundImage =
        //        CurrentThemeImages.CustomerBackgroundImage =
        //        CurrentThemeImages.CustomerDashboardBackgroundImage =
        //        CurrentThemeImages.TermsAndConditionsBackgroundImage =
        //        CurrentThemeImages.CardCountBasicBackgroundImage =
        //        CurrentThemeImages.CashInsertBackgroundImage =
        //        CurrentThemeImages.IssueTempCardsBackgroundImage =
        //        CurrentThemeImages.KioskActivityDetailsBackgroundImage =
        //        CurrentThemeImages.PrintTransactionLinesBackgroundImage =
        //        CurrentThemeImages.FSKCoverPageBackgroundImage =
        //        CurrentThemeImages.CardCountBackgroundImage =
        //        CurrentThemeImages.CardTransactionBackgroundImage =
        //        CurrentThemeImages.ChooseProductBackgroundImage =
        //        CurrentThemeImages.LoadBonusBackgroundImage =
        //        CurrentThemeImages.PaymentGameCardBackgroundImage =
        //        CurrentThemeImages.PaymentModeBackgroundImage =
        //        CurrentThemeImages.RedeemTokensBackgroundImage =
        //        CurrentThemeImages.FSKExecuteOnlineTransactionBackgroundImage =
        //        CurrentThemeImages.FundRaiserBackgroundImage =
        //        CurrentThemeImages.CustomerDetailsForWaiverBackgroundImage =
        //        CurrentThemeImages.CustomerOptionsBackgroundImage =
        //        CurrentThemeImages.CustomerOTPBackgroundImage =
        //        CurrentThemeImages.CustomerSignedWaiversBackgroundImage =
        //        CurrentThemeImages.GetCustomerInputBackgroundImage =
        //        CurrentThemeImages.LinkRelatedCustomerBackgroundImage =
        //        CurrentThemeImages.SignWaiverFilesBackgroundImage =
        //        CurrentThemeImages.SignWaiversBackgroundImage =
        //        CurrentThemeImages.ViewWaiverUIBackgroundImage =
        //        CurrentThemeImages.BalanceBackgroundImage =
        //        CurrentThemeImages.TransferBackgroundImage =
        //        CurrentThemeImages.UpsellBackgroundImage =
        //        CurrentThemeImages.CartScreenBackgroundImage =
        //        CurrentThemeImages.EmailDetailsBackgroundImage =
        //        CurrentThemeImages.PurchaseMenuBackgroundImage =
        //        CurrentThemeImages.FAQBackgroundImage =
        //        CurrentThemeImages.SuccessBackgroundImage =
        //        CurrentThemeImages.SelectAdultBackgroundImage =
        //        CurrentThemeImages.SelectChildBackgroundImage =
        //        CurrentThemeImages.EnterAdultBackgroundImage =
        //        CurrentThemeImages.EnterChildBackgroundImage =
        //        CurrentThemeImages.SelectWaiverOptionsBackgroundImage =
        //        CurrentThemeImages.TransactionViewBackgroundImage =
        //        CurrentThemeImages.KioskPrintSummaryBackgroundImage = CurrentThemeImages.DefaultBackgroundImage;
        //}


        /// <summary>
        /// This method can be used for scroll up event in flowlayout Panel
        /// </summary>
        /// <param name="flp">FlowLayoutControl from the form</param>
        /// <param name="index">Variable value which will decreament in each Call of this function and passed as reference</param>

        public static void FlowLayoutScrollUp(FlowLayoutPanel flp, ref int index)
        {
            log.LogMethodEntry(flp, index);
            if (flp.Controls.Count > (flp.Height / flp.Controls[0].Height))//5 is the visible row count  without scrolling
            {
                if (index <= 0)
                    index = 1;
                else
                    index = Math.Max(index - (flp.Controls[0].Height + 1), 0);

                flp.VerticalScroll.Value = index;
                flp.Refresh();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method can be used for scroll Down event in flowlayout Panel
        /// </summary>
        /// <param name="flp">FlowLayoutControl from the form</param>
        /// <param name="index">Variable value which will increament in each Call of this function and passed as reference</param>
        public static void FlowLayoutScrollDown(FlowLayoutPanel flp, ref int index)
        {
            log.LogMethodEntry(flp, index);
            if (flp.Controls.Count > (flp.Height / flp.Controls[0].Height))//5 is the visible row count  without scrolling
            {
                if (index <= 0)
                    index = 1;
                else
                    index = Math.Min(index + flp.Controls[0].Height + 1, ((flp.Controls[0].Height + 1) * (flp.Controls.Count + 1 - (flp.Height / flp.Controls[0].Height))) + 1);//5 is the visible row count  without scrolling

                flp.VerticalScroll.Value = index;
                flp.Refresh();

            }
            log.LogMethodExit();
        }
        /// <summary>
        /// This method can be used only for flow layout control when the child control size of both the rows are same.
        /// </summary>
        /// <param name="flp">FlowLayoutControl from the form</param>
        /// <param name="Number_Of_Rows">The No of rows the flowlayout control is displaying.</param>
        public static void InitializeFlowLayoutHorizontalScroll(FlowLayoutPanel flp, int Number_Of_Rows)
        {
            log.LogMethodEntry(flp, Number_Of_Rows);
            if (flp.Controls.Count > 0)
            {
                //flp.HorizontalScroll.Maximum = ((flp.Controls[0].Width + 1) * ((flp.Controls.Count) + 1 - ((flp.Width * Number_Of_Rows) / (flp.Controls[0].Width+ flp.Controls[0].Margin.Left+ flp.Controls[0].Margin.Right)))) + 1;//NewTheme102015:Ends
                flp.HorizontalScroll.Maximum = (Math.Max((flp.Controls.Count + 1) / 2, 3) + 1) * (flp.Controls[0].Width + flp.Controls[0].Margin.Left + flp.Controls[0].Margin.Right);//((flp.Controls[0].Width + flp.Controls[0].Margin.Left + flp.Controls[0].Margin.Right) * ((flp.Controls.Count) + 1 - ((flp.Width * Number_Of_Rows) / (flp.Controls[0].Width + flp.Controls[0].Margin.Left + flp.Controls[0].Margin.Right)))) + 1;//NewTheme102015:Ends
                flp.HorizontalScroll.SmallChange = flp.Controls[0].Width + flp.Controls[0].Margin.Left + flp.Controls[0].Margin.Right;
                flp.HorizontalScroll.LargeChange = (flp.Controls[0].Width + flp.Controls[0].Margin.Left + flp.Controls[0].Margin.Right) * 2;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method can be used for scroll left event in flowlayout Panel
        /// </summary>
        /// <param name="flp">FlowLayoutControl from the form</param>
        /// <param name="index">Variable value which will decreament in each Call of this function and passed as reference</param>
        public static void FlowLayoutScrollLeft(FlowLayoutPanel flp, ref int index)
        {
            log.LogMethodEntry(flp, index);
            if (flp.Controls.Count > (flp.Width / flp.Controls[0].Width))//5 is the visible row count  without scrolling
            {
                if (index <= 0)
                    index = 1;
                else
                    index = Math.Max(index - (flp.Controls[0].Width + flp.Controls[0].Margin.Left + flp.Controls[0].Margin.Right), 0);

                flp.HorizontalScroll.Value = index;
                flp.Refresh();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method can be used for scroll right event in flowlayout Panel
        /// </summary>
        /// <param name="flp">FlowLayoutControl from the form</param>
        /// <param name="index">Variable value which will increament in each Call of this function and passed as reference</param>
        public static void FlowLayoutScrollRight(FlowLayoutPanel flp, ref int index)
        {
            log.LogMethodEntry(flp, index);
            if (flp.Controls.Count > ((flp.Width * 2) / flp.Controls[0].Width))//5 is the visible row count  without scrolling
            {
                if (index <= 0)
                    index = 1;
                else
                    index = Math.Min(index + flp.Controls[0].Width + flp.Controls[0].Margin.Left + flp.Controls[0].Margin.Right, ((flp.Controls[0].Width + 1) * ((flp.Controls.Count) + 1 - ((flp.Width * 2) / flp.Controls[0].Width))) + 1);//5 is the visible row count  without scrolling

                flp.HorizontalScroll.Value = index;
                flp.Refresh();

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
                log.LogMethodExit("DefaultBackgroundImageOne");
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
            Image backGroundImageValue = backGroundImage; 
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
