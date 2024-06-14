/********************************************************************************************
 * Project Name - POS
 * Description  - POSMachineViewContainer holds the POS machine values for a given siteId
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
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.POS;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// POSMachineViewContainer holds the POS machine values for a given siteId
    /// </summary>
    public class POSMachineViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ConcurrentDictionary<int, POSMachineContainerDTO> posMachineIdPOSMachineContainerDTODictionary = new ConcurrentDictionary<int, POSMachineContainerDTO>();
        private readonly POSMachineContainerDTOCollection posMachineContainerDTOCollection;
        private readonly int siteId;

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="posMachineContainerDTOCollection">posMachineContainerDTOCollection</param>
        internal POSMachineViewContainer(int siteId, POSMachineContainerDTOCollection posMachineContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, posMachineContainerDTOCollection);
            this.siteId = siteId;
            this.posMachineContainerDTOCollection = posMachineContainerDTOCollection;
            if (posMachineContainerDTOCollection != null &&
                posMachineContainerDTOCollection.POSMachineContainerDTOList != null &&
                posMachineContainerDTOCollection.POSMachineContainerDTOList.Any())
            {
                foreach (var posMachineContainerDTO in posMachineContainerDTOCollection.POSMachineContainerDTOList)
                {
                    posMachineIdPOSMachineContainerDTODictionary[posMachineContainerDTO.POSMachineId] = posMachineContainerDTO;
                }
            }
            log.LogMethodExit();
        }

        internal POSMachineViewContainer(int siteId)
            : this(siteId, GetPOSMachineContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        private static POSMachineContainerDTOCollection GetPOSMachineContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            POSMachineContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IPOSMachineUseCases pOSMachineUseCases = POSUseCaseFactory.GetPOSMachineUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<POSMachineContainerDTOCollection> task = pOSMachineUseCases.GetPOSMachineContainerDTOCollection(siteId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving POSMachineContainerDTOCollection.", ex);
                result = new POSMachineContainerDTOCollection();
            }

            log.LogMethodExit(result);
            return result;
        }

        internal POSMachineContainerDTO GetPOSMachineContainerDTO(int machineId)
        {
            log.LogMethodEntry(machineId);
            if (posMachineIdPOSMachineContainerDTODictionary.ContainsKey(machineId) == false)
            {
                string errorMessage = "POS with machine Id :" + machineId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            var result = posMachineIdPOSMachineContainerDTODictionary[machineId];
            log.LogMethodExit(result);
            return result;
        }
        internal List<POSMachineContainerDTO> GetPOSMachineContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(posMachineContainerDTOCollection.POSMachineContainerDTOList);
            return posMachineContainerDTOCollection.POSMachineContainerDTOList;
        }
        /// <summary>
        /// returns the latest in POSMachineContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        internal POSMachineContainerDTOCollection GetPOSMachineContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (posMachineContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(posMachineContainerDTOCollection);
            return posMachineContainerDTOCollection;
        }

        internal POSMachineViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            POSMachineContainerDTOCollection latestPOSMachineContainerDTOCollection = GetPOSMachineContainerDTOCollection(siteId, posMachineContainerDTOCollection.Hash, rebuildCache);
            if (latestPOSMachineContainerDTOCollection == null ||
                latestPOSMachineContainerDTOCollection.POSMachineContainerDTOList == null ||
                latestPOSMachineContainerDTOCollection.POSMachineContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            POSMachineViewContainer result = new POSMachineViewContainer(siteId, latestPOSMachineContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}
