/********************************************************************************************
 * Project Name - Games
 * Description  - ReaderConfigurationMasterList class to get the List of games from the container API
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
    public static  class ReaderConfigurationMasterList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<int, ReaderConfigurationContainer> readerconfigurationContainerDictionary = new ConcurrentDictionary<int, ReaderConfigurationContainer>();
        private static readonly object locker = new object();

        private static ReaderConfigurationContainer GetReaderConfigurationContainer(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            if (readerconfigurationContainerDictionary.ContainsKey(executionContext.GetSiteId()) == false)
            {
                readerconfigurationContainerDictionary[executionContext.GetSiteId()] = new ReaderConfigurationContainer(executionContext.GetSiteId());
            }
            ReaderConfigurationContainer readerConfigurationContainer = readerconfigurationContainerDictionary[executionContext.GetSiteId()];
            log.LogMethodExit(readerConfigurationContainer);
            return readerConfigurationContainer;
        }


        //public static List<MachineAttributeDTO> GetAttributeList(ExecutionContext executionContext, DateTime? maxLastUpdatedDate, string hash)
        //{
        //    log.LogMethodEntry(executionContext, maxLastUpdatedDate, hash);
        //    List<MachineAttributeDTO> machineAttributeDTOList;
        //    lock (locker)
        //    {
        //        ReaderConfigurationContainer readerConfigurationContainer = GetReaderConfigurationContainer(executionContext);
        //        machineAttributeDTOList = readerConfigurationContainer.GetMachineAttributeDTOListModifiedAfter(maxLastUpdatedDate, hash);
        //    }
        //    log.LogMethodExit(machineAttributeDTOList);
        //    return machineAttributeDTOList;
        //}


        //public static MachineAttributeDTO GetMachineAttributeDTO(ExecutionContext executionContext, int attributeId)
        //{
        //    log.LogMethodEntry(executionContext, attributeId);
        //    MachineAttributeDTO result;
        //    lock (locker)
        //    {
        //        lock (locker)
        //        {
        //            ReaderConfigurationContainer readerConfigurationContainer = GetReaderConfigurationContainer(executionContext);
        //            result = readerConfigurationContainer.GetMachineAttributeDTO(attributeId);
        //        }
        //    }
        //    log.LogMethodExit(result);
        //    return result;
        //}
        //public static MachineAttributeDTO GetMachineAttributeDTO(ExecutionContext executionContext, MachineAttributeDTO.MachineAttribute attributeName)
        //{
        //    log.LogMethodEntry(executionContext, attributeName);
        //    MachineAttributeDTO result;
        //    lock (locker)
        //    {
        //        lock (locker)
        //        {
        //            ReaderConfigurationContainer readerConfigurationContainer = GetReaderConfigurationContainer(executionContext);
        //            result = readerConfigurationContainer.GetMachineAttributeDTO(attributeName);
        //        }
        //    }
        //    log.LogMethodExit(result);
        //    return result;
        //}
       
        //public static ReaderConfigurationGetBL GetReaderConfigurationBL(ExecutionContext executionContext, int attributeId)
        //{
        //    log.LogMethodEntry(executionContext, attributeId);
        //    ReaderConfigurationGetBL readerConfigurationGetBL;
        //    lock (locker)
        //    {
        //        ReaderConfigurationContainer readerConfigurationContainer = GetReaderConfigurationContainer(executionContext);
        //        readerConfigurationGetBL = readerConfigurationContainer.GetReaderConfigurationBL(attributeId);
        //    }
        //    log.LogMethodExit(readerConfigurationGetBL);
        //    return readerConfigurationGetBL;
        //}

        /// <summary>
        /// clears the master list
        /// </summary>
        public static void Clear()
        {
            log.LogMethodEntry();
            lock (locker)
            {
                foreach (var readerConfigurationContainer in readerconfigurationContainerDictionary.Values)
                {
                    readerConfigurationContainer.RebuildCache();
                }
            }
            log.LogMethodExit();
        }
    }
}
