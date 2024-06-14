/********************************************************************************************
 * Project Name - Utilities
 * Description  - LanguageContainer class to get the List of lookup from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Semnox.Parafait.Languages
{
    /// <summary>
    /// Class holds the parafait default values.
    /// </summary>
    public class LanguageContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Dictionary<int, LanguageContainerDTO> languageContainerDTODictionary = new Dictionary<int, LanguageContainerDTO>();
        private readonly List<LanguagesDTO> languagesDTOList;
        private readonly LanguageContainerDTOCollection languageContainerDTOCollection;
        private readonly DateTime? languageModuleLastUpdateTime;
        private readonly int siteId;

        public LanguageContainer(int siteId) : this(siteId, GetLanguageDTOList(siteId), GetLanguageModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }
        public LanguageContainer(int siteId, List<LanguagesDTO> languagesDTOList, DateTime? languageModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            
            List<LanguageContainerDTO> languageContainerDTOList = new List<LanguageContainerDTO>();

            this.languageModuleLastUpdateTime = languageModuleLastUpdateTime;
            this.languagesDTOList = languagesDTOList;
            if (languagesDTOList != null && languagesDTOList.Any())
            {
                foreach (LanguagesDTO languagesDTO in languagesDTOList)
                {
                    LanguageContainerDTO languageContainerDTO = new LanguageContainerDTO(languagesDTO.LanguageId, languagesDTO.LanguageName, languagesDTO.LanguageCode, languagesDTO.CultureCode);
                    languageContainerDTOList.Add(languageContainerDTO);
                    languageContainerDTODictionary.Add(languagesDTO.LanguageId, languageContainerDTO);
                }
            }
            
            languageContainerDTOCollection = new LanguageContainerDTOCollection(languageContainerDTOList);
            log.LogMethodExit();
        }
        private static List<LanguagesDTO> GetLanguageDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<LanguagesDTO> languagesDTOList = null;
            try
            {
                LanguagesList languagesList = new LanguagesList();
                List<KeyValuePair<LanguagesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LanguagesDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<LanguagesDTO.SearchByParameters, string>(LanguagesDTO.SearchByParameters.IS_ACTIVE, "1"));
                searchParameters.Add(new KeyValuePair<LanguagesDTO.SearchByParameters, string>(LanguagesDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                languagesDTOList = languagesList.GetAllLanguagesList(searchParameters);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the system options.", ex);
            }

            if (languagesDTOList == null)
            {
                languagesDTOList = new List<LanguagesDTO>();
            }
            log.LogMethodExit(languagesDTOList);
            return languagesDTOList;
        }

        private static DateTime? GetLanguageModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                LanguagesList languageList = new LanguagesList();
                result = languageList.GetLanguageModuleLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the language max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        public LanguageContainerDTOCollection GetLanguageContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(languageContainerDTOCollection);
            return languageContainerDTOCollection;
        }

        public LanguageContainerDTO GetLanguageContainerDTO(int languageId)
        {
            log.LogMethodEntry(languageId);
            if (languageContainerDTODictionary.ContainsKey(languageId) == false)
            {
                string errorMessage = "Language with Language Id :" + languageId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            LanguageContainerDTO result = languageContainerDTODictionary[languageId];;
            log.LogMethodExit(result);
            return result;
        }

        public LanguageContainer Refresh()
        {
            log.LogMethodEntry();
            DateTime? updateTime = GetLanguageModuleLastUpdateTime(siteId);
            if (languageModuleLastUpdateTime.HasValue
                && languageModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in languages since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            LanguageContainer result = new LanguageContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
        
    }
}
