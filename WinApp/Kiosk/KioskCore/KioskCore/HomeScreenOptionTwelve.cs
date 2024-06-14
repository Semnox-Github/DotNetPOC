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
    /// HomeScreenOptionTwelve
    /// </summary>
    public class HomeScreenOptionTwelve: HomeScreenOption
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //protected const string ATTRACTIONSBUTTONBIG = "AttractionsButtonBig";
        //protected const string ATTRACTIONSBUTTONSMALL = "AttractionsButtonSmall";
        protected const string ATTRACTIONS_BUTTON = "btnAttractions";
        /// <summary>
        /// HomeScreenOptionTwelve
        /// </summary> 
        public HomeScreenOptionTwelve(ExecutionContext executionContext, string optionCode, int sortOrder) :base(executionContext)
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
                case HomeScreenOptionValues.TwelveSmall:
                case HomeScreenOptionValues.TwelveMedium:
                case HomeScreenOptionValues.TwelveBig:
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
            bool retVal = (KioskStatic.DisablePurchase == false 
                              && ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_ATTRACTION_PRODUCTS_IN_KIOSK", false)
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
                case HomeScreenOptionValues.TwelveSmall:
                    retVal = HomeScreenOptionImageList.ATTRACTIONSBUTTONSMALL;
                    break;
                case HomeScreenOptionValues.TwelveMedium:
                    retVal = HomeScreenOptionImageList.ATTRACTIONSBUTTONMEDIUM;
                    break;
                case HomeScreenOptionValues.TwelveBig:
                    retVal = HomeScreenOptionImageList.ATTRACTIONSBUTTONBIG;
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
            string retVal = ATTRACTIONS_BUTTON;
            log.LogMethodExit(retVal);
            return retVal;
        }
        protected override string GetOptionFontSize(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            string retVal = string.Empty;
            switch (optionCode)
            {
                case HomeScreenOptionValues.TwelveSmall:
                    retVal = SMALL_FONT;
                    break;
                case HomeScreenOptionValues.TwelveMedium:
                    retVal = MEDIUM_FONT;
                    break;
                case HomeScreenOptionValues.TwelveBig:
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
