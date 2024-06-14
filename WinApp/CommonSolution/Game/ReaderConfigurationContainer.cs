/********************************************************************************************
 * Project Name - Games  
 * Description  - ReaderConfigurationContainer class to get the data    
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API.
 2.110.0     16-Dec-2020       Prajwal S                 Updated for Changes in Container API.
 ********************************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Timers;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Game
{
    class ReaderConfigurationContainer : BaseContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private readonly object locker = new object();
        private DateTime? maxLastUpdatedDate;
        private string hash;
        private readonly Timer refreshTimer;
        private DateTime? buildTime;
        private ConcurrentDictionary<int, ReaderConfigurationGetBL> readerConfigurationGetBLDictionary;
        private readonly int siteId;
        private DateTime? machineAttributeLastUpdateTime;
        private ConcurrentDictionary<int, MachineAttributeDTO> machineAttributeDTODictionary;
        private readonly List<MachineAttributeDTO> machineAttributeDTOList;

        public List<ReaderConfigurationContainerDTO> GetReaderConfigurationContainerDTOList()
        {
            log.LogMethodEntry();
            List<ReaderConfigurationContainerDTO> machineAttributeViewDTOList = new List<ReaderConfigurationContainerDTO>();
            foreach (MachineAttributeDTO machineAttributeDTO in machineAttributeDTOList)
            {

                ReaderConfigurationContainerDTO machineAttributeViewDTO = new ReaderConfigurationContainerDTO(machineAttributeDTO.AttributeId, machineAttributeDTO.AttributeName, machineAttributeDTO.AttributeValue, machineAttributeDTO.IsFlag,
                                                                                                              machineAttributeDTO.IsSoftwareAttribute, machineAttributeDTO.ContextOfAttribute);
                machineAttributeViewDTOList.Add(machineAttributeViewDTO);
            }
            log.LogMethodExit(machineAttributeViewDTOList);
            return machineAttributeViewDTOList;
        }

        internal ReaderConfigurationContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            machineAttributeDTODictionary = new ConcurrentDictionary<int, MachineAttributeDTO>();
            machineAttributeDTOList = new List<MachineAttributeDTO>();
            MachineAttributeListBL machineAttributeList = new MachineAttributeListBL(executionContext);
            machineAttributeLastUpdateTime = machineAttributeList.GetAttributeModuleLastUpdateTime(siteId);

            List<KeyValuePair<MachineAttributeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MachineAttributeDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<MachineAttributeDTO.SearchByParameters, string>(MachineAttributeDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<MachineAttributeDTO.SearchByParameters, string>(MachineAttributeDTO.SearchByParameters.SITE_ID, siteId.ToString()));
            machineAttributeDTOList = machineAttributeList.GetMachineAttributes(searchParameters, MachineAttributeDTO.AttributeContext.SYSTEM);
            if (machineAttributeDTOList != null && machineAttributeDTOList.Any())
            {
                foreach (MachineAttributeDTO machineAttributeDTO in machineAttributeDTOList)
                {
                    machineAttributeDTODictionary[machineAttributeDTO.AttributeId] = machineAttributeDTO;
                }
            }
            else
            {
                machineAttributeDTOList = new List<MachineAttributeDTO>();
                machineAttributeDTODictionary = new ConcurrentDictionary<int, MachineAttributeDTO>();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Rebulds the container
        /// </summary>

        public ReaderConfigurationContainer Refresh() //added
        {
            log.LogMethodEntry();
            MachineAttributeListBL machineAttributeListBL = new MachineAttributeListBL(executionContext);
            DateTime? updateTime = machineAttributeListBL.GetAttributeModuleLastUpdateTime(siteId);
            if (machineAttributeLastUpdateTime.HasValue
                && machineAttributeLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in Game since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            ReaderConfigurationContainer result = new ReaderConfigurationContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }

    }
}

