/********************************************************************************************
 * Project Name - Site
 * Description  - RoamingSiteContainer class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.110.0     21-Dec-2020      Lakshminarayana           Created for POS UI Redesign.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Semnox.Parafait.Site
{
    public class RoamingSiteContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly List<RoamingSiteDTO> roamingSiteDTOList;
        private readonly DateTime? roamingSiteLastUpdateTime;
        private readonly HashSet<int> roamingSiteIdHashSet = new HashSet<int>();
        internal RoamingSiteContainer()
        {
            log.LogMethodEntry();
            try
            {
                RoamingSiteListBL roamingSiteListBL = new RoamingSiteListBL();
                roamingSiteLastUpdateTime = roamingSiteListBL.GetRoamingSiteModuleLastUpdateTime();

                List<KeyValuePair<RoamingSiteDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<RoamingSiteDTO.SearchByParameters, string>>();
                roamingSiteDTOList = roamingSiteListBL.GetAllRoamingSiteDTOList(searchParameters);
                if (roamingSiteDTOList == null)
                {
                    roamingSiteDTOList = new List<RoamingSiteDTO>();
                }
                if (roamingSiteDTOList.Any())
                {
                    foreach (RoamingSiteDTO roamingSiteDTO in roamingSiteDTOList)
                    {
                        roamingSiteIdHashSet.Add(roamingSiteDTO.RoamingSiteId);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while creating the roamingSite container.", ex);
                roamingSiteDTOList = new List<RoamingSiteDTO>();
                roamingSiteIdHashSet.Clear();
            }
            log.LogMethodExit();
        }


        internal bool IsRoamingSite(int siteId)
        {
            log.LogMethodEntry();
            var result = roamingSiteIdHashSet.Contains(siteId);
            log.LogMethodExit(result);
            return result;
        }


        public RoamingSiteContainer Refresh()
        {
            log.LogMethodEntry();
            RoamingSiteListBL roamingSiteListBL = new RoamingSiteListBL();
            DateTime? updateTime = roamingSiteListBL.GetRoamingSiteModuleLastUpdateTime();
            if (roamingSiteLastUpdateTime.HasValue
                && roamingSiteLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in Roaming Site since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            RoamingSiteContainer result = new RoamingSiteContainer();
            log.LogMethodExit(result);
            return result;
        }
    }
}
