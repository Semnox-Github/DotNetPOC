
/********************************************************************************************
 * Project Name - Utilities
 * Description  - SystemOptionViewContainer holds the parafait default values for a given siteId, userId and POSMachineId
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
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// SystemOptionViewContainer holds the parafait default values for a given siteId, userId and POSMachineId
    /// </summary>
    public class SystemOptionViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SystemOptionContainerDTOCollection systemOptionContainerDTOCollection;
        private readonly Dictionary<string, Dictionary<string, string>> systemOptionDictionary = new Dictionary<string, Dictionary<string, string>>();
        private readonly int siteId;

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="systemOptionContainerDTOCollection">systemOptionContainerDTOCollection</param>
        public SystemOptionViewContainer(int siteId, SystemOptionContainerDTOCollection systemOptionContainerDTOCollection)//Changed to public
        {
            log.LogMethodEntry(siteId, systemOptionContainerDTOCollection);
            this.siteId = siteId;
            this.systemOptionContainerDTOCollection = systemOptionContainerDTOCollection;
            if (systemOptionContainerDTOCollection != null &&
                systemOptionContainerDTOCollection.SystemOptionContainerDTOList != null &&
                systemOptionContainerDTOCollection.SystemOptionContainerDTOList.Any())
            {
                foreach (var systemOptionContainerDTO in systemOptionContainerDTOCollection.SystemOptionContainerDTOList)
                {
                    AddToSystemOptionDictionary(systemOptionContainerDTO);
                }
            }
            log.LogMethodExit();
        }

        private void AddToSystemOptionDictionary(SystemOptionContainerDTO systemOptionContainerDTO)
        {
            log.LogMethodEntry(systemOptionContainerDTO);
            if (systemOptionDictionary.ContainsKey(systemOptionContainerDTO.OptionType) == false)
            {
                systemOptionDictionary.Add(systemOptionContainerDTO.OptionType, new Dictionary<string, string>());
            }
            if (systemOptionDictionary[systemOptionContainerDTO.OptionType].ContainsKey(systemOptionContainerDTO.OptionName))
            {
                log.LogMethodExit(null, "duplicate system option " + systemOptionContainerDTO.OptionName);
                return;
            }
            systemOptionDictionary[systemOptionContainerDTO.OptionType].Add(systemOptionContainerDTO.OptionName, systemOptionContainerDTO.OptionValue);
            log.LogMethodExit();
        }

        public SystemOptionViewContainer(int siteId)//changed from internal to public
            : this(siteId, GetSystemOptionContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        private static SystemOptionContainerDTOCollection GetSystemOptionContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            SystemOptionContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                ISystemOptionUseCases systemOptionUseCases = SystemOptionUseCaseFactory.GetSystemOptionUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<SystemOptionContainerDTOCollection> task = systemOptionUseCases.GetSystemOptionContainerDTOCollection(siteId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving SystemOptionContainerDTOCollection.", ex);
                result = new SystemOptionContainerDTOCollection();
            }

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the default value for the default value name
        /// </summary>
        /// <param name="optionName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// returns the latest in SystemOptionContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        public SystemOptionContainerDTOCollection GetSystemOptionContainerDTOCollection(string hash)//changed from internal to public
        {
            log.LogMethodEntry(hash);
            if (systemOptionContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(systemOptionContainerDTOCollection);
            return systemOptionContainerDTOCollection;
        }

        internal SystemOptionViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            SystemOptionContainerDTOCollection latestSystemOptionContainerDTOCollection = GetSystemOptionContainerDTOCollection(siteId, systemOptionContainerDTOCollection.Hash, rebuildCache);
            if (latestSystemOptionContainerDTOCollection == null ||
                latestSystemOptionContainerDTOCollection.SystemOptionContainerDTOList == null ||
                latestSystemOptionContainerDTOCollection.SystemOptionContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            SystemOptionViewContainer result = new SystemOptionViewContainer(siteId, latestSystemOptionContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}
