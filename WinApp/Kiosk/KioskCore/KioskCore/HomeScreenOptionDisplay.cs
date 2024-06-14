/********************************************************************************************
 * Project Name -  Semnox.Parafait.KioskCore
 * Description  - HomeScreenOptionDisplay
 * 
 **************
 **Version Log
 **************
 *Version      Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.150.6.0    26-Oct-2023   Guru S A             Created for Dynamic home screen options
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Semnox.Parafait.KioskCore
{
    /// <summary>
    /// HomeScreenOptionDisplay
    /// </summary>
    public class HomeScreenOptionDisplay
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<HomeScreenOption> optionList = new List<HomeScreenOption>();
        private ExecutionContext executionContext;
        private string[] optionArray;
        /// <summary>
        /// OptionList
        /// </summary>
        public List<HomeScreenOption> GetOptionList
        {
            get
            {
                return (optionList != null && optionList.Any()
                                           ? optionList.OrderBy(op => op.GetSortOrder()).ToList()
                                           : optionList);
            }
        }
        /// <summary>
        /// HomeScreenOptionDisplay
        /// </summary>
        /// <param name="executionContext"></param>
        public HomeScreenOptionDisplay(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            int sortOrderValue = 1;
            string dynamicImageConfig = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "KIOSK_HOME_SCREEN_MENU_IMAGES");
            if (string.IsNullOrWhiteSpace(dynamicImageConfig) == false && HasValidDynamicImageConfig(executionContext))
            {
                dynamicImageConfig = FormatInputString(dynamicImageConfig);
                string[] dynamicImageConfigList = dynamicImageConfig.Split('|');
                if (dynamicImageConfigList.Length > 0)
                {
                    this.optionArray = dynamicImageConfigList;
                    foreach (string item in optionArray)
                    {
                        try
                        {
                            HomeScreenOption homeScreenOption = HomeScreenOptionFactory.GetHomeScreenOption(executionContext, item, sortOrderValue);
                            optionList.Add(homeScreenOption);
                            sortOrderValue++;
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            KioskStatic.logToFile("Error while GetHomeScreenOption for " + item);
                        }
                    }
                    AddMissingOptions(sortOrderValue);
                }
            }

            log.LogMethodExit();
        }
        /// <summary>
        /// HasValidDynamicImageConfig
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static bool HasValidDynamicImageConfig(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            bool retVal = false;
            string dynamicImageConfig = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "KIOSK_HOME_SCREEN_MENU_IMAGES");
            if (string.IsNullOrWhiteSpace(dynamicImageConfig) == false)
            {
                dynamicImageConfig = FormatInputString(dynamicImageConfig);
                //retVal = Regex.IsMatch(dynamicImageConfig, @"^(?:(?:[1-9]|1[0-4])[BS]\|)*(?:[1-9]|1[0-4])[BS]$", RegexOptions.IgnoreCase);
                retVal = Regex.IsMatch(dynamicImageConfig, @"^(?:\d{1,2}[SMB]\|)*\d{1,2}[SMB]$", RegexOptions.IgnoreCase);
                if (retVal == false)
                {
                    KioskStatic.logToFile("KIOSK_HOME_SCREEN_MENU_IMAGES has invalid value " + dynamicImageConfig);
                    log.Error("KIOSK_HOME_SCREEN_MENU_IMAGES has invalid value " + dynamicImageConfig);
                }
            }
            log.LogMethodExit(retVal);
            return retVal;
        }
        private void AddMissingOptions(int sortOrderValue)
        {
            log.LogMethodEntry(sortOrderValue);
            sortOrderValue = AddOption(sortOrderValue, HomeScreenOptionValues.OneSmall, HomeScreenOptionValues.OneMedium, HomeScreenOptionValues.OneBig);
            sortOrderValue = AddOption(sortOrderValue, HomeScreenOptionValues.TwoSmall, HomeScreenOptionValues.TwoMedium, HomeScreenOptionValues.TwoBig);
            sortOrderValue = AddOption(sortOrderValue, HomeScreenOptionValues.ThreeSmall, HomeScreenOptionValues.ThreeMedium, HomeScreenOptionValues.ThreeBig);
            sortOrderValue = AddOption(sortOrderValue, HomeScreenOptionValues.FourSmall, HomeScreenOptionValues.FourMedium, HomeScreenOptionValues.FourBig);
            sortOrderValue = AddOption(sortOrderValue, HomeScreenOptionValues.FiveSmall, HomeScreenOptionValues.FiveMedium, HomeScreenOptionValues.FiveBig);
            sortOrderValue = AddOption(sortOrderValue, HomeScreenOptionValues.SixSmall, HomeScreenOptionValues.SixMedium, HomeScreenOptionValues.SixBig);
            sortOrderValue = AddOption(sortOrderValue, HomeScreenOptionValues.SevenSmall, HomeScreenOptionValues.SevenMedium, HomeScreenOptionValues.SevenBig);
            sortOrderValue = AddOption(sortOrderValue, HomeScreenOptionValues.EightSmall, HomeScreenOptionValues.EightMedium, HomeScreenOptionValues.EightBig);
            sortOrderValue = AddOption(sortOrderValue, HomeScreenOptionValues.NineSmall, HomeScreenOptionValues.NineMedium, HomeScreenOptionValues.NineBig);
            sortOrderValue = AddOption(sortOrderValue, HomeScreenOptionValues.TenSmall, HomeScreenOptionValues.TenMedium, HomeScreenOptionValues.TenBig);
            sortOrderValue = AddOption(sortOrderValue, HomeScreenOptionValues.ElevenSmall, HomeScreenOptionValues.ElevenMedium, HomeScreenOptionValues.ElevenBig);
            sortOrderValue = AddOption(sortOrderValue, HomeScreenOptionValues.TwelveSmall, HomeScreenOptionValues.TwelveMedium, HomeScreenOptionValues.TwelveBig);
            sortOrderValue = AddOption(sortOrderValue, HomeScreenOptionValues.ThirteenSmall, HomeScreenOptionValues.ThirteenMedium, HomeScreenOptionValues.ThirteenBig);
            sortOrderValue = AddOption(sortOrderValue, HomeScreenOptionValues.FourteenSmall, HomeScreenOptionValues.FourteenMedium, HomeScreenOptionValues.FourteenBig);
            log.LogMethodExit();
        }

        private int AddOption(int sortOrderValue, string smallOpt, string mediumOpt, string bigOpt)
        {
            log.LogMethodEntry(sortOrderValue, smallOpt, mediumOpt, bigOpt);
            if (optionList.Exists(opt => opt.GetOptionCode() == smallOpt || opt.GetOptionCode() == mediumOpt || opt.GetOptionCode() == bigOpt) == false)
            {
                try
                {
                    HomeScreenOption homeScreenOption = HomeScreenOptionFactory.GetHomeScreenOption(executionContext, smallOpt, sortOrderValue);
                    if (homeScreenOption.CanShowTheOption())
                    {
                        optionList.Add(homeScreenOption);
                        sortOrderValue++;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    KioskStatic.logToFile("Error in AddOption for " + smallOpt);
                }
            }
            log.LogMethodExit(sortOrderValue);
            return sortOrderValue;
        }
        private static string FormatInputString(string configData)
        {
            log.LogMethodEntry(configData);
            string resultData = (string.IsNullOrWhiteSpace(configData) == false ? configData.Trim().Replace(" ", "").ToUpper() : string.Empty);
            log.LogMethodExit(resultData);
            return resultData;
        }
    }
}
