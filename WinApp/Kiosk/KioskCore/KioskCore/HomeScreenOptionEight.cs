/********************************************************************************************
 * Project Name -  Semnox.Parafait.KioskCore
 * Description  - HomeScreenOptionEight
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
    /// HomeScreenOptionEight
    /// </summary>
    public class HomeScreenOptionEight : HomeScreenOption
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //protected const string SIGN_WAIVER_BUTTON_BIG = "Sign_Waiver_Button_Big";
        //protected const string SIGN_WAIVER_BUTTON_SMALL = "Sign_Waiver_Button_Small";          
        protected const string SIGN_WAIVER_BUTTON = "btnSignWaiver";
        /// <summary>
        /// HomeScreenOptionEight
        /// </summary> 
        public HomeScreenOptionEight(ExecutionContext executionContext, string optionCode, int sortOrder) : base(executionContext)
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
                case HomeScreenOptionValues.EightSmall:
                case HomeScreenOptionValues.EightMedium:
                case HomeScreenOptionValues.EightBig:
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
            bool retVal = false;
            retVal = KioskStatic.EnableWaiverSignInKiosk;
            log.LogMethodExit(retVal);
            return retVal;
        }
        protected override string GetOptionImageName(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            string retVal = string.Empty;
            switch (optionCode)
            {
                case HomeScreenOptionValues.EightSmall:
                    retVal = HomeScreenOptionImageList.SIGN_WAIVER_BUTTON_SMALL;
                    break;
                case HomeScreenOptionValues.EightMedium:
                    retVal = HomeScreenOptionImageList.SIGN_WAIVER_BUTTON_MEDIUM;
                    break;
                case HomeScreenOptionValues.EightBig:
                    retVal = HomeScreenOptionImageList.SIGN_WAIVER_BUTTON_BIG;
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
            string retVal = SIGN_WAIVER_BUTTON; 
            log.LogMethodExit(retVal);
            return retVal;
        }
        protected override string GetOptionFontSize(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            string retVal = string.Empty;
            switch (optionCode)
            {
                case HomeScreenOptionValues.EightSmall:
                    retVal = SMALL_FONT;
                    break;
                case HomeScreenOptionValues.EightMedium:
                    retVal = MEDIUM_FONT;
                    break;
                case HomeScreenOptionValues.EightBig:
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
