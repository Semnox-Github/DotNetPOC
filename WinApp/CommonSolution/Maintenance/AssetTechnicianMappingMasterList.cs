
/********************************************************************************************
 * Project Name - Maintenance
 * Description  - Master list to hold AssetTechnicianMapping
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
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// Master list of all the assets and technician mapping in the system. 
    /// </summary>
    public static class AssetTechnicianMappingMasterList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static ConcurrentDictionary<int, AssetTechnicianMappingContainer> assetTechnicianMappingContainerDictionary = new ConcurrentDictionary<int, AssetTechnicianMappingContainer>();
        private static readonly object locker = new object();
        
        private static AssetTechnicianMappingContainer GetAssetTechnicianMappingContainer(ExecutionContext executionContext)
		{
            log.LogMethodEntry(executionContext);

            if (assetTechnicianMappingContainerDictionary.ContainsKey(executionContext.GetSiteId()) == false)
            {
                assetTechnicianMappingContainerDictionary[executionContext.GetSiteId()] = new AssetTechnicianMappingContainer(executionContext);

            }
            AssetTechnicianMappingContainer result = assetTechnicianMappingContainerDictionary[executionContext.GetSiteId()];
            log.LogMethodExit(result);
            return result;
        }
		
		
        /// <summary>
        /// Returns the technician id matching the asset id 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public static int GetAssetTechnician(ExecutionContext executionContext, int assetId)
        {
            log.LogMethodEntry(executionContext, assetId);
            int result;
            lock (locker)
            {
                AssetTechnicianMappingContainer assetTechnicianMappingContainer = GetAssetTechnicianMappingContainer(executionContext);

                result = assetTechnicianMappingContainer.GetAssetTechnician(assetId);
            }
            log.LogMethodExit(result);
            return result;
        }

    }
}
