/********************************************************************************************
 * Project Name -  Semnox.Parafait.KioskCore
 * Description  - HomeScreenOptionValues
 * 
 **************
 **Version Log
 **************
 *Version      Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.150.6.0    26-Oct-2023   Guru S A             Created for Dynamic home screen options
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages; 

namespace Semnox.Parafait.KioskCore
{

    /// <summary>
    /// HomeScreenOptionList
    /// </summary> 
    public static class HomeScreenOptionValues
    {
        /// <summary>
        /// 1S - PurchaseButtonSmall
        /// </summary>
        public const string OneSmall = "1S";
        /// <summary>
        /// 1M - PurchaseButtonMedium
        /// </summary>
        public const string OneMedium = "1M";
        /// <summary>
        /// 1B - PurchaseButtonBig
        /// </summary>
        public const string OneBig = "1B";
        /// <summary>
        /// 2S = Check_Balance_Button_Small
        /// </summary>
        public const string TwoSmall = "2S";
        /// <summary>
        /// 2M - CheckBalanceButtonMedium
        /// </summary>
        public const string TwoMedium = "2M";
        /// <summary>
        /// Check_Balance_Button_Big
        /// </summary>
        public const string TwoBig = "2B";
        /// <summary>
        /// 3S = register_pass_Small
        /// </summary>
        public const string ThreeSmall = "3S";
        /// <summary>
        /// 3M - RegisterPassMedium
        /// </summary>
        public const string ThreeMedium = "3M";
        /// <summary>
        /// 3B = register_pass_big	
        /// </summary>
        public const string ThreeBig = "3B";
        // <summary>
        /// 4S = Transfer_Point_Small
        /// </summary>
        public const string FourSmall = "4S";
        /// <summary>
        /// 4M - TransferPointButtonMedium
        /// </summary>
        public const string FourMedium = "4M";
        /// <summary>
        /// 4B = Transfer_Point	
        /// </summary>
        public const string FourBig = "4B";
        // <summary>
        /// 5S = Points_To_Time_Button_Small
        /// </summary>
        public const string FiveSmall = "5S";
        /// <summary>
        /// 5M - PointsToTimeButtonMedium
        /// </summary>
        public const string FiveMedium = "5M";
        /// <summary>
        /// 5B = Points_To_Time_Button_Big
        /// </summary>
        public const string FiveBig = "5B";
        // <summary>
        /// 6S = Pause_Card_Button_Small
        /// </summary>
        public const string SixSmall = "6S";
        /// <summary>
        /// 6M - PauseCardButtonMedium
        /// </summary>
        public const string SixMedium = "6M";
        /// <summary>
        ///6B = Pause_Card_Button_Big
        /// </summary>
        public const string SixBig = "6B";
        // <summary>
        /// 7S = Execute_Online_Trx_Button_Small
        /// </summary>
        public const string SevenSmall = "7S";
        /// <summary>
        /// 7M - ExecuteOnlineTrxButtonMedium
        /// </summary>
        public const string SevenMedium = "7M";
        /// <summary>
        /// 7B = Execute_Online_Trx_Button
        /// </summary>
        public const string SevenBig = "7B";
        // <summary>
        /// 8S = Sign_Waiver_Button_Small
        /// </summary>
        public const string EightSmall = "8S";
        /// <summary>
        /// 8M - SignWaiverButtonMedium
        /// </summary>
        public const string EightMedium = "8M";
        /// <summary>
        /// 8B = Sign_Waiver_Button_Big
        /// </summary>
        public const string EightBig = "8B";
        // <summary>
        /// 9S = Exchange_tokens_Button_Small
        /// </summary>
        public const string NineSmall = "9S";
        /// <summary>
        /// 9M - ExchangeTokensButtonMedium
        /// </summary>
        public const string NineMedium = "9M";
        /// <summary>
        /// 9B = Exchange_tokens_Button
        /// </summary>
        public const string NineBig = "9B";
        // <summary>
        /// 10S = New_Play_Pass_Button
        /// </summary>
        public const string TenSmall = "10S";
        /// <summary>
        /// 10M - NewPlayPassButtonMedium
        /// </summary>
        public const string TenMedium = "10M";
        /// <summary>
        /// 10B = New_Play_Pass_Button_big
        /// </summary>
        public const string TenBig = "10B";
        // <summary>
        /// 11S = Recharge_Play_Pass_small
        /// </summary>
        public const string ElevenSmall = "11S";
        /// <summary>
        /// 11M - RechargePlayPassButtonMedium
        /// </summary>
        public const string ElevenMedium = "11M";
        /// <summary>
        /// 11B = Recharge_Play_Pass_Big
        /// </summary>
        public const string ElevenBig = "11B";
        // <summary>
        /// 12S = AttractionsButtonSmall
        /// </summary>
        public const string TwelveSmall = "12S";
        /// <summary>
        /// 12M - AttractionsButtonMedium
        /// </summary>
        public const string TwelveMedium = "12M";
        /// <summary>
        /// 12B = AttractionsButtonBig
        /// </summary>
        public const string TwelveBig = "12B";
        // <summary>
        /// 13S = FoodAndBeverageSmall
        /// </summary>
        public const string ThirteenSmall = "13S";
        /// <summary>
        /// 13M - FoodAndBeverageMedium
        /// </summary>
        public const string ThirteenMedium = "13M";
        /// <summary>
        /// 13B = FoodAndBeverageBig
        /// </summary>
        public const string ThirteenBig = "13B";
        // <summary>
        /// 14S = Playground_Entry_Small
        /// </summary>
        public const string FourteenSmall = "14S";
        /// <summary>
        /// 14M - PlaygroundEntryMedium
        /// </summary>
        public const string FourteenMedium = "14M";
        /// <summary>
        /// 14B = Playground_Entry_Big
        /// </summary>
        public const string FourteenBig = "14B";
        /// <summary>
        /// GetOptionCode
        /// </summary>
        /// <param name="optionCode"></param>
        /// <returns></returns>
        public static string GetOptionCode(string optionCode)
        {
            string retVal = string.Empty;
            switch (optionCode)
            {
                case OneSmall:
                    retVal = OneSmall;
                    break;
                case OneMedium:
                    retVal = OneMedium;
                    break;
                case OneBig:
                    retVal = OneBig;
                    break;
                case TwoSmall:
                    retVal = TwoSmall;
                    break;
                case TwoMedium:
                    retVal = TwoMedium;
                    break;
                case TwoBig:
                    retVal = TwoBig;
                    break;
                case ThreeSmall:
                    retVal = ThreeSmall;
                    break;
                case ThreeMedium:
                    retVal = ThreeMedium;
                    break;
                case ThreeBig:
                    retVal = ThreeBig;
                    break;
                case FourSmall:
                    retVal = FourSmall;
                    break;
                case FourMedium:
                    retVal = FourMedium;
                    break;
                case FourBig:
                    retVal = FourBig;
                    break;
                case FiveSmall:
                    retVal = FiveSmall;
                    break;
                case FiveMedium:
                    retVal = FiveMedium;
                    break;
                case FiveBig:
                    retVal = FiveBig;
                    break;
                case SixSmall:
                    retVal = SixSmall;
                    break;
                case SixMedium:
                    retVal = SixMedium;
                    break;
                case SixBig:
                    retVal = SixBig;
                    break;
                case SevenSmall:
                    retVal = SevenSmall;
                    break;
                case SevenMedium:
                    retVal = SevenMedium;
                    break;
                case SevenBig:
                    retVal = SevenBig;
                    break;
                case EightSmall:
                    retVal = EightSmall;
                    break;
                case EightMedium:
                    retVal = EightMedium;
                    break;
                case EightBig:
                    retVal = EightBig;
                    break;
                case NineSmall:
                    retVal = NineSmall;
                    break;
                case NineMedium:
                    retVal = NineMedium;
                    break;
                case NineBig:
                    retVal = NineBig;
                    break;
                case TenSmall:
                    retVal = TenSmall;
                    break;
                case TenMedium:
                    retVal = TenMedium;
                    break;
                case TenBig:
                    retVal = TenBig;
                    break;
                case ElevenSmall:
                    retVal = ElevenSmall;
                    break;
                case ElevenMedium:
                    retVal = ElevenMedium;
                    break;
                case ElevenBig:
                    retVal = ElevenBig;
                    break;
                case TwelveSmall:
                    retVal = TwelveSmall;
                    break;
                case TwelveMedium:
                    retVal = TwelveMedium;
                    break;
                case TwelveBig:
                    retVal = TwelveBig;
                    break;
                case ThirteenSmall:
                    retVal = ThirteenSmall;
                    break;
                case ThirteenMedium:
                    retVal = ThirteenMedium;
                    break;
                case ThirteenBig:
                    retVal = ThirteenBig;
                    break;
                case FourteenSmall:
                    retVal = FourteenSmall;
                    break;
                case FourteenMedium:
                    retVal = FourteenMedium;
                    break;
                case FourteenBig:
                    retVal = FourteenBig;
                    break;
                default:
                    retVal = string.Empty;
                    break;
            }
            return retVal;
        }
         
    } 
}
