/********************************************************************************************
 * Project Name - Maintenance
 * Description  -Lookup Value Converter
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
    public class LookupValueConverter : ValueConverter
    {
        private ExecutionContext executionContext;
        Dictionary<int, LookupValuesDTO> statausIdLookupValuesDTODictionary;
        Dictionary<string, LookupValuesDTO> statusLookupValuesDTODictionary;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public LookupValueConverter(ExecutionContext executionContext, string lookupName)
        {
            this.executionContext = executionContext;
            statausIdLookupValuesDTODictionary = new Dictionary<int, LookupValuesDTO>();
            statusLookupValuesDTODictionary = new Dictionary<string, LookupValuesDTO>();
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, lookupName));
            List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
            if (lookupValuesDTOList != null && lookupValuesDTOList.Count != 0)
            {
                foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                {
                    if (statausIdLookupValuesDTODictionary.ContainsKey(lookupValuesDTO.LookupValueId) == false)
                    {
                        statausIdLookupValuesDTODictionary.Add(lookupValuesDTO.LookupValueId, lookupValuesDTO);
                    }
                    if (statusLookupValuesDTODictionary.ContainsKey(lookupValuesDTO.Description.ToUpper()) == false)
                    {
                        statusLookupValuesDTODictionary.Add(lookupValuesDTO.Description.ToUpper(), lookupValuesDTO);
                    }
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
            if (statusLookupValuesDTODictionary != null && statusLookupValuesDTODictionary.ContainsKey(stringValue.ToUpper()))
            {
                assetTypeId = statusLookupValuesDTODictionary[stringValue.ToUpper()].LookupValueId;
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
            if (statausIdLookupValuesDTODictionary != null && statausIdLookupValuesDTODictionary.ContainsKey(Convert.ToInt32(value)))
            {
                assetType = statausIdLookupValuesDTODictionary[Convert.ToInt32(value)].Description;
            }
            return assetType;
        }
    }
}
