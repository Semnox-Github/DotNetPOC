/********************************************************************************************
 * Project Name -  Semnox.Parafait.KioskCore
 * Description  - HomeScreenOptionSix
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
    /// HomeScreenOptionSix
    /// </summary>
    public class HomeScreenOptionSix: HomeScreenOption
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        //protected const string PAUSE_CARD_BUTTON_BIG = "Pause_Card_Button_Big";
        //protected const string PAUSE_CARD_BUTTON_SMALL = "Pause_Card_Button_Small";
        protected const string PAUSE_CARD_BUTTON = "btnPauseTime";
        /// <summary>
        /// HomeScreenOptionSix
        /// </summary> 
        public HomeScreenOptionSix(ExecutionContext executionContext, string optionCode, int sortOrder) :base(executionContext)
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
                case HomeScreenOptionValues.SixSmall:
                case HomeScreenOptionValues.SixMedium:
                case HomeScreenOptionValues.SixBig:
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
            bool retVal = KioskStatic.EnablePauseCard;
            log.LogMethodExit(retVal);
            return retVal;
        }
        
        protected override string GetOptionImageName(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            string retVal = string.Empty;
            switch (optionCode)
            {
                case HomeScreenOptionValues.SixSmall:
                    retVal = HomeScreenOptionImageList.PAUSE_CARD_BUTTON_SMALL;
                    break;
                case HomeScreenOptionValues.SixMedium:
                    retVal = HomeScreenOptionImageList.PAUSE_CARD_BUTTON_MEDIUM;
                    break;
                case HomeScreenOptionValues.SixBig:
                    retVal = HomeScreenOptionImageList.PAUSE_CARD_BUTTON_BIG;
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
            string retVal = PAUSE_CARD_BUTTON;
            log.LogMethodExit(retVal);
            return retVal;
        }
        protected override string GetOptionFontSize(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            string retVal = string.Empty;
            switch (optionCode)
            {
                case HomeScreenOptionValues.SixSmall:
                    retVal = SMALL_FONT;
                    break;
                case HomeScreenOptionValues.SixMedium:
                    retVal = MEDIUM_FONT;
                    break;
                case HomeScreenOptionValues.SixBig:
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
