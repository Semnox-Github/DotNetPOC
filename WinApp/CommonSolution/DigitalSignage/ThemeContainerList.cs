/********************************************************************************************
 * Project Name - DigitalSignage
 * Description  - ThemeMasterList class to get the List of themes from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.DigitalSignage
{
    public class ThemeContainerList
    {
        private static readonly object locker = new object();
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<int, ThemeContainer> themeContainerDictionary = new ConcurrentDictionary<int, ThemeContainer>();
        private static Timer refreshTimer;

        static ThemeContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            refreshTimer.Start();
            log.LogMethodExit();
        }

        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            List<int> uniqueKeyList = themeContainerDictionary.Keys.ToList();
            foreach (var uniqueKey in uniqueKeyList)
            {
                ThemeContainer themeContainer;
                if (themeContainerDictionary.TryGetValue(uniqueKey, out themeContainer))
                {
                    themeContainerDictionary[uniqueKey] = themeContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }


        private static ThemeContainer GetThemeContainer(int siteId, ExecutionContext executionContext = null) //added
        {
            log.LogMethodEntry(siteId);
            if (themeContainerDictionary.ContainsKey(siteId) == false)
            {
                themeContainerDictionary[siteId] = new ThemeContainer(siteId, executionContext);
            }
            ThemeContainer result = themeContainerDictionary[siteId];
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// rebulds the container
        /// </summary>
        public static void Rebuild(int siteId) //added
        {
            log.LogMethodEntry();
            ThemeContainer themeContainer = GetThemeContainer(siteId);
            themeContainerDictionary[siteId] = themeContainer.Refresh();
            log.LogMethodExit();
        }

        internal static List<ThemeContainerDTO> GetThemeContainerDTOList(int siteId, ExecutionContext executionContext)
        {

            log.LogMethodEntry(siteId);
            ThemeContainer container = GetThemeContainer(siteId, executionContext);
            List<ThemeContainerDTO> themeContainerDTOList = container.GetThemeContainerDTOList();
            log.LogMethodExit(themeContainerDTOList);
            return themeContainerDTOList;


        }
    }
}
