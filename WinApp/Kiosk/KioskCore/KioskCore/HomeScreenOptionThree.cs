/********************************************************************************************
 * Project Name -  Semnox.Parafait.KioskCore
 * Description  - HomeScreenOptionThree
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
    /// HomeScreenOptionThree
    /// </summary>
    public class HomeScreenOptionThree: HomeScreenOption
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //protected const string REGISTER_PASS_BIG = "register_pass_big";
        //protected const string REGISTER_PASS_SMALL = "register_pass_Small";
        protected const string REGISTER_BUTTON = "btnRegister";
        /// <summary>
        /// HomeScreenOptionThree
        /// </summary> 
        public HomeScreenOptionThree(ExecutionContext executionContext, string optionCode, int sortOrder) :base(executionContext)
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
                case HomeScreenOptionValues.ThreeSmall:
                case HomeScreenOptionValues.ThreeMedium:
                case HomeScreenOptionValues.ThreeBig:
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
            bool retVal =  (KioskStatic.RegistrationAllowed == true && KioskStatic.DisableCustomerRegistration == false);
            log.LogMethodExit(retVal);
            return retVal;
        }        
        protected override string GetOptionImageName(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            string retVal = string.Empty;
            switch (optionCode)
            {
                case HomeScreenOptionValues.ThreeSmall:
                    retVal = HomeScreenOptionImageList.REGISTER_PASS_SMALL;
                    break;
                case HomeScreenOptionValues.ThreeMedium:
                    retVal = HomeScreenOptionImageList.REGISTER_PASS_MEDIUM;
                    break;
                case HomeScreenOptionValues.ThreeBig:
                    retVal = HomeScreenOptionImageList.REGISTER_PASS_BIG;
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
            string retVal = REGISTER_BUTTON;
            log.LogMethodExit(retVal);
            return retVal;
        }
        protected override string GetOptionFontSize(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            string retVal = string.Empty;
            switch (optionCode)
            {
                case HomeScreenOptionValues.ThreeSmall:
                    retVal = SMALL_FONT;
                    break;
                case HomeScreenOptionValues.ThreeMedium:
                    retVal = MEDIUM_FONT;
                    break;
                case HomeScreenOptionValues.ThreeBig:
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
