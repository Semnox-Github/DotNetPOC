/********************************************************************************************
 * Project Name -  Semnox.Parafait.KioskCore
 * Description  - HomeScreenOptionTen
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
    /// HomeScreenOptionTen
    /// </summary>
    public class HomeScreenOptionTen: HomeScreenOption
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        //protected const string NEW_PLAY_PASS_BUTTON_BIG = "New_Play_Pass_Button_big";
        //protected const string NEW_PLAY_PASS_BUTTON = "New_Play_Pass_Button";
        protected const string NEW_CARD_BUTTON = "btnNewCard";
        /// <summary>
        /// HomeScreenOptionTen
        /// </summary> 
        public HomeScreenOptionTen(ExecutionContext executionContext, string optionCode, int sortOrder) :base(executionContext)
        {
            log.LogMethodEntry(executionContext, optionCode, showTheOption, sortOrder); 
            if (IsValidOption(optionCode) == false)
            {
               string msg = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Menu Option"));
                throw new ValidationException(msg);
            }
            this.optionCode = optionCode;
            this.showTheOption = SetShowTheOption(optionCode);
            this.sortOrderPosition = sortOrder;
            log.LogMethodExit();
        }

        protected override bool IsValidOption(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            bool retVal = false;
            switch (optionCode)
            {
                case HomeScreenOptionValues.TenSmall:
                case HomeScreenOptionValues.TenMedium:
                case HomeScreenOptionValues.TenBig:
                    retVal = true;
                    break;
                default:
                    retVal = false;
                    break;
            }
            log.LogMethodExit(retVal);
            return retVal;
        }
        protected override bool SetShowTheOption(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            bool retVal = (KioskStatic.DisablePurchase == false && KioskStatic.DisableNewCard == false
                              && ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "SHOW_CART_IN_KIOSK", false) == false);
            log.LogMethodExit(retVal);
            return retVal;
        }        
        protected override string GetOptionImageName(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            string retVal = string.Empty;
            switch (optionCode)
            { 
                case HomeScreenOptionValues.TenSmall:
                    retVal = HomeScreenOptionImageList.NEW_PLAY_PASS_BUTTON;
                    break;
                case HomeScreenOptionValues.TenMedium:
                    retVal = HomeScreenOptionImageList.NEW_PLAY_PASS_BUTTON_MEDIUM;
                    break;
                case HomeScreenOptionValues.TenBig:
                    retVal = HomeScreenOptionImageList.NEW_PLAY_PASS_BUTTON_BIG;
                    break; 
                default:
                    retVal = string.Empty;
                    break;
            }
            log.LogMethodExit(retVal);
            return retVal;
        }

        protected override string GetOptionButtonName(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            string retVal = NEW_CARD_BUTTON;
            log.LogMethodExit(retVal);
            return retVal;
        }
        protected override string GetOptionFontSize(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            string retVal = string.Empty;
            switch (optionCode)
            {
                case HomeScreenOptionValues.TenSmall:
                    retVal = SMALL_FONT;
                    break;
                case HomeScreenOptionValues.TenMedium:
                    retVal = MEDIUM_FONT;
                    break;
                case HomeScreenOptionValues.TenBig:
                    retVal = LARGE_FONT;
                    break;
                default:
                    retVal = string.Empty;
                    break;
            }
            log.LogMethodExit(retVal);
            return retVal;
        }
    }
}
