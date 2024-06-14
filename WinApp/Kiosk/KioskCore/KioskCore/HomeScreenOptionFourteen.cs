/********************************************************************************************
 * Project Name -  Semnox.Parafait.KioskCore
 * Description  - HomeScreenOptionFourteen
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
    /// HomeScreenOptionFourteen
    /// </summary>
    public class HomeScreenOptionFourteen: HomeScreenOption
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        //protected const string PLAYGROUND_ENTRY_BIG = "Playground_Entry_Big";
        //protected const string PLAYGROUND_ENTRY_SMALL = "Playground_Entry_Small";
        protected const string PLAYGROUND_ENTRY_BUTTON = "btnPlaygroundEntry";
        /// <summary>
        /// HomeScreenOptionFourteen
        /// </summary>
        public HomeScreenOptionFourteen(ExecutionContext executionContext, string optionCode, int sortOrder) :base(executionContext)
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
                case HomeScreenOptionValues.FourteenSmall:
                case HomeScreenOptionValues.FourteenMedium:
                case HomeScreenOptionValues.FourteenBig:
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
            bool retVal =  (KioskStatic.DisablePurchase == false && KioskStatic.EnablePlaygroundEntry
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
                case HomeScreenOptionValues.FourteenSmall:
                    retVal = HomeScreenOptionImageList.PLAYGROUND_ENTRY_SMALL;
                    break;
                case HomeScreenOptionValues.FourteenMedium:
                    retVal = HomeScreenOptionImageList.PLAYGROUND_ENTRY_MEDIUM;
                    break;
                case HomeScreenOptionValues.FourteenBig:
                    retVal = HomeScreenOptionImageList.PLAYGROUND_ENTRY_BIG;
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
            string retVal = PLAYGROUND_ENTRY_BUTTON;
            log.LogMethodExit(retVal);
            return retVal;
        }
        protected override string GetOptionFontSize(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            string retVal = string.Empty;
            switch (optionCode)
            {
                case HomeScreenOptionValues.FourteenSmall:
                    retVal = SMALL_FONT;
                    break;
                case HomeScreenOptionValues.FourteenMedium:
                    retVal = MEDIUM_FONT;
                    break;
                case HomeScreenOptionValues.FourteenBig:
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
