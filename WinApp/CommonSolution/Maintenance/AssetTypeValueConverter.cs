/********************************************************************************************
 * Project Name - GenericAsset                                                                         
 * Description  - Bulk Upload Mapper AssetTypeDTO  Class 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.80       18-Sept-2019   Rakesh Kumar   Created    
 *2.80       10-May-2020    Girish Kundar  Modified: REST API Changes merge from WMS  
 ********************************************************************************************/
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.GenericUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Semnox.Parafait.Maintenance
{
    public class AssetTypeValueConverter:ValueConverter
    {
        private ExecutionContext executionContext;
        Dictionary<int, AssetTypeDTO> assetTypeIdAssetTypeDTODictionary;
        Dictionary<string, AssetTypeDTO> assetTypeAssetTypeDTOictionary;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public AssetTypeValueConverter(ExecutionContext executionContext)
        {
            this.executionContext = executionContext;
            assetTypeIdAssetTypeDTODictionary = new Dictionary<int, AssetTypeDTO>();
            assetTypeAssetTypeDTOictionary = new Dictionary<string, AssetTypeDTO>();
            List<AssetTypeDTO> assetTypeDTOList = null;
            AssetTypeList assetTypeList = new AssetTypeList(executionContext);
            List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>> searchParameters = new List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>>();
            searchParameters.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            assetTypeDTOList = assetTypeList.GetAllAssetTypes(searchParameters);
            if (assetTypeDTOList != null && assetTypeDTOList.Count > 0)
            {
                foreach (AssetTypeDTO assetTypeDTO in assetTypeDTOList)
                {
                    assetTypeIdAssetTypeDTODictionary.Add(assetTypeDTO.AssetTypeId, assetTypeDTO);
                    assetTypeAssetTypeDTOictionary.Add(assetTypeDTO.Name.ToUpper(), assetTypeDTO);
                }

            }
        }
        /// <summary>
        /// Converts assetType to assetTypeId
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public override object FromString(string stringValue)
        {
            int assetTypeId = -1;
            if (assetTypeAssetTypeDTOictionary != null && assetTypeAssetTypeDTOictionary.ContainsKey(stringValue.ToUpper()))
            {
                assetTypeId = assetTypeAssetTypeDTOictionary[stringValue.ToUpper()].AssetTypeId;
            }
            return assetTypeId;
        }

        /// <summary>
        /// Converts assetTypeId to assetType
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string ToString(object value)
        {
            string assetType = string.Empty;
            if (assetTypeIdAssetTypeDTODictionary != null && assetTypeIdAssetTypeDTODictionary.ContainsKey(Convert.ToInt32(value)))
            {
                assetType = assetTypeIdAssetTypeDTODictionary[Convert.ToInt32(value)].Name;
            }
            return assetType;
        }
    }
    
}
