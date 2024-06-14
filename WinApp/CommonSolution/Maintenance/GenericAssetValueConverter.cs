/********************************************************************************************
 * Project Name - Maintenance
 * Description  - Generic AssetValue Converter
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.80        10-May-2020   Girish Kundar  Modified: REST API Changes merge from WMS  
 ********************************************************************************************/
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Maintenance
{
    public class GenericAssetValueConverter : ValueConverter
    {
        private ExecutionContext executionContext;
        Dictionary<int, GenericAssetDTO> assetIdGenericAssetDTODictionary;
        Dictionary<string, GenericAssetDTO> assetGenericAssetDTODictionary;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public GenericAssetValueConverter(ExecutionContext executionContext)
        {
            this.executionContext = executionContext;
            assetIdGenericAssetDTODictionary = new Dictionary<int, GenericAssetDTO>();
            assetGenericAssetDTODictionary = new Dictionary<string, GenericAssetDTO>();
            List<GenericAssetDTO> assetTypeDTOList = null;
            AssetList assetList = new AssetList(executionContext);
            List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>> searchParameters = new List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>>
            {
                new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.SITE_ID, executionContext.GetSiteId().ToString()),
                new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.ACTIVE_FLAG, "1")
            };
            assetTypeDTOList = assetList.GetAllAssets(searchParameters);
            if (assetTypeDTOList != null && assetTypeDTOList.Count != 0)
            {
                foreach (GenericAssetDTO genericAssetDTO in assetTypeDTOList)
                {
                    assetIdGenericAssetDTODictionary.Add(genericAssetDTO.AssetId, genericAssetDTO);
                    assetGenericAssetDTODictionary.Add(genericAssetDTO.Name.ToUpper(), genericAssetDTO);
                }

            }
        }
        /// <summary>
        /// Converts asset to assetId
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public override object FromString(string stringValue)
        {
            int assetId = -1;
            if (assetGenericAssetDTODictionary != null && assetGenericAssetDTODictionary.ContainsKey(stringValue.ToUpper()))
            {
                assetId = assetGenericAssetDTODictionary[stringValue.ToUpper()].AssetId;
            }
            return assetId;
        }

        /// <summary>
        /// Converts assetId to asset
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string ToString(object value)
        {
            string asset = string.Empty;
            if (assetIdGenericAssetDTODictionary != null && assetIdGenericAssetDTODictionary.ContainsKey(Convert.ToInt32(value)))
            {
                asset = assetIdGenericAssetDTODictionary[Convert.ToInt32(value)].Name;
            }
            return asset;
        }
    }
}
