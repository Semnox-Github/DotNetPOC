/********************************************************************************************
 * Project Name -  Semnox.Parafait.KioskCore
 * Description  - HomeScreenOptionOne
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
    /// HomeScreenOptionOne
    /// </summary>
    public class HomeScreenOptionOne: HomeScreenOption
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        //protected const string PURCHASEBUTTONBIG = "PurchaseButtonBig";
        //protected const string PURCHASEBUTTONSMALL = "PurchaseButtonSmall";
        protected const string PURCHASE_BUTTON = "btnPurchase";
        /// <summary>
        /// HomeScreenOptionOne
        /// </summary> 
        public HomeScreenOptionOne(ExecutionContext executionContext, string optionCode, int sortOrder) :base(executionContext)
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
                case HomeScreenOptionValues.OneSmall:
                case HomeScreenOptionValues.OneMedium:
                case HomeScreenOptionValues.OneBig: 
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
            bool retVal = (KioskStatic.DisablePurchase == false);
            log.LogMethodExit(retVal);
            return retVal;
        }
        
        protected override string GetOptionImageName(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            string retVal = string.Empty;
            switch (optionCode)
            {
                case HomeScreenOptionValues.OneSmall:
                    retVal = HomeScreenOptionImageList.PURCHASEBUTTONSMALL;
                    break;
                case HomeScreenOptionValues.OneMedium:
                    retVal = HomeScreenOptionImageList.PURCHASEBUTTONMEDIUM;
                    break;
                case HomeScreenOptionValues.OneBig:
                    retVal = HomeScreenOptionImageList.PURCHASEBUTTONBIG;
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
            string retVal = PURCHASE_BUTTON;
            log.LogMethodExit(retVal);
            return retVal;
        }
        protected override string GetOptionFontSize(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            string retVal = string.Empty;
            switch (optionCode)
            {
                case HomeScreenOptionValues.OneSmall:
                    retVal = SMALL_FONT;
                    break;
                case HomeScreenOptionValues.OneMedium:
                    retVal = MEDIUM_FONT;
                    break;
                case HomeScreenOptionValues.OneBig:
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
