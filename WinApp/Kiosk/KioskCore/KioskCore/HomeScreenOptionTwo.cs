/********************************************************************************************
 * Project Name -  Semnox.Parafait.KioskCore
 * Description  - HomeScreenOptionTwo
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
    /// HomeScreenOptionTwo
    /// </summary>
    public class HomeScreenOptionTwo: HomeScreenOption
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
         
        //protected const string CHECK_BALANCE_BUTTON_BIG = "Check_Balance_Button_Big";
        //protected const string CHECK_BALANCE_BUTTON_SMALL = "Check_Balance_Button_Small"; 
        protected const string CHECK_BALANCE_BUTTON = "btnCheckBalance";
        /// <summary>
        /// HomeScreenOptionTwo
        /// </summary> 
        public HomeScreenOptionTwo(ExecutionContext executionContext, string optionCode, int sortOrder) :base(executionContext)
        {
            log.LogMethodEntry(executionContext, optionCode, showTheOption, sortOrder); 
            if (IsValidOption(optionCode) == false)
            {
                string msg = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Menu Option"));
                //Please enter valid value for &1
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
                case HomeScreenOptionValues.TwoSmall:
                case HomeScreenOptionValues.TwoMedium:
                case HomeScreenOptionValues.TwoBig:
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
            bool retVal =  true;
            log.LogMethodExit(retVal);
            return retVal;
        }        
        protected override string GetOptionImageName(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            string retVal = string.Empty;
            switch (optionCode)
            { 
                case HomeScreenOptionValues.TwoSmall:
                    retVal = HomeScreenOptionImageList.CHECK_BALANCE_BUTTON_SMALL;
                    break;
                case HomeScreenOptionValues.TwoMedium:
                    retVal = HomeScreenOptionImageList.CHECK_BALANCE_BUTTON_MEDIUM;
                    break;
                case HomeScreenOptionValues.TwoBig:
                    retVal = HomeScreenOptionImageList.CHECK_BALANCE_BUTTON_BIG;
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
            string retVal = CHECK_BALANCE_BUTTON;
            log.LogMethodExit(retVal);
            return retVal;
        }
        protected override string GetOptionFontSize(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            string retVal = string.Empty;
            switch (optionCode)
            {
                case HomeScreenOptionValues.TwoSmall:
                    retVal = SMALL_FONT;
                    break;
                case HomeScreenOptionValues.TwoMedium:
                    retVal = MEDIUM_FONT;
                    break;
                case HomeScreenOptionValues.TwoBig:
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
