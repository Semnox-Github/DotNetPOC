/********************************************************************************************
 * Project Name - Maintenance
 * Description  - Container to hold AssetTechnicianMapping
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.100.0    09-Oct-2020   Gururaja Kanjan         Created.
 ********************************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Maintenance
{
    public class AssetTechnicianMappingContainer
    {
		
		private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ConcurrentDictionary<int, int> assetMapperDictionary;

        private readonly object locker = new object();
        private DateTime? refreshTime;
        private DateTime? assetMappingLastUpdateTime;
		
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public AssetTechnicianMappingContainer(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            assetMapperDictionary = new ConcurrentDictionary<int, int>();
            RefreshAssetMapperList();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the technician for the given asset
        /// </summary>
        /// <param name="assetId">Asset ID</param>
        /// <returns>Returns int</returns>
        public int GetAssetTechnician(int assetId)
        {
            log.LogMethodEntry(assetId);
            int result = -1;
            lock (locker)
            {
                RefreshAssetMapperList();
                
                if (assetMapperDictionary.ContainsKey(assetId))
                {
                    result = assetMapperDictionary[assetId];
                }
            }

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Refreshes the container
        /// </summary>
        private void RefreshAssetMapperList()
        {
            log.LogMethodEntry();
            if (refreshTime.HasValue && refreshTime > DateTime.UtcNow)
            {
                log.LogMethodExit(null, "Refreshed the list in last 15 minutes.");
                return;
            }

            AssetList assetList = new AssetList(executionContext);

            // get latest last updated date of assets technician mapping
            AssetTechnicianMappingListBL assetTechnicianMappingListBL = new AssetTechnicianMappingListBL(executionContext);
            DateTime? updateTime = assetTechnicianMappingListBL.GetAssetTechnicianMappingTime(executionContext.GetSiteId());
            refreshTime = DateTime.UtcNow.AddMinutes(15);
            int siteId = GetSiteId();

            if (updateTime.HasValue && assetMappingLastUpdateTime.HasValue
                && assetMappingLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(null, "No changes in asset module since " + assetMappingLastUpdateTime);
                return;
            }

            assetMappingLastUpdateTime = updateTime;
            ClearAssetMapperCache();
            LoadAssetMapperList();
            log.LogMethodExit();
        }

        /// <summary>
        /// Reloads all asset mapping dictionaries
        /// </summary>
        private void LoadAssetMapperList()
        {
            log.LogMethodEntry();
            int siteId = GetSiteId();

            AssetList assetList = new AssetList(executionContext);
            List<GenericAssetDTO> genericAssetList = new List<GenericAssetDTO>();
            List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>> assetSearchParameters = new List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>>();
            assetSearchParameters.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.ACTIVE_FLAG, "1"));
            assetSearchParameters.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            genericAssetList = assetList.GetAllAssets(assetSearchParameters);

            ConcurrentDictionary<int, int> assetTechnicianDictionary = GetAssetTechinicianMapping();
            ConcurrentDictionary<int, int> assetTypeTechnicianDictionary = GetAssetTypeTechinicianMapping();
            ConcurrentDictionary<int, int> siteTechnicianDictionary = GetSiteTechinicianMapping();

            if (genericAssetList != null)
            {
                foreach (GenericAssetDTO assetDTO in genericAssetList)
                {
                    int assetID = assetDTO.AssetId;
                    int assetTypeID = assetDTO.AssetTypeId;
                    int technicianID = -1;

                    if (assetTechnicianDictionary.ContainsKey(assetID)) // asset level mapping
                    {
                        assetTechnicianDictionary.TryGetValue(assetID, out technicianID);
                        assetMapperDictionary.TryAdd(assetID, technicianID);
                    } else if (assetTypeTechnicianDictionary.ContainsKey(assetTypeID)) // asset type level mapping
                    {
                        assetTypeTechnicianDictionary.TryGetValue(assetTypeID, out technicianID);
                        assetMapperDictionary.TryAdd(assetID, technicianID);
                    } else if(siteTechnicianDictionary.ContainsKey(siteId)) // site level mapping
                    {
                        siteTechnicianDictionary.TryGetValue(siteId, out technicianID);
                        assetMapperDictionary.TryAdd(assetID, technicianID);
                    } else
                    {
                        log.Error("No Technician mapping available for asset:"+assetDTO.Name+" ("+assetID+")");
                    }
                }
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the Asset technician mapping dictionary
        /// </summary>
        /// <returns>Returns ConcurrentDictionary</returns>
        private ConcurrentDictionary<int, int> GetAssetTechinicianMapping()
        {
            log.LogMethodEntry();
            ConcurrentDictionary<int, int> assetTechnicianDictionary = new ConcurrentDictionary<int, int>();
            AssetTechnicianMappingListBL assetTechnicianMappingListBL = new AssetTechnicianMappingListBL(executionContext);
            List<KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>(AssetTechnicianMappingDTO.SearchByParameters.IS_PRIMARY, "1"));
            searchParams.Add(new KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>(AssetTechnicianMappingDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchParams.Add(new KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>(AssetTechnicianMappingDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<AssetTechnicianMappingDTO> mappingDTOList = assetTechnicianMappingListBL.GetAllAssetTechnicianMappingList(searchParams);

            if (mappingDTOList != null && mappingDTOList.Any())
            {
                foreach (AssetTechnicianMappingDTO mappingDTO in mappingDTOList)
                {
                    if (!assetTechnicianDictionary.ContainsKey(mappingDTO.AssetId))
                    {
                        assetTechnicianDictionary.TryAdd(mappingDTO.AssetId, mappingDTO.UserId);
                    }
                }
            }

            log.LogMethodExit(assetTechnicianDictionary);
            return assetTechnicianDictionary;
        }

        /// <summary>
        /// Gets Asset Type technician mapping dictionary
        /// </summary>
        /// <returns>Returns ConcurrentDictionary</returns>
        private ConcurrentDictionary<int, int> GetAssetTypeTechinicianMapping()
        {
            log.LogMethodEntry();
            ConcurrentDictionary<int, int> assetTypeTechnicianDictionary = new ConcurrentDictionary<int, int>();
            AssetTechnicianMappingListBL assetTechnicianMappingListBL = new AssetTechnicianMappingListBL(executionContext);
            List<KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>(AssetTechnicianMappingDTO.SearchByParameters.IS_PRIMARY, "1"));
            searchParams.Add(new KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>(AssetTechnicianMappingDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchParams.Add(new KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>(AssetTechnicianMappingDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<AssetTechnicianMappingDTO> mappingDTOList = assetTechnicianMappingListBL.GetAllAssetTypeTechnicianMappingList(searchParams);

            if (mappingDTOList != null && mappingDTOList.Any())
            {
                foreach (AssetTechnicianMappingDTO mappingDTO in mappingDTOList)
                {
                    if (!assetTypeTechnicianDictionary.ContainsKey(mappingDTO.AssetId))
                    {
                        assetTypeTechnicianDictionary.TryAdd(mappingDTO.AssetTypeId, mappingDTO.UserId);
                    }
                }
            }

            log.LogMethodExit(assetTypeTechnicianDictionary);
            return assetTypeTechnicianDictionary;
        }

        /// <summary>
        /// Get Asset Technician Mapping dictionary
        /// </summary>
        /// <returns>Returns Concurrent Dictionary</returns>
        private ConcurrentDictionary<int, int> GetSiteTechinicianMapping()
        {
            log.LogMethodEntry();
            ConcurrentDictionary<int, int> siteTechnicianDictionary = new ConcurrentDictionary<int, int>();
            AssetTechnicianMappingListBL assetTechnicianMappingListBL = new AssetTechnicianMappingListBL(executionContext);
            List<KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>(AssetTechnicianMappingDTO.SearchByParameters.IS_PRIMARY, "1"));
            searchParams.Add(new KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>(AssetTechnicianMappingDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchParams.Add(new KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>(AssetTechnicianMappingDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<AssetTechnicianMappingDTO> mappingDTOList = assetTechnicianMappingListBL.GetAllSiteTechnicianMappingList(searchParams);
            
            if (mappingDTOList != null && mappingDTOList.Any())
            {
                foreach (AssetTechnicianMappingDTO mappingDTO in mappingDTOList)
                {
                    if (!siteTechnicianDictionary.ContainsKey(mappingDTO.AssetId))
                    {
                        siteTechnicianDictionary.TryAdd(executionContext.IsCorporate ? mappingDTO.SiteId : executionContext.GetSiteId(), mappingDTO.UserId);
                    }
                }
            }

            log.LogMethodExit(siteTechnicianDictionary);
            return siteTechnicianDictionary;
        }


        private void ClearAssetMapperCache()
        {
            log.LogMethodEntry();
            assetMapperDictionary.Clear();
            log.LogMethodExit();
        }

        private int GetSiteId()
        {
            log.LogMethodEntry();
            int siteId = -1;
            if (executionContext.GetIsCorporate())
            {
                siteId = executionContext.GetSiteId();
            }
            log.LogMethodExit(siteId);
            return siteId;
        }
	}
	
}
