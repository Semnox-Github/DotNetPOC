/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - LookupsContainer class to get the List of lookup from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.160.0      24-Jul-2022      Prajwal S                 Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Semnox.Core.Utilities
{
    public class LookupsContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Dictionary<int, LookupsContainerDTO> lookupsIdlookupsDTODictionary = new Dictionary<int, LookupsContainerDTO>();
        private readonly Dictionary<string, LookupsContainerDTO> lookupsNamelookupsDTODictionary = new Dictionary<string, LookupsContainerDTO>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<int, LookupValuesContainerDTO> lookupValueIdLookupValuesContainerDTODictionary = new Dictionary<int, LookupValuesContainerDTO>();
        private readonly List<LookupsDTO> lookupsDTOList;
        private readonly LookupsContainerDTOCollection lookupsContainerDTOCollection;
        private readonly DateTime? lookupsModuleLastUpdateTime;
        private readonly int siteId;

        /// <summary>
        /// Default Container Constructor
        /// </summary>
        /// <param name="siteId"></param>
        public LookupsContainer(int siteId) : this(siteId, GetLookupsDTOList(siteId), GetLookupsModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with parameters siteId, lookupsDTOList, lookupsModuleLastUpdateTime
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="lookupsDTOList"></param>
        /// <param name="lookupsModuleLastUpdateTime"></param>
        public LookupsContainer(int siteId, List<LookupsDTO> lookupsDTOList, DateTime? lookupsModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId, lookupsDTOList, lookupsModuleLastUpdateTime);
            this.siteId = siteId;
            this.lookupsDTOList = lookupsDTOList;
            this.lookupsModuleLastUpdateTime = lookupsModuleLastUpdateTime;
            List<LookupsContainerDTO> lookupsContainerDTOList = new List<LookupsContainerDTO>();
            foreach (LookupsDTO lookupsDTO in lookupsDTOList)
            {
                if (lookupsIdlookupsDTODictionary.ContainsKey(lookupsDTO.LookupId))
                {
                    continue;
                }
                LookupsContainerDTO lookupsContainerDTO = new LookupsContainerDTO(lookupsDTO.LookupId, lookupsDTO.LookupName, lookupsDTO.IsProtected);
                foreach (LookupValuesDTO lookupValuesDTO in lookupsDTO.LookupValuesDTOList)
                {
                    LookupValuesContainerDTO lookupValuesContainerDTO = new LookupValuesContainerDTO(lookupValuesDTO.LookupValueId, lookupValuesDTO.LookupValue, lookupValuesDTO.Description, lookupsDTO.LookupName);
                    lookupsContainerDTO.LookupValuesContainerDTOList.Add(lookupValuesContainerDTO);
                    if(lookupValueIdLookupValuesContainerDTODictionary.ContainsKey(lookupValuesContainerDTO.LookupValueId) == false)
                    {
                        lookupValueIdLookupValuesContainerDTODictionary.Add(lookupValuesContainerDTO.LookupValueId, lookupValuesContainerDTO);
                    }
                }
                lookupsContainerDTOList.Add(lookupsContainerDTO);
                lookupsIdlookupsDTODictionary.Add(lookupsContainerDTO.LookupId, lookupsContainerDTO);
                if (lookupsNamelookupsDTODictionary.ContainsKey(lookupsDTO.LookupName) == false)
                {
                    lookupsNamelookupsDTODictionary[lookupsDTO.LookupName] = lookupsContainerDTO;
                }
            }
            lookupsContainerDTOCollection = new LookupsContainerDTOCollection(lookupsContainerDTOList);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get the latest update time of Lookups table from DB.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private static DateTime? GetLookupsModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                LookupsList lookupsListBL = new LookupsList();
                result = lookupsListBL.GetLookupModuleLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the Lookups max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Get all the active Lookups records for the given siteId.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private static List<LookupsDTO> GetLookupsDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<LookupsDTO> lookupsDTOList = null;
            try
            {
                LookupsList lookupsListBL = new LookupsList();
                List<KeyValuePair<LookupsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LookupsDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                searchParameters.Add(new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.ISACTIVE, "1"));
                lookupsDTOList = lookupsListBL.GetAllLookups(searchParameters, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the Lookups.", ex);
            }

            if (lookupsDTOList == null)
            {
                lookupsDTOList = new List<LookupsDTO>();
            }
            log.LogMethodExit(lookupsDTOList);
            return lookupsDTOList;
        }

        /// <summary>
        /// Returns lookupsContainerDTOCollection.
        /// </summary>
        /// <returns></returns>
        public LookupsContainerDTOCollection GetLookupsContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(lookupsContainerDTOCollection);
            return lookupsContainerDTOCollection;
        }

        public LookupsContainerDTO GetLookupsContainerDTO(string lookupName)
        {
            log.LogMethodEntry(lookupName);
            if (lookupsNamelookupsDTODictionary.ContainsKey(lookupName) == false)
            {
                string errorMessage = "Lookups with Lookups Name :" + lookupName + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            LookupsContainerDTO result = lookupsNamelookupsDTODictionary[lookupName];
            log.LogMethodExit(result);
            return result;
        }

        public LookupsContainerDTO GetLookupsContainerDTOOrDefault(string lookupName)
        {
            log.LogMethodEntry(lookupName);
            LookupsContainerDTO result = null;
            if (lookupsNamelookupsDTODictionary.ContainsKey(lookupName) == false)
            {
                log.LogMethodExit(result, "Lookups with Lookups Name :" + lookupName + " doesn't exists.");
                return result;
            }
            result = lookupsNamelookupsDTODictionary[lookupName];
            log.LogMethodExit(result);
            return result;
        }

        public LookupValuesContainerDTO GetLookupValuesContainerDTO(int lookupValueId)
        {
            log.LogMethodEntry(lookupValueId);
            if (lookupValueIdLookupValuesContainerDTODictionary.ContainsKey(lookupValueId) == false)
            {
                string errorMessage = "Lookup Value with LookupValueId :" + lookupValueId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            LookupValuesContainerDTO result = lookupValueIdLookupValuesContainerDTODictionary[lookupValueId];
            log.LogMethodExit(result);
            return result;
        }

        public LookupValuesContainerDTO GetLookupValuesContainerDTO(string lookupName, string lookupValue)
        {
            log.LogMethodEntry(lookupName, lookupValue);
            LookupsContainerDTO lookupsContainerDTO = GetLookupsContainerDTO(lookupName);
            if(lookupsContainerDTO.LookupValuesContainerDTOList == null ||
                lookupsContainerDTO.LookupValuesContainerDTOList.Any(x => x.LookupValue.ToLower() == lookupValue.ToLower()) == false)
            {
                string errorMessage = "Lookup Value :"+ lookupValue + " with lookup Name :" + lookupName + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            LookupValuesContainerDTO result = lookupsContainerDTO.LookupValuesContainerDTOList.First(x => x.LookupValue.ToLower() == lookupValue.ToLower());
            log.LogMethodExit(result);
            return result;
        }

        public LookupValuesContainerDTO GetLookupValuesContainerDTOOrDefault(string lookupName, string lookupValue)
        {
            log.LogMethodEntry(lookupName, lookupValue);
            LookupValuesContainerDTO result = null;
            LookupsContainerDTO lookupsContainerDTO = GetLookupsContainerDTO(lookupName);
            if (lookupsContainerDTO == null ||
                lookupsContainerDTO.LookupValuesContainerDTOList == null ||
                lookupsContainerDTO.LookupValuesContainerDTOList.Any(x => x.LookupValue.ToLower() == lookupValue.ToLower()) == false)
            {
                log.LogMethodExit(result, "Lookup Value :" + lookupValue + " with lookup Name :" + lookupName + " doesn't exists.");
                return result;
            }
            result = lookupsContainerDTO.LookupValuesContainerDTOList.First(x => x.LookupValue.ToLower() == lookupValue.ToLower());
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Refresh the container if there is any update in Db.
        /// </summary>
        /// <returns></returns>
        public LookupsContainer Refresh()
        {
            log.LogMethodEntry();
            LookupsList lookupsListBL = new LookupsList();
            DateTime? updateTime = lookupsListBL.GetLookupModuleLastUpdateTime(siteId);
            if (lookupsModuleLastUpdateTime.HasValue
                && lookupsModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in Lookups since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            LookupsContainer result = new LookupsContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
