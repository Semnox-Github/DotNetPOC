/********************************************************************************************
 * Project Name - Utilities
 * Description  - ObjectTranslationContainer class to get the List of lookup from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      04-Aug-2021      Prajwal S                 Created
 ********************************************************************************************/

using Semnox.Core.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Class holds the parafait default values.
    /// </summary>
    public class ObjectTranslationContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ConcurrentDictionary<Key, ObjectTranslationContainerDTO> elementKeyObjectTranslationContainerDTODictionary = new ConcurrentDictionary<Key, ObjectTranslationContainerDTO>();
        private readonly List<ObjectTranslationContainerDTO> objectTranslationContainerDTOList = new List<ObjectTranslationContainerDTO>();
        private readonly Cache<string, ObjectTranslationContainerDTOCollection> objectTranslationContainerDTOCollectionCache = new Cache<string, ObjectTranslationContainerDTOCollection>();
        private readonly DateTime? objectTranslationModuleLastUpdateTime;
        private readonly int siteId;

        public ObjectTranslationContainer(int siteId) 
            : this(siteId, GetObjectTranslationsDTOList(siteId), GetObjectTranslationModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        public ObjectTranslationContainer(int siteId, List<ObjectTranslationsDTO> objectTranslationDTOList, DateTime? objectTranslationModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            this.objectTranslationModuleLastUpdateTime = objectTranslationModuleLastUpdateTime;
            foreach (ObjectTranslationsDTO objectTranslationDTO in objectTranslationDTOList)
            {
                ObjectTranslationContainerDTO objectTranslationContainerDTO = new ObjectTranslationContainerDTO(objectTranslationDTO.Id, objectTranslationDTO.LanguageId, objectTranslationDTO.TableObject,  objectTranslationDTO.ElementGuid, objectTranslationDTO.Element, objectTranslationDTO.Translation);
                objectTranslationContainerDTOList.Add(objectTranslationContainerDTO);
                Key key = new Key(objectTranslationContainerDTO.LanguageId, objectTranslationContainerDTO.TableObject, objectTranslationContainerDTO.Element, objectTranslationContainerDTO.ElementGuid);
                if (elementKeyObjectTranslationContainerDTODictionary.ContainsKey(key))
                {
                    continue;
                }
                elementKeyObjectTranslationContainerDTODictionary[key] = objectTranslationContainerDTO;
            }
            log.LogMethodExit();
        }

        private string GetKey(string element, string elementGuid)
        {
            return "Element:" + element + "ElementGuid:" + elementGuid;
        }

        private static DateTime? GetObjectTranslationModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                ObjectTranslationsList objectTranslationList = new ObjectTranslationsList();
                result = objectTranslationList.GetObjectTranslationsModuleLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the system option max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        private static List<ObjectTranslationsDTO> GetObjectTranslationsDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<ObjectTranslationsDTO> objectTranslationDTOList = null;
            try
            {
                ObjectTranslationsList objectTranslationList = new ObjectTranslationsList();
                List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>> searchParameters = new List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>>();
                searchParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.SITE_ID, siteId.ToString()));
                searchParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.IS_ACTIVE, "1"));
                objectTranslationDTOList = objectTranslationList.GetAllObjectTranslations(searchParameters);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the object translations.", ex);
            }

            if (objectTranslationDTOList == null)
            {
                objectTranslationDTOList = new List<ObjectTranslationsDTO>();
            }
            log.LogMethodExit(objectTranslationDTOList);
            return objectTranslationDTOList;
        }

        public string GetObjectTranslation(int languageId, string tableObject, string element, string elementGuid, string defaultValue)
        {
            log.LogMethodEntry(languageId, tableObject, element, elementGuid, defaultValue);
            string result = defaultValue;
            Key key = new Key(languageId, tableObject, element, elementGuid);
            if (elementKeyObjectTranslationContainerDTODictionary.ContainsKey(key))
            {
                result = elementKeyObjectTranslationContainerDTODictionary[key].Translation;
            }
            return result;
        }

        public ObjectTranslationContainerDTOCollection GetObjectTranslationContainerDTOCollection(int languageId, string tableObject)
        {
            log.LogMethodEntry();
            string key = "languageId:" + languageId + "tableObject:" + tableObject;
            ObjectTranslationContainerDTOCollection result = objectTranslationContainerDTOCollectionCache.GetOrAdd(key, (k)=> CreateObjectTranslationContainerDTOCollection(languageId, tableObject));
            log.LogMethodExit(result);
            return result;
        }

        private ObjectTranslationContainerDTOCollection CreateObjectTranslationContainerDTOCollection(int languageId, string tableObject)
        {
            log.LogMethodEntry(languageId, tableObject);
            List<ObjectTranslationContainerDTO> list =  new List<ObjectTranslationContainerDTO>();
            foreach (var objectTranslationContainerDTO in objectTranslationContainerDTOList)
            {
                if(objectTranslationContainerDTO.LanguageId == languageId && objectTranslationContainerDTO.TableObject == tableObject)
                {
                    list.Add(objectTranslationContainerDTO);
                }
            }
            ObjectTranslationContainerDTOCollection result = new ObjectTranslationContainerDTOCollection(list);
            log.LogMethodExit(result);
            return result;
        }

        public ObjectTranslationContainer Refresh()
        {
            log.LogMethodEntry();
            ObjectTranslationsList objectTranslationList = new ObjectTranslationsList();
            DateTime? updateTime = objectTranslationList.GetObjectTranslationsModuleLastUpdateTime(siteId);
            if (objectTranslationModuleLastUpdateTime.HasValue
                && objectTranslationModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in system option since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            ObjectTranslationContainer result = new ObjectTranslationContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }

        class Key : ValueObject
        {
            private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            private readonly int languageId;
            private readonly string tableObject;
            private readonly string element;
            private readonly string elementGuid;
            public Key(int languageId, string tableObject, string element, string elementGuid)
            {
                log.LogMethodEntry(languageId, tableObject, element, elementGuid);
                this.languageId = languageId;
                this.tableObject = tableObject;
                this.element = element;
                this.elementGuid = elementGuid;
                log.LogMethodExit();
            }
            protected override IEnumerable<object> GetAtomicValues()
            {
                yield return languageId;
                yield return tableObject;
                yield return element;
                yield return elementGuid;
            }

            public override string ToString()
            {
                return "languageId:" + languageId + "tableObject:" + tableObject + "element:" + element + "elementGuid:" + elementGuid; 
            }
        }
    }
}
