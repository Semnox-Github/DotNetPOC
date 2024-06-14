/********************************************************************************************
 * Project Name - ObjectTranslation
 * Description  - ObjectTranslationList holds the ObjectTranslation container
 *
 **************
 ** Version Log
  **************
  * Version     Date            Modified By         Remarks
 *********************************************************************************************
  2.130.0       04-Aug-2021     Prajwal S          Created 
 ********************************************************************************************/
using System.Timers;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Holds the product container object
    /// </summary>
    public class ObjectTranslationContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Timer refreshTimer;
        private static readonly Cache<int, ObjectTranslationContainer> objectTranslationContainerCache = new Cache<int, ObjectTranslationContainer>();


        static ObjectTranslationContainerList()
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
            var uniqueKeyList = objectTranslationContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                ObjectTranslationContainer objectTranslationContainer;
                if (objectTranslationContainerCache.TryGetValue(uniqueKey, out objectTranslationContainer))
                {
                    objectTranslationContainerCache[uniqueKey] = objectTranslationContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static ObjectTranslationContainer GetObjectTranslationContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            ObjectTranslationContainer result = objectTranslationContainerCache.GetOrAdd(siteId, (k) => new ObjectTranslationContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static ObjectTranslationContainerDTOCollection GetObjectTranslationContainerDTOCollection(int siteId, int languageId, string tableObject)
        {
            log.LogMethodEntry(siteId);
            ObjectTranslationContainer container = GetObjectTranslationContainer(siteId);
            ObjectTranslationContainerDTOCollection result = container.GetObjectTranslationContainerDTOCollection(languageId, tableObject);
            log.LogMethodExit(result);
            return result;
        }

        public static string GetObjectTranslation(ExecutionContext executionContext, string tableObject, string element, string elementGuid, string defaultValue)
        {
            log.LogMethodEntry(executionContext, tableObject, element, elementGuid, defaultValue);
            string result = GetObjectTranslation(executionContext.SiteId, executionContext.LanguageId, tableObject, element, elementGuid, defaultValue);
            log.LogMethodExit(result);
            return result;
        }

        public static string GetObjectTranslation(int siteId, int languageId, string tableObject, string element, string elementGuid, string defaultValue)
        {
            log.LogMethodEntry(siteId, languageId, tableObject, element, elementGuid, defaultValue);
            if(languageId < 0)
            {
                log.LogMethodExit(defaultValue, "languageId < 0");
                return defaultValue;
            }
            ObjectTranslationContainer objectTranslationContainer = GetObjectTranslationContainer(siteId);
            string result = objectTranslationContainer.GetObjectTranslation(languageId, tableObject, element, elementGuid, defaultValue);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            ObjectTranslationContainer objectTranslationContainer = GetObjectTranslationContainer(siteId);
            objectTranslationContainerCache[siteId] = objectTranslationContainer.Refresh();
            log.LogMethodExit();
        }

    }
}
