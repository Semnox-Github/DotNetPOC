/********************************************************************************************
 * Project Name - Games
 * Description  - MachineMasterList class to get the List of games from the container API
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
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Game
{
    public static class MachineMasterList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<int, MachineContainer> machineContainerDictionary = new ConcurrentDictionary<int, MachineContainer>();
        private static readonly object locker = new object();

        private static MachineContainer GetMachineContainer(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            if (machineContainerDictionary.ContainsKey(executionContext.GetSiteId()) == false)
            {
                machineContainerDictionary[executionContext.GetSiteId()] = new MachineContainer(executionContext.GetSiteId());
            }
            MachineContainer machineContainer = machineContainerDictionary[executionContext.GetSiteId()];
            log.LogMethodExit(machineContainer);
            return machineContainer;
        }


        //public static List<MachineDTO> GetMachinesList(ExecutionContext executionContext, DateTime? maxLastUpdatedDate, string hash)
        //{
        //    log.LogMethodEntry(executionContext, maxLastUpdatedDate, hash);
        //    List<MachineDTO> machineDTOList;
        //    lock (locker)
        //    {
        //        MachineContainer machineContainer = GetMachineContainer(executionContext);
        //        machineDTOList = machineContainer.GetMachineDTOListModifiedAfter(maxLastUpdatedDate, hash);
        //    }
        //    log.LogMethodExit(machineDTOList);
        //    return machineDTOList;
        //}

        //public static MachineDTO GetMachineDTO(ExecutionContext executionContext, int machineId)
        //{
        //    log.LogMethodEntry(executionContext, machineId);
        //    MachineDTO result;
        //    lock (locker)
        //    {
        //        lock (locker)
        //        {
        //            MachineContainer machineContainer = GetMachineContainer(executionContext);
        //            result = machineContainer.GetMachineDTO(machineId);
        //        }
        //    }
        //    log.LogMethodExit(result);
        //    return result;
        //}

        //public static MachineDTO GetMachineDTO(ExecutionContext executionContext, string machineName)
        //{
        //    log.LogMethodEntry(executionContext, machineName);
        //    MachineDTO result;
        //    lock (locker)
        //    {
        //        lock (locker)
        //        {
        //            MachineContainer machineContainer = GetMachineContainer(executionContext);
        //            result = machineContainer.GetMachineDTO(machineName);
        //        }
        //    }
        //    log.LogMethodExit(result);
        //    return result;
        //}
        //public static MachineGetBL GetMahchineBL(ExecutionContext executionContext, int machineId)
        //{
        //    log.LogMethodEntry(executionContext, machineId);
        //    MachineGetBL machinesGetBL;
        //    lock (locker)
        //    {
        //        MachineContainer machineContainer = GetMachineContainer(executionContext);
        //        machinesGetBL = machineContainer.GetMachineBL(machineId);
        //    }
        //    log.LogMethodExit(machinesGetBL);
        //    return machinesGetBL;
        //}

        /// <summary>
        /// clears the master list
        /// </summary>
        public static void Clear()
        {
            log.LogMethodEntry();
            lock (locker)
            {
                foreach (var machineContainer in machineContainerDictionary.Values)
                {
                    machineContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }
    }
}
