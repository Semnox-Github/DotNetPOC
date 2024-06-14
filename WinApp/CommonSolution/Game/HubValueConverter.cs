/********************************************************************************************
 * Project Name - Machine                                                                          
 * Description  - Bulk Upload Mapper HubDTO Class 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60       16-Mar-2019   Muhammed Mehraj  Created 
 *2.70.2       12-Aug-2019   Deeksha          Added logger methods.
 ********************************************************************************************/
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.Game
{
    public class HubValueConverter : ValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        Dictionary<int, HubDTO> hubIdHubDTODictionary;
        Dictionary<string, HubDTO> hubNameHubDTODictionary;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public HubValueConverter(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            hubNameHubDTODictionary = new Dictionary<string, HubDTO>();
            hubIdHubDTODictionary = new Dictionary<int, HubDTO>();
            List<HubDTO> hubList = null;
            HubList hubDTOList = new HubList(executionContext);
            List<KeyValuePair<HubDTO.SearchByHubParameters, string>> searchParameters = new List<KeyValuePair<HubDTO.SearchByHubParameters, string>>();
            searchParameters.Add(new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            hubList = hubDTOList.GetHubSearchList(searchParameters);
            if (hubList != null && hubList.Count > 0)
            {
                foreach (var hub in hubList)
                {
                    if (hubIdHubDTODictionary.ContainsKey(hub.MasterId) == false)
                    {
                        hubIdHubDTODictionary.Add(hub.MasterId, hub);
                    }
                    if (hubNameHubDTODictionary.ContainsKey(hub.MasterName) == false)
                    {
                        hubNameHubDTODictionary.Add(hub.MasterName.ToUpper(), hub);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts hubname to masterid
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public override object FromString(string stringValue)
        {
            log.LogMethodEntry(stringValue);
            int masterId = -1;
            if (hubNameHubDTODictionary != null && hubNameHubDTODictionary.ContainsKey(stringValue.ToUpper()))
            {
                masterId = hubNameHubDTODictionary[stringValue.ToUpper()].MasterId;
            }
            log.LogMethodExit(masterId);
            return masterId;
        }

        /// <summary>
        /// Converts masterid to hubname
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string ToString(object value)
        {
            log.LogMethodEntry(value);
            string masterName = string.Empty;
            if (hubIdHubDTODictionary != null && hubIdHubDTODictionary.ContainsKey(Convert.ToInt32(value)))
            {
                masterName = hubIdHubDTODictionary[Convert.ToInt32(value)].MasterName;
            }
            log.LogMethodExit(masterName);
            return masterName;
        }
    }
}
