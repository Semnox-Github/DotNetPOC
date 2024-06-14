/********************************************************************************************
 * Project Name -  Semnox.Parafait.KioskCore
 * Description  - HomeScreenOptionFive
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
    /// HomeScreenOptionFive
    /// </summary>
    public class HomeScreenOptionFive: HomeScreenOption
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                
        //protected const string POINTS_TO_TIME_BUTTON_BIG = "Points_To_Time_Button_Big";
        //protected const string POINTS_TO_TIME_BUTTON_SMALL = "Points_To_Time_Button_Small";
        protected const string POINTS_TO_TIME_BUTTON = "btnPointsToTime";
        /// <summary>
        /// HomeScreenOptionFive
        /// </summary> 
        public HomeScreenOptionFive(ExecutionContext executionContext, string optionCode, int sortOrder) :base(executionContext)
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
                case HomeScreenOptionValues.FiveSmall:
                case HomeScreenOptionValues.FiveMedium:
                case HomeScreenOptionValues.FiveBig:
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
            bool retVal =  KioskStatic.AllowPointsToTimeConversion; 
            log.LogMethodExit(retVal);
            return retVal;
        }
        
        protected override string GetOptionImageName(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            string retVal = string.Empty;
            switch (optionCode)
            { 
                case HomeScreenOptionValues.FiveSmall:
                    retVal = HomeScreenOptionImageList.POINTS_TO_TIME_BUTTON_SMALL;
                    break;
                case HomeScreenOptionValues.FiveMedium:
                    retVal = HomeScreenOptionImageList.POINTS_TO_TIME_BUTTON_MEDIUM;
                    break;
                case HomeScreenOptionValues.FiveBig:
                    retVal = HomeScreenOptionImageList.POINTS_TO_TIME_BUTTON_BIG;
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
            string retVal = POINTS_TO_TIME_BUTTON; 
            log.LogMethodExit(retVal);
            return retVal;
        }
        protected override string GetOptionFontSize(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            string retVal = string.Empty;
            switch (optionCode)
            {
                case HomeScreenOptionValues.FiveSmall:
                    retVal = SMALL_FONT;
                    break;
                case HomeScreenOptionValues.FiveMedium:
                    retVal = MEDIUM_FONT;
                    break;
                case HomeScreenOptionValues.FiveBig:
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
