/********************************************************************************************
 * Project Name - Hub
 * Description  - HubContainer class to get the data    
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 2.110.0     11-Dec-2020       Prajwal S                 Modified for WEB API changes.
 ********************************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Timers;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Game
{
    class HubContainer : BaseContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private string hash;
        private readonly int siteId; // added
        private readonly Timer refreshTimer;
        private DateTime? hubLastUpdateTime; //added
        private ConcurrentDictionary<int, HubDTO> hubDTODictionary; //added
        private readonly List<HubDTO> hubDTOList;
        private DateTime? buildTime;

        internal HubContainer(int siteId) // added
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            hubDTODictionary = new ConcurrentDictionary<int, HubDTO>();
            hubDTOList = new List<HubDTO>();
            HubList hubList = new HubList(executionContext);
            hubLastUpdateTime = hubList.GetHubModuleLastUpdateTime(siteId);

            List<KeyValuePair<HubDTO.SearchByHubParameters, string>> searchParameters = new List<KeyValuePair<HubDTO.SearchByHubParameters, string>>();
            searchParameters.Add(new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.SITE_ID, siteId.ToString()));
            hubDTOList = hubList.GetHubSearchList(searchParameters);
            if (hubDTOList != null && hubDTOList.Any())
            {
                foreach (HubDTO hubDTO in hubDTOList)
                {
                    hubDTODictionary[hubDTO.MasterId] = hubDTO;
                }
            }
            else
            {
                hubDTOList = new List<HubDTO>();
                hubDTODictionary = new ConcurrentDictionary<int, HubDTO>();
            }
            log.LogMethodExit();
        }

        public List<HubContainerDTO> GetHubContainerDTOList() //added
        {
            log.LogMethodEntry();
            List<HubContainerDTO> hubViewDTOList = new List<HubContainerDTO>();
            foreach (HubDTO hubDTO in hubDTOList)
            {

                HubContainerDTO hubViewDTO = new HubContainerDTO(hubDTO.MasterId,
                hubDTO.MasterName,
                hubDTO.PortNumber,
                hubDTO.BaudRate,
                hubDTO.Notes,
                hubDTO.Address,
                hubDTO.Frequency,
                hubDTO.ServerMachine,
                hubDTO.DirectMode,
                hubDTO.IPAddress,
                hubDTO.TCPPort,
                hubDTO.MACAddress,
                hubDTO.RestartAP,
                hubDTO.IsEByte,
                hubDTO.IsRadian
                );
                hubViewDTOList.Add(hubViewDTO);
            }
            log.LogMethodExit(hubViewDTOList);
            return hubViewDTOList;
        }


        public HubContainer Refresh() //added
        {
            log.LogMethodEntry();
            HubList hubListBL = new HubList(executionContext);
            DateTime? updateTime = hubListBL.GetHubModuleLastUpdateTime(siteId);
            if (hubLastUpdateTime.HasValue
                && hubLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in Hub since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            HubContainer result = new HubContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}