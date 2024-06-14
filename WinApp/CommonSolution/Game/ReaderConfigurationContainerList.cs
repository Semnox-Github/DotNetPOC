/********************************************************************************************
 * Project Name - Games
 * Description  - ReaderConfigurationMasterList class to get the List of games from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API.
 2.110.0     14-Dec-2020       Prajwal S                 Updated for new container API changes.
 ********************************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Game
{
    public static  class ReaderConfigurationContainerList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<int, ReaderConfigurationContainer> readerConfigurationContainerDictionary = new ConcurrentDictionary<int, ReaderConfigurationContainer>();
        private static Timer refreshTimer;
        private static readonly object locker = new object();

        static ReaderConfigurationContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            refreshTimer.Start();
            log.LogMethodExit();
        }


        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)  //added
        {
            log.LogMethodEntry();
            List<int> uniqueKeyList = readerConfigurationContainerDictionary.Keys.ToList();
            foreach (var uniqueKey in uniqueKeyList)
            {
                ReaderConfigurationContainer ReaderConfigurationContainer;
                if (readerConfigurationContainerDictionary.TryGetValue(uniqueKey, out ReaderConfigurationContainer))
                {
                    readerConfigurationContainerDictionary[uniqueKey] = ReaderConfigurationContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }


        private static ReaderConfigurationContainer GetReaderConfigurationContainer(int siteId) //added
        {
            log.LogMethodEntry(siteId);
            if (readerConfigurationContainerDictionary.ContainsKey(siteId) == false)
            {
                readerConfigurationContainerDictionary[siteId] = new ReaderConfigurationContainer(siteId);
            }
            ReaderConfigurationContainer result = readerConfigurationContainerDictionary[siteId];
            log.LogMethodExit(result);
            return result;
        }



        /// <summary>
        /// rebulds the container
        /// </summary>
        public static void Rebuild(int siteId) //added
        {
            log.LogMethodEntry();
            ReaderConfigurationContainer readerConfigurationContainer = GetReaderConfigurationContainer(siteId);
            readerConfigurationContainerDictionary[siteId] = readerConfigurationContainer.Refresh();
            log.LogMethodExit();
        }

        internal static List<ReaderConfigurationContainerDTO> GetReaderConfigurationContainerDTOList(int siteId)
        {

            log.LogMethodEntry(siteId);
            ReaderConfigurationContainer container = GetReaderConfigurationContainer(siteId);
            List<ReaderConfigurationContainerDTO> readerConfigurationContainerDTOList = container.GetReaderConfigurationContainerDTOList();
            log.LogMethodExit(readerConfigurationContainerDTOList);
            return readerConfigurationContainerDTOList;


        }
    }
}
