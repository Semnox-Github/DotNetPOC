/********************************************************************************************
 * Project Name - GenericAsset                                                                         
 * Description  - Bulk Upload Mapper AssetTaxDTO  Class 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70       18-Sept-2019   Rakesh Kumar   Created       
 *2.80       10-May-2020   Girish Kundar   Modified: REST API Changes merge from WMS  
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
    class AssetTaxValueConverter:ValueConverter
    {
        private ExecutionContext executionContext;
        Dictionary<int, AssetTaxDTO> taxIdAssetTaxDTODictionary;
        Dictionary<string, AssetTaxDTO> taxNameAssetTaxDTOictionary;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public AssetTaxValueConverter(ExecutionContext executionContext)
        {
            this.executionContext = executionContext;
            taxIdAssetTaxDTODictionary = new Dictionary<int, AssetTaxDTO>();
            taxNameAssetTaxDTOictionary = new Dictionary<string, AssetTaxDTO>();
            List<AssetTaxDTO> assetTaxDTOList = null;
            List<KeyValuePair<AssetTaxDTO.SearchByAssetTaxParameters, string>> searchParameters = new List<KeyValuePair<AssetTaxDTO.SearchByAssetTaxParameters,string>>();
            searchParameters.Add(new KeyValuePair<AssetTaxDTO.SearchByAssetTaxParameters, string>(AssetTaxDTO.SearchByAssetTaxParameters.ACTIVE_FLAG,"Y"));
            assetTaxDTOList = new AssetTaxDataHandler().GetAssetTaxList(searchParameters);  
            if (assetTaxDTOList != null && assetTaxDTOList.Count > 0)
            {
                foreach (AssetTaxDTO assetTaxDTO in assetTaxDTOList)
                {
                    taxIdAssetTaxDTODictionary.Add(assetTaxDTO.TaxId,assetTaxDTO);
                    taxNameAssetTaxDTOictionary.Add(assetTaxDTO.TaxName.ToUpper(),assetTaxDTO);
                }
            }
        }
        /// <summary>
        /// Converts taxName to taxId
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public override object FromString(string stringValue)
        {
            int taxId = -1;
            if (taxNameAssetTaxDTOictionary != null && taxNameAssetTaxDTOictionary.ContainsKey(stringValue.ToUpper()))
            {
                taxId = taxNameAssetTaxDTOictionary[stringValue.ToUpper()].TaxId;
            }
            return taxId;
        }

        /// <summary>
        /// Converts taxId to taxName
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string ToString(object value)
        {
            string taxName = string.Empty;
            if (taxIdAssetTaxDTODictionary != null && taxIdAssetTaxDTODictionary.ContainsKey(Convert.ToInt32(value)))
            {
                taxName = taxIdAssetTaxDTODictionary[Convert.ToInt32(value)].TaxName;
                return taxName;
            }
            else
            {
                return "None";   
            }
            
        }
    }
}
