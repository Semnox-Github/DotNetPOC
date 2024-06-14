/********************************************************************************************
 * Project Name -  Semnox.Parafait.KioskCore
 * Description  - HomeScreenOptionFactory
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
    public class HomeScreenOptionFactory
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// GetHomeScreenOption
        /// </summary> 
        /// <returns></returns>
        public static HomeScreenOption GetHomeScreenOption(ExecutionContext executionContext, string optionCode, int sortOrder)
        {
            log.LogMethodEntry(executionContext, optionCode, sortOrder);
            HomeScreenOption retVal = null; 
            switch (optionCode)
            {
                case HomeScreenOptionValues.OneSmall:
                case HomeScreenOptionValues.OneMedium:
                case HomeScreenOptionValues.OneBig:
                    retVal = new HomeScreenOptionOne(executionContext, optionCode, sortOrder);
                    break;
                case HomeScreenOptionValues.TwoSmall:
                case HomeScreenOptionValues.TwoMedium:
                case HomeScreenOptionValues.TwoBig:
                    retVal = new HomeScreenOptionTwo(executionContext, optionCode, sortOrder); 
                    break;
                case HomeScreenOptionValues.ThreeSmall:
                case HomeScreenOptionValues.ThreeMedium:
                case HomeScreenOptionValues.ThreeBig:
                    retVal = new HomeScreenOptionThree(executionContext, optionCode, sortOrder);
                    break;
                case HomeScreenOptionValues.FourSmall:
                case HomeScreenOptionValues.FourMedium:
                case HomeScreenOptionValues.FourBig:
                    retVal = new HomeScreenOptionFour(executionContext, optionCode, sortOrder);
                    break;
                case HomeScreenOptionValues.FiveSmall:
                case HomeScreenOptionValues.FiveMedium:
                case HomeScreenOptionValues.FiveBig:
                    retVal = new HomeScreenOptionFive(executionContext, optionCode, sortOrder);
                    break;
                case HomeScreenOptionValues.SixSmall:
                case HomeScreenOptionValues.SixMedium:
                case HomeScreenOptionValues.SixBig:
                    retVal = new HomeScreenOptionSix(executionContext, optionCode, sortOrder);
                    break;
                case HomeScreenOptionValues.SevenSmall:
                case HomeScreenOptionValues.SevenMedium:
                case HomeScreenOptionValues.SevenBig:
                    retVal = new HomeScreenOptionSeven(executionContext, optionCode, sortOrder);
                    break;
                case HomeScreenOptionValues.EightSmall:
                case HomeScreenOptionValues.EightMedium:
                case HomeScreenOptionValues.EightBig:
                    retVal = new HomeScreenOptionEight(executionContext, optionCode, sortOrder);
                    break;
                case HomeScreenOptionValues.NineSmall:
                case HomeScreenOptionValues.NineMedium:
                case HomeScreenOptionValues.NineBig:
                    retVal = new HomeScreenOptionNine(executionContext, optionCode, sortOrder);
                    break;
                case HomeScreenOptionValues.TenSmall:
                case HomeScreenOptionValues.TenMedium:
                case HomeScreenOptionValues.TenBig:
                    retVal = new HomeScreenOptionTen(executionContext, optionCode, sortOrder);
                    break;
                case HomeScreenOptionValues.ElevenSmall:
                case HomeScreenOptionValues.ElevenMedium:
                case HomeScreenOptionValues.ElevenBig:
                    retVal = new HomeScreenOptionEleven(executionContext, optionCode, sortOrder);
                    break;
                case HomeScreenOptionValues.TwelveSmall:
                case HomeScreenOptionValues.TwelveMedium:
                case HomeScreenOptionValues.TwelveBig:
                    retVal = new HomeScreenOptionTwelve(executionContext, optionCode, sortOrder);
                    break;
                case HomeScreenOptionValues.ThirteenSmall:
                case HomeScreenOptionValues.ThirteenMedium:
                case HomeScreenOptionValues.ThirteenBig:
                    retVal = new HomeScreenOptionThirteen(executionContext, optionCode, sortOrder);
                    break;
                case HomeScreenOptionValues.FourteenSmall:
                case HomeScreenOptionValues.FourteenMedium:
                case HomeScreenOptionValues.FourteenBig:
                    retVal = new HomeScreenOptionFourteen(executionContext, optionCode, sortOrder);
                    break;
                default:
                    retVal = null;
                    break;
            }
            if (retVal == null)
            {
                string msg = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Menu Option"));
                //Please enter valid value for &1
                throw new ValidationException(msg);
            }
            log.LogMethodExit();
            return retVal;
        }
    }
}
