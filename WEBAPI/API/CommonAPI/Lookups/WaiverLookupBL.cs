//********************************************************************************************
// * Project Name - WaiverLookupBL
// * Description  - 
// *  
// **************
// **Version Log
// **************
// *Version     Date          Modified By    Remarks          
// *********************************************************************************************
// *2.80        05-Dec-2019   Indrajeet Kumar    Created.
// *2.80        30-Apr-2020   Mushahid Faizan    Added new lookups.
// ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using Semnox.Parafait.Customer;
using System.Linq;

namespace Semnox.CommonAPI.Lookups
{
    public class WaiverLookupBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string entityName;
        private ExecutionContext executionContext;
        DataAccessHandler dataAccessHandler = new DataAccessHandler();

        private CommonLookupDTO lookupDataObject;
        public List<LookupValuesDTO> lookupValuesDTOList;
        private CommonLookupsDTO lookupDTO;

        public WaiverLookupBL(string entityName, ExecutionContext executionContext)
        {
            log.LogMethodEntry(entityName, executionContext);
            this.entityName = entityName;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public enum WaiverEntityNameLookup
        {
            SETRELATIONSHIPTYPE,
            SETCUSTOMATTRIBUTES
        }

        public List<CommonLookupsDTO> GetLookUpMasterDataList()
        {
            try
            {
                log.LogMethodEntry();
                List<CommonLookupsDTO> lookups = new List<CommonLookupsDTO>();
                string dropdownNames = string.Empty;
                string[] dropdowns = null;

                WaiverEntityNameLookup waiverEntityNameLookup = (WaiverEntityNameLookup)Enum.Parse(typeof(WaiverEntityNameLookup), entityName.ToUpper().ToString());
                switch (waiverEntityNameLookup)
                {
                    case WaiverEntityNameLookup.SETRELATIONSHIPTYPE:
                        dropdownNames = "RELATIONSHIPTYPE";
                        break; 
                    case WaiverEntityNameLookup.SETCUSTOMATTRIBUTES:
                        dropdownNames = "CUSTOMATTRIBUTES";
                        break;
                }
                dropdowns = dropdownNames.Split(',');
                foreach (string dropdownName in dropdowns)
                {
                    lookupDTO = new CommonLookupsDTO
                    {
                        Items = new List<CommonLookupDTO>(),
                        DropdownName = dropdownName
                    };

                    /// The Below if condition is for RELATIONSHIPTYPE dropdown 
                    if (dropdownName.ToUpper().ToString() == "RELATIONSHIPTYPE")
                    {
                        LoadDefaultValue("<SELECT>");
                        CustomerRelationshipTypeListBL customerRelationshipTypeListBL = new CustomerRelationshipTypeListBL(executionContext);
                        List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>> searchParameter = new List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>>();
                        searchParameter.Add(new KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>(CustomerRelationshipTypeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<CustomerRelationshipTypeDTO> customerRelationshipTypeDTOList = customerRelationshipTypeListBL.GetCustomerRelationshipTypeDTOList(searchParameter);
                        if (customerRelationshipTypeDTOList != null && customerRelationshipTypeDTOList.Any())
                        {
                            foreach (var customerRelationshipType in customerRelationshipTypeDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(customerRelationshipType.Id), customerRelationshipType.Name);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "CUSTOMATTRIBUTES")
                    {
                        LoadDefaultValue("<SELECT>");
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "CUSTOM_ATTRIBUTES_IN_WAIVER"));
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValue), lookupValuesDTO.Description);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    lookups.Add(lookupDTO);
                }
                return lookups;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Loads default value to the lookupDto
        /// </summary>
        /// <param name="defaultValue">default value of Type string</param>
        private void LoadDefaultValue(string defaultValue)
        {
            List<KeyValuePair<string, string>> selectKey = new List<KeyValuePair<string, string>>();
            selectKey.Add(new KeyValuePair<string, string>("-1", defaultValue));
            foreach (var select in selectKey)
            {
                CommonLookupDTO lookupDataObject = new CommonLookupDTO(Convert.ToString(select.Key), select.Value);
                lookupDTO.Items.Add(lookupDataObject);
            }
        }
    }
}