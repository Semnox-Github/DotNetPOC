/********************************************************************************************
 * Project Name -  Semnox.Parafait.KioskCore
 * Description  - HomeScreenOptionEleven
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
    /// HomeScreenOptionEleven
    /// </summary>
    public class HomeScreenOptionEleven : HomeScreenOption
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //protected const string RECHARGE_PLAY_PASS_BIG = "Recharge_Play_Pass_Big";
        //protected const string RECHARGE_PLAY_PASS_SMALL = "Recharge_Play_Pass_small"; 
        protected const string RECHARGE_BUTTON = "btnRecharge";
        /// <summary>
        /// HomeScreenOptionEleven
        /// </summary> 
        public HomeScreenOptionEleven(ExecutionContext executionContext, string optionCode, int sortOrder) : base(executionContext)
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
                case HomeScreenOptionValues.ElevenSmall:
                case HomeScreenOptionValues.ElevenMedium:
                case HomeScreenOptionValues.ElevenBig:
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
            retVal = (KioskStatic.DisablePurchase == false
                      && ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "DISABLE_RECHARGE", false) == false
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
                case HomeScreenOptionValues.ElevenSmall:
                    retVal = HomeScreenOptionImageList.RECHARGE_PLAY_PASS_SMALL;
                    break;
                case HomeScreenOptionValues.ElevenMedium:
                    retVal = HomeScreenOptionImageList.RECHARGE_PLAY_PASS_MEDIUM;
                    break;
                case HomeScreenOptionValues.ElevenBig:
                    retVal = HomeScreenOptionImageList.RECHARGE_PLAY_PASS_BIG;
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
            string retVal = RECHARGE_BUTTON;
            log.LogMethodExit(retVal);
            return retVal;
        }
        protected override string GetOptionFontSize(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            string retVal = string.Empty;
            switch (optionCode)
            {
                case HomeScreenOptionValues.ElevenSmall:
                    retVal = SMALL_FONT;
                    break;
                case HomeScreenOptionValues.ElevenMedium:
                    retVal = MEDIUM_FONT;
                    break;
                case HomeScreenOptionValues.ElevenBig:
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
