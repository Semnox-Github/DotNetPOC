/********************************************************************************************
 * Project Name - Products
 * Description  - ModifierSetMasterList class to get the List of games from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
*2.140.00   14-Sep-2021        Prajwal S          Created : Web Inventory UI Redesign with REST API
 ********************************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Product
{
    public static class ModifierSetContainerList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<int, ModifierSetContainer> modifierSetContainerDictionary = new ConcurrentDictionary<int, ModifierSetContainer>();
        private static Timer refreshTimer;

        static ModifierSetContainerList()
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
            List<int> uniqueKeyList = modifierSetContainerDictionary.Keys.ToList();
            foreach (var uniqueKey in uniqueKeyList)
            {
                ModifierSetContainer ModifierSetContainer;
                if (modifierSetContainerDictionary.TryGetValue(uniqueKey, out ModifierSetContainer))
                {
                    modifierSetContainerDictionary[uniqueKey] = ModifierSetContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }
        internal static List<ModifierSetContainerDTO> GetModifierSetContainerDTOList(int siteId)
        {

            log.LogMethodEntry(siteId);
            ModifierSetContainer container = GetModifierSetContainer(siteId);
            List<ModifierSetContainerDTO> modifierSetContainerDTOList = container.GetModifierSetContainerDTOList();
            log.LogMethodExit(modifierSetContainerDTOList);
            return modifierSetContainerDTOList;


        }

        private static ModifierSetContainer GetModifierSetContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            if (modifierSetContainerDictionary.ContainsKey(siteId) == false)
            {
                modifierSetContainerDictionary[siteId] = new ModifierSetContainer(siteId);
            }
            ModifierSetContainer result = modifierSetContainerDictionary[siteId];
            log.LogMethodExit(result);
            return result;
        }

        public static ModifierSetContainerDTOCollection GetModifierSetContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            ModifierSetContainer container = GetModifierSetContainer(siteId);
            ModifierSetContainerDTOCollection result = container.GetModifierSetContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            ModifierSetContainer modifierSetContainer = GetModifierSetContainer(siteId);
            modifierSetContainerDictionary[siteId] = modifierSetContainer.Refresh();
            log.LogMethodExit();
        }

    }
}
