/********************************************************************************************
 * Project Name - Digital Signage
 * Description  - ThemeContainer class to get the data    
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
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Timers;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.DigitalSignage
{
    class ThemeContainer : BaseContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private readonly object locker = new object();
        private DateTime? maxLastUpdatedDate;
        private string hash;
        private readonly int siteId;
        private DateTime? themeLastUpdateTime;
        private ConcurrentDictionary<int, ThemeDTO> themeDTODictionary;
        private readonly Timer refreshTimer;
        private readonly List<ThemeDTO> themeDTOList;
        private DateTime? buildTime;

        public List<ThemeContainerDTO> GetThemeContainerDTOList()
        {
            log.LogMethodEntry();
            List<ThemeContainerDTO> themeViewDTOList = new List<ThemeContainerDTO>();
            foreach (ThemeDTO themeDTO in themeDTOList)
            {
                ThemeContainerDTO themeViewDTO = new ThemeContainerDTO(themeDTO.Id, themeDTO.Name, themeDTO.TypeId, themeDTO.Description, themeDTO.InitialScreenId, themeDTO.ThemeNumber);
                themeViewDTOList.Add(themeViewDTO);
            }
            log.LogMethodExit(themeViewDTOList);
            return themeViewDTOList;
        }

        internal ThemeContainer(int siteId, ExecutionContext executionContext = null)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            themeDTODictionary = new ConcurrentDictionary<int, ThemeDTO>();
            themeDTOList = new List<ThemeDTO>();
            ThemeListBL themeList = new ThemeListBL(executionContext);
            themeLastUpdateTime = themeList.GetThemeModuleLastUpdateTime(siteId);

            List<KeyValuePair<ThemeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ThemeDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.SITE_ID, siteId.ToString()));
            themeDTOList = themeList.GetThemeDTOList(searchParameters);
            if (themeDTOList != null && themeDTOList.Any())
            {
                foreach (ThemeDTO themeDTO in themeDTOList)
                {
                    themeDTODictionary[themeDTO.Id] = themeDTO;
                }
            }
            else
            {
                themeDTOList = new List<ThemeDTO>();
                themeDTODictionary = new ConcurrentDictionary<int, ThemeDTO>();
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Rebulds the container
        /// </summary>

        public ThemeContainer Refresh() //added
        {
            log.LogMethodEntry();
            ThemeListBL themeListBL = new ThemeListBL(executionContext);
            DateTime? updateTime = themeListBL.GetThemeModuleLastUpdateTime(siteId);
            if (themeLastUpdateTime.HasValue
                && themeLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in Theme since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            ThemeContainer result = new ThemeContainer(siteId, executionContext);
            log.LogMethodExit(result);
            return result;
        }

    }
}