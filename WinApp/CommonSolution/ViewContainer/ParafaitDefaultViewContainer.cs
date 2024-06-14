
/********************************************************************************************
 * Project Name - Utilities
 * Description  - ParafaitDefaultViewContainer holds the parafait default values for a given siteId, userId and POSMachineId
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 2.130.0        12-Jul-2021    Lakshminarayana      Modified : Static menu enhancement
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// ParafaitDefaultViewContainer holds the parafait default values for a given siteId, userId and POSMachineId
    /// </summary>
    public class ParafaitDefaultViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ParafaitDefaultContainerDTOCollection parafaitDefaultContainerDTOCollection;
        private readonly ConcurrentDictionary<string, string> parafaitDefaultDictionary = new ConcurrentDictionary<string, string>();
        private readonly int siteId;
        private readonly int userPkId;
        private readonly int machineId;

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="userPkId">user primary key</param>
        /// <param name="machineId">machine id</param>
        /// <param name="parafaitDefaultContainerDTOCollection">parafaitDefaultContainerDTOCollection</param>
        internal ParafaitDefaultViewContainer(int siteId, int userPkId, int machineId, ParafaitDefaultContainerDTOCollection parafaitDefaultContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, userPkId, machineId, parafaitDefaultContainerDTOCollection);
            this.siteId = siteId;
            this.userPkId = userPkId;
            this.machineId = machineId;
            this.parafaitDefaultContainerDTOCollection = parafaitDefaultContainerDTOCollection;
            if (parafaitDefaultContainerDTOCollection != null &&
                parafaitDefaultContainerDTOCollection.ParafaitDefaultContainerDTOList != null &&
                parafaitDefaultContainerDTOCollection.ParafaitDefaultContainerDTOList.Any())
            {
                foreach (var parafaitDefaultContainerDTO in parafaitDefaultContainerDTOCollection.ParafaitDefaultContainerDTOList)
                {
                    parafaitDefaultDictionary[parafaitDefaultContainerDTO.DefaultValueName] = parafaitDefaultContainerDTO.DefaultValue;
                }
            }
            log.LogMethodExit();
        }

        internal ParafaitDefaultViewContainer(int siteId, int userPkId, int machineId)
            :this(siteId, userPkId, machineId, GetParafaitDefaultContainerDTOCollection(siteId, userPkId, machineId, null, false))
        {
            log.LogMethodEntry(siteId, userPkId, machineId);
            log.LogMethodExit();
        }

        private static ParafaitDefaultContainerDTOCollection GetParafaitDefaultContainerDTOCollection(int siteId, int userPkId, int machineId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId, userPkId, machineId);
            ParafaitDefaultContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IParafaitDefaultUseCases parafaitDefaultUseCases = ParafaitDefaultUseCaseFactory.GetParafaitDefaultUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<ParafaitDefaultContainerDTOCollection> task = parafaitDefaultUseCases.GetParafaitDefaultContainerDTOCollection(siteId, userPkId, machineId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving ParafaitDefaultContainerDTOCollection.", ex);
                result = new ParafaitDefaultContainerDTOCollection();
            }
            
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the default value for the default value name
        /// </summary>
        /// <param name="defaultValueName"></param>
        /// <returns></returns>
        public string GetParafaitDefault(string defaultValueName)
        {
            log.LogMethodEntry(defaultValueName);
            string result = string.Empty;
            if (parafaitDefaultDictionary.ContainsKey(defaultValueName) == false)
            {
                string errorMessage = "Unable to find the default value: " + defaultValueName;
                log.Error(errorMessage);
                log.LogMethodExit(result, errorMessage);
                return result;
            }
            log.LogMethodExit();
            return parafaitDefaultDictionary[defaultValueName];
        }

        /// <summary>
        /// returns the latest in DefaultViewDTOCollection
        /// </summary>
        /// <returns></returns>
        internal ParafaitDefaultContainerDTOCollection GetDefaultViewDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (parafaitDefaultContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(parafaitDefaultContainerDTOCollection);
            return parafaitDefaultContainerDTOCollection;
        }

        internal ParafaitDefaultViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if(LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            ParafaitDefaultContainerDTOCollection latestParafaitDefaultContainerDTOCollection = GetParafaitDefaultContainerDTOCollection(siteId, userPkId, machineId, parafaitDefaultContainerDTOCollection.Hash, rebuildCache);
            if (latestParafaitDefaultContainerDTOCollection == null || 
                latestParafaitDefaultContainerDTOCollection.ParafaitDefaultContainerDTOList== null ||
                latestParafaitDefaultContainerDTOCollection.ParafaitDefaultContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            ParafaitDefaultViewContainer result = new ParafaitDefaultViewContainer(siteId, userPkId, machineId, latestParafaitDefaultContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}
