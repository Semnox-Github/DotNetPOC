/********************************************************************************************
 * Project Name -  Semnox.Parafait.KioskCore
 * Description  - HomeScreenOptionFour
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
    /// HomeScreenOptionFour
    /// </summary>
    public class HomeScreenOptionFour: HomeScreenOption
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        //protected const string TRANSFER_POINT = "Transfer_Point";
        //protected const string TRANSFER_POINT_SMALL = "Transfer_Point_Small";
        protected const string TRANSFER_POINT_BUTTON = "btnTransfer";
        /// <summary>
        /// HomeScreenOptionFour
        /// </summary> 
        public HomeScreenOptionFour(ExecutionContext executionContext, string optionCode, int sortOrder) :base(executionContext)
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
                case HomeScreenOptionValues.FourSmall:
                case HomeScreenOptionValues.FourMedium:
                case HomeScreenOptionValues.FourBig:
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
            bool retVal = KioskStatic.EnableTransfer;
            log.LogMethodExit(retVal);
            return retVal;
        }        
        protected override string GetOptionImageName(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            string retVal = string.Empty;
            switch (optionCode)
            { 
                case HomeScreenOptionValues.FourSmall:
                    retVal = HomeScreenOptionImageList.TRANSFER_POINT_SMALL;
                    break;
                case HomeScreenOptionValues.FourMedium:
                    retVal = HomeScreenOptionImageList.TRANSFER_POINT_MEDIUM;
                    break;
                case HomeScreenOptionValues.FourBig:
                    retVal = HomeScreenOptionImageList.TRANSFER_POINT;
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
            string retVal = TRANSFER_POINT_BUTTON;
            log.LogMethodExit(retVal);
            return retVal;
        }
        protected override string GetOptionFontSize(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            string retVal = string.Empty;
            switch (optionCode)
            {
                case HomeScreenOptionValues.FourSmall:
                    retVal = SMALL_FONT;
                    break;
                case HomeScreenOptionValues.FourMedium:
                    retVal = MEDIUM_FONT;
                    break;
                case HomeScreenOptionValues.FourBig:
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
