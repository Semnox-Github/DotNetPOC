/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - MachineViewContainer class
 *
 **************
 ** Version Log
  **************
  * Version     Date Modified By Remarks
 *********************************************************************************************
 0.0         10-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// Holds the list of Machines
    /// </summary>
    public class MachineViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly MachineContainerDTOCollection machineDTOCollection;
        private readonly ConcurrentDictionary<int, MachineContainerDTO> machineContainerDTODictionary = new ConcurrentDictionary<int, MachineContainerDTO>();
        private readonly int siteId;
        //private DateTime lastRefreshTime;
        //private readonly object locker = new object();

        internal MachineViewContainer(int siteId, MachineContainerDTOCollection machineDTOCollection)
        {
            log.LogMethodEntry(siteId, machineDTOCollection);
            this.siteId = siteId;
            this.machineDTOCollection = machineDTOCollection;
            //lastRefreshTime = DateTime.Now;
            if (machineDTOCollection != null &&
               machineDTOCollection.MachineContainerDTOList != null &&
               machineDTOCollection.MachineContainerDTOList.Any())
            {
                foreach (var machineContainerDTO in machineDTOCollection.MachineContainerDTOList)
                {
                    machineContainerDTODictionary[machineContainerDTO.MachineId] = machineContainerDTO;
                }
            }
            log.LogMethodExit();
        }

        internal MachineViewContainer(int siteId) :
            this(siteId, GetMachineContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        private static MachineContainerDTOCollection GetMachineContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {

            log.LogMethodEntry(siteId, hash, rebuildCache);
            MachineContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IMachineUseCases machineUseCases = GameUseCaseFactory.GetMachineUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<MachineContainerDTOCollection> machineViewDTOCollectionTask = machineUseCases.GetMachineContainerDTOCollection(siteId, hash, rebuildCache);
                    machineViewDTOCollectionTask.Wait();
                    result = machineViewDTOCollectionTask.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving MachineContainerDTOCollection.", ex);
                result = new MachineContainerDTOCollection();
            }

            return result;
        }

        /// <summary>
        /// returns the latest in MachineDTOCollection
        /// </summary>
        /// <returns></returns>
        internal MachineContainerDTOCollection GetMachineDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (machineDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(machineDTOCollection);
            return machineDTOCollection;
        }

        /// <summary>
        /// returns the MachineContainerDTO for the MachineId
        /// </summary>
        /// <param name="MachineId"></param>
        /// <returns></returns>
        public MachineContainerDTO GetMachineContainerDTO(int MachineId)
        {
            log.LogMethodEntry(MachineId);
            if (machineContainerDTODictionary.ContainsKey(MachineId) == false)
            {
                string errorMessage = "Machine with Machine Id :" + MachineId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            MachineContainerDTO result = machineContainerDTODictionary[MachineId];
            log.LogMethodExit(result);
            return result;
        }


        internal MachineViewContainer Refresh()
        {
            log.LogMethodEntry();
            //if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            //{
            //    log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
            //    return this;
            //}
            LastRefreshTime = DateTime.Now;
            MachineContainerDTOCollection latestMachineDTOCollection = GetMachineContainerDTOCollection(siteId, machineDTOCollection.Hash, true);
            if (latestMachineDTOCollection == null ||
                latestMachineDTOCollection.MachineContainerDTOList == null ||
                latestMachineDTOCollection.MachineContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            MachineViewContainer result = new MachineViewContainer(siteId, latestMachineDTOCollection);
            log.LogMethodExit(result);
            return result;
        }


    }
}

