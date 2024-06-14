/********************************************************************************************
 * Project Name -  Semnox.Parafait.KioskCore
 * Description  - HomeScreenOptionThirteen
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
    /// HomeScreenOptionThirteen
    /// </summary>
    public class HomeScreenOptionThirteen: HomeScreenOption
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        //protected const string FOODANDBEVERAGEBIG = "FoodAndBeverageBig";
        //protected const string FOODANDBEVERAGESMALL = "FoodAndBeverageSmall"; 
        protected const string FOODANDBEVERAGE_BUTTON = "btnFNB";
        /// <summary>
        /// HomeScreenOptionThirteen
        /// </summary> 
        public HomeScreenOptionThirteen(ExecutionContext executionContext, string optionCode, int sortOrder) :base(executionContext)
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
                case HomeScreenOptionValues.ThirteenSmall:
                case HomeScreenOptionValues.ThirteenMedium:
                case HomeScreenOptionValues.ThirteenBig:
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
                              && ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_FNB_PRODUCTS_IN_KIOSK", false)
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
                case HomeScreenOptionValues.ThirteenSmall:
                    retVal = HomeScreenOptionImageList.FOODANDBEVERAGESMALL;
                    break;
                case HomeScreenOptionValues.ThirteenMedium:
                    retVal = HomeScreenOptionImageList.FOODANDBEVERAGEMEDIUM;
                    break;
                case HomeScreenOptionValues.ThirteenBig:
                    retVal = HomeScreenOptionImageList.FOODANDBEVERAGEBIG;
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
            string retVal = FOODANDBEVERAGE_BUTTON;
            log.LogMethodExit(retVal);
            return retVal;
        }
        protected override string GetOptionFontSize(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            string retVal = string.Empty;
            switch (optionCode)
            {
                case HomeScreenOptionValues.ThirteenSmall:
                    retVal = SMALL_FONT;
                    break;
                case HomeScreenOptionValues.ThirteenMedium:
                    retVal = MEDIUM_FONT;
                    break;
                case HomeScreenOptionValues.ThirteenBig:
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
