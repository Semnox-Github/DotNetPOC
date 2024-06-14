/********************************************************************************************
 * Project Name - Utilities
 * Description  - SystemOptionContainer class to get the List of lookup from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 2.140.0      23-Aug-2021      Prashanth V               Modified : Modified SystemOptionContainer constructor to allow Parafait Keys to be added to systemOptionContainerDTOList (POS MasterCard UI Re-Design )
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// Class holds the parafait default values.
    /// </summary>
    public class SystemOptionContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Dictionary<string, Dictionary<string, string>> systemOptionDictionary = new Dictionary<string, Dictionary<string, string>>();
        private readonly List<SystemOptionsDTO> systemOptionsDTOList;
        private readonly SystemOptionContainerDTOCollection systemOptionContainerDTOCollection;
        private readonly DateTime? systemOptionModuleLastUpdateTime;
        private readonly int siteId;

        public SystemOptionContainer(int siteId) : this(siteId, GetSystemOptionsDTOList(siteId), GetSystemOptionModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        public SystemOptionContainer(int siteId, List<SystemOptionsDTO> systemOptionsDTOList, DateTime? systemOptionModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            this.systemOptionsDTOList = systemOptionsDTOList;
            this.systemOptionModuleLastUpdateTime = systemOptionModuleLastUpdateTime;
            List<SystemOptionContainerDTO> systemOptionContainerDTOList = new List<SystemOptionContainerDTO>();
            foreach (SystemOptionsDTO systemOptionsDTO in systemOptionsDTOList)
            {
                AddToSystemOptionDictionary(systemOptionsDTO);
                if (systemOptionsDTO.OptionType == "Parafait Keys")
                {
                    systemOptionsDTO.OptionValue = GetDecryptedOptionValue(systemOptionsDTO);
                }
                SystemOptionContainerDTO systemOptionContainerDTO = new SystemOptionContainerDTO(systemOptionsDTO.OptionType, systemOptionsDTO.OptionName, systemOptionsDTO.OptionValue);
                systemOptionContainerDTOList.Add(systemOptionContainerDTO);
            }
            systemOptionContainerDTOCollection = new SystemOptionContainerDTOCollection(systemOptionContainerDTOList);
            log.LogMethodExit();
        }

        private static DateTime? GetSystemOptionModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                SystemOptionsList systemOptionsList = new SystemOptionsList();
                result = systemOptionsList.GetSystemOptionModuleLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the system option max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        private static List<SystemOptionsDTO> GetSystemOptionsDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<SystemOptionsDTO> systemOptionsDTOList = null;
            try
            {
                SystemOptionsList systemOptionsList = new SystemOptionsList();
                List<KeyValuePair<SystemOptionsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<SystemOptionsDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<SystemOptionsDTO.SearchByParameters, string>(SystemOptionsDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                systemOptionsDTOList = systemOptionsList.GetSystemOptionsDTOList(searchParameters);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the system options.", ex);
            }

            if (systemOptionsDTOList == null)
            {
                systemOptionsDTOList = new List<SystemOptionsDTO>();
            }
            log.LogMethodExit(systemOptionsDTOList);
            return systemOptionsDTOList;
        }

        private void AddToSystemOptionDictionary(SystemOptionsDTO systemOptionsDTO)
        {
            log.LogMethodEntry(systemOptionsDTO);
            if (systemOptionDictionary.ContainsKey(systemOptionsDTO.OptionType) == false)
            {
                systemOptionDictionary.Add(systemOptionsDTO.OptionType, new Dictionary<string, string>());
            }
            if (systemOptionDictionary[systemOptionsDTO.OptionType].ContainsKey(systemOptionsDTO.OptionName))
            {
                log.LogMethodExit(null, "duplicate system option " + systemOptionsDTO.OptionName);
                return;
            }
            string optionValue = systemOptionsDTO.OptionValue;
            if (systemOptionsDTO.OptionType == "Parafait Keys")
            {
                optionValue = GetDecryptedOptionValue(systemOptionsDTO);
            }
            systemOptionDictionary[systemOptionsDTO.OptionType].Add(systemOptionsDTO.OptionName, optionValue);
            log.LogMethodExit();
        }

        private static string GetDecryptedOptionValue(SystemOptionsDTO systemOptionsDTO)
        {
            log.LogMethodEntry(systemOptionsDTO);
            string optionValue = string.Empty;
            try
            {
                optionValue = Encryption.Decrypt(systemOptionsDTO.OptionValue);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while decrypting system option " + systemOptionsDTO.OptionName, ex);
            }
            log.LogMethodExit("optionValue");
            return optionValue;
        }

        public SystemOptionContainerDTOCollection GetSystemOptionContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(systemOptionContainerDTOCollection);
            return systemOptionContainerDTOCollection;
        }

        public string GetSystemOption(string optionType, string optionName)
        {
            log.LogMethodEntry(optionType, optionName);
            string result = string.Empty;
            if (systemOptionDictionary.ContainsKey(optionType) == false)
            {
                string errorMessage = "optionType : " + optionType + " not found";
                log.Error(errorMessage);
                log.LogMethodExit(result, errorMessage);
                return result;
            }
            if (systemOptionDictionary[optionType].ContainsKey(optionName) == false)
            {
                string errorMessage = "optionType : " + optionType + ". optionName : " + optionName + " not found";
                log.Error(errorMessage);
                log.LogMethodExit(result, errorMessage);
                return result;
            }
            result = systemOptionDictionary[optionType][optionName];
            log.LogMethodExit("result");
            return result;
        }

        public SystemOptionContainer Refresh()
        {
            log.LogMethodEntry();
            SystemOptionsList systemOptionsList = new SystemOptionsList();
            DateTime? updateTime = systemOptionsList.GetSystemOptionModuleLastUpdateTime(siteId);
            if (systemOptionModuleLastUpdateTime.HasValue
                && systemOptionModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in system option since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            SystemOptionContainer result = new SystemOptionContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
