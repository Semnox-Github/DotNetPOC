/********************************************************************************************
 * Project Name -  Semnox.Parafait.KioskCore
 * Description  - HomeScreenOptionNine
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
    /// HomeScreenOptionNine
    /// </summary>
    public class HomeScreenOptionNine: HomeScreenOption
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        //protected const string EXCHANGE_TOKENS_BUTTON = "Exchange_tokens_Button";
        //protected const string EXCHANGE_TOKENS_BUTTON_SMALL = "Exchange_tokens_Button_Small";
        protected const string EXCHANGE_TOKEN_BUTTON = "btnRedeemTokens";
        /// <summary>
        /// HomeScreenOptionNine
        /// </summary> 
        public HomeScreenOptionNine(ExecutionContext executionContext, string optionCode, int sortOrder) :base(executionContext)
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
                case HomeScreenOptionValues.NineSmall:
                case HomeScreenOptionValues.NineMedium:
                case HomeScreenOptionValues.NineBig:
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
            bool retVal = KioskStatic.EnableRedeemTokens;
            log.LogMethodExit(retVal);
            return retVal;
        }        
        protected override string GetOptionImageName(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            string retVal = string.Empty;
            switch (optionCode)
            {
                case HomeScreenOptionValues.NineSmall:
                    retVal = HomeScreenOptionImageList.EXCHANGE_TOKENS_BUTTON_SMALL;
                    break;
                case HomeScreenOptionValues.NineMedium:
                    retVal = HomeScreenOptionImageList.EXCHANGE_TOKENS_BUTTON_MEDIUM;
                    break;
                case HomeScreenOptionValues.NineBig:
                    retVal = HomeScreenOptionImageList.EXCHANGE_TOKENS_BUTTON;
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
            string retVal = EXCHANGE_TOKEN_BUTTON;
            log.LogMethodExit(retVal);
            return retVal;
        }
        protected override string GetOptionFontSize(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            string retVal = string.Empty;
            switch (optionCode)
            {
                case HomeScreenOptionValues.NineSmall:
                    retVal = SMALL_FONT;
                    break;
                case HomeScreenOptionValues.NineMedium:
                    retVal = MEDIUM_FONT;
                    break;
                case HomeScreenOptionValues.NineBig:
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
