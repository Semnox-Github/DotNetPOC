/********************************************************************************************
 * Project Name -  Semnox.Parafait.KioskCore
 * Description  - HomeScreenOptionSeven
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
    /// HomeScreenOptionSeven
    /// </summary>
    public class HomeScreenOptionSeven: HomeScreenOption
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        //protected const string EXECUTE_ONLINE_TRX_BUTTON = "Execute_Online_Trx_Button";
        //protected const string EXECUTE_ONLINE_TRX_BUTTON_SMALL = "Execute_Online_Trx_Button_Small";
        protected const string EXECUTE_ONLINE_TRAN_BUTTON = "btnExecuteOnlineTransaction";
        /// <summary>
        /// HomeScreenOptionSeven
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="optionCode"></param>
        /// <param name="sortOrder"></param>
        public HomeScreenOptionSeven(ExecutionContext executionContext, string optionCode, int sortOrder) :base(executionContext)
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
                case HomeScreenOptionValues.SevenSmall:
                case HomeScreenOptionValues.SevenMedium:
                case HomeScreenOptionValues.SevenBig:
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
            bool retVal =  ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "KIOSK_ENABLE_EXECUTE_TRANSACTION_OPTION"); 
            log.LogMethodExit(retVal);
            return retVal;
        }        
        protected override string GetOptionImageName(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            string retVal = string.Empty;
            switch (optionCode)
            {
                case HomeScreenOptionValues.SevenSmall:
                    retVal = HomeScreenOptionImageList.EXECUTE_ONLINE_TRX_BUTTON_SMALL;
                    break;
                case HomeScreenOptionValues.SevenMedium:
                    retVal = HomeScreenOptionImageList.EXECUTE_ONLINE_TRX_BUTTON_MEDIUM;
                    break;
                case HomeScreenOptionValues.SevenBig:
                    retVal = HomeScreenOptionImageList.EXECUTE_ONLINE_TRX_BUTTON;
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
            string retVal = EXECUTE_ONLINE_TRAN_BUTTON;
            log.LogMethodExit(retVal);
            return retVal;
        }
        protected override string GetOptionFontSize(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            string retVal = string.Empty;
            switch (optionCode)
            {
                case HomeScreenOptionValues.SevenSmall:
                    retVal = SMALL_FONT;
                    break;
                case HomeScreenOptionValues.SevenMedium:
                    retVal = MEDIUM_FONT;
                    break;
                case HomeScreenOptionValues.SevenBig:
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
