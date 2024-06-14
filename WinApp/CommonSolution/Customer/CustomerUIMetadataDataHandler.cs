

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// CustomerUIMetadataDataHandler
    /// </summary>
    public class CustomerUIMetadataDataHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
         DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of CustomerDataHandler class
        /// </summary>
        public CustomerUIMetadataDataHandler()
        {
            log.LogMethodEntry();
            dataAccessHandler = new  DataAccessHandler();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// GetCustomerUIMetadata method
        /// </summary>
        /// <returns></returns>
        public List<CustomerFieldStruct> GetCustomerUIMetadata(int siteId, List<Semnox.Core.Utilities.ParafaitDefaultsDTO> parafaitDefaultsDTOList)
        {
            log.LogMethodEntry();
            List<CustomerFieldStruct> customerMetadatList;
            try
            {
                String queryText = @"select ((case when PD.default_value_name like '%[_]name' then 100 else 200 end)+ROW_NUMBER() over (order by pd.default_value_id)) as fieldOrder, 
                                        PD.default_value_name as fieldName, 
                                        case 
                                        when PD.default_value_name in ('ADDRESS_TYPE','TITLE', 'GENDER','STATE','COUNTRY', 'OPT_IN_PROMOTIONS_MODE','CUSTOMERTYPE') then 'LIST' 
                                        when PD.default_value_name in ('VERIFIED', 'TEAMUSER','RIGHTHANDED','TERMS_AND_CONDITIONS' ,'OPT_IN_PROMOTIONS', 'DEFAULT_ADDRESS') then 'FLAG' 
                                        when PD.default_value_name in ('BIRTH_DATE', 'ANNIVERSARY', 'LASTLOGINTIME') then 'DATE'  
                                        when PD.default_value_name = CA.Name then CA.Type else 'TEXT' 
                                        end as fieldType,
                                        (SELECT STUFF((SELECT '|' + Value
                                        FROM CustomAttributeValueList CAV
                                        where CA.CustomAttributeId=CAV.CustomAttributeId
                                        and (ca.site_id = @site_id or @site_id = -1)
                                        FOR XML PATH('')), 1, 1, '')) as fieldValues,
                                        CA.CustomAttributeId
                                        from parafait_defaults PD
                                        left join defaults_datatype DDT
                                        on PD.datatype_id = DDT.datatype_id and (ddt.site_id = @site_id or @site_id = -1)
                                        left join CustomAttributes CA
                                        on PD.default_value_name = CA.Name and (ca.site_id = @site_id or @site_id = -1)
                                        where PD.active_flag='Y' 
                                        and PD.screen_group='Customer'
                                        and DDT.datatype='CustomFieldDisplayOptions'
                                        and PD.default_value_name NOT IN ('DOWNLOADBATCHID', 'CUSTOMER_PHOTO','LASTLOGINTIME','PASSWORD')
                                        and (pd.site_id = @site_id or @site_id = -1)
                                        order by fieldOrder";


                DataTable customerFieldsDataTable = new DataTable();
                List<SqlParameter> paramterList = new List<SqlParameter>();
                paramterList.Add(new SqlParameter("@site_id", siteId));
                customerFieldsDataTable = dataAccessHandler.executeSelectQuery(queryText, paramterList.ToArray());

                CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(ExecutionContext.GetExecutionContext());
                customerMetadatList = new List<CustomerFieldStruct>();
                int fieldOrder = 0;
                for (int i = 0; i < customerFieldsDataTable.Rows.Count; i++)
                {
                    // Field Name
                    String fieldName = customerFieldsDataTable.Rows[i]["fieldName"].ToString();
					String fieldCaption = "";
                    string validationType = "O";

                    if (parafaitDefaultsDTOList.Where(x => x.DefaultValueName == fieldName).FirstOrDefault() != null)
                            validationType = parafaitDefaultsDTOList.Where(x => x.DefaultValueName == fieldName).FirstOrDefault().DefaultValue;
                        if (validationType.CompareTo("") == 0)
                            validationType = "O";

                    if (validationType.CompareTo("N") != 0)
                    {
                        // Field Values
                        List<String> fieldValuesList = null;
                        if (Convert.IsDBNull(customerFieldsDataTable.Rows[i]["fieldValues"]))
                        {
                            if (fieldName.CompareTo("TITLE") == 0) // NOTE: this can be got from lookup CUSTOMER_TITLES
                                fieldValuesList = new List<String> { "Mr.", "Mrs.", "Ms." };
                            else if (fieldName.CompareTo("CUSTOMERTYPE") == 0)
                                fieldValuesList = new List<String> { "REGISTERED", "UNREGISTERED"};
                            else if (fieldName.CompareTo("GENDER") == 0)
                                fieldValuesList = new List<String> { "Male", "Female", "Not Set" };
                            else if (fieldName.CompareTo("OPT_IN_PROMOTIONS_MODE") == 0)
                            {
                                LookupValuesList lookUpList = new LookupValuesList(null);
                                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookUpValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                                lookUpValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "PROMOTION_MODES"));
                                List<LookupValuesDTO> lookUpValuesList = lookUpList.GetAllLookupValues(lookUpValuesSearchParams);
                                if ((lookUpValuesList != null) && (lookUpValuesList.Count > 0))
                                    fieldValuesList = lookUpValuesList.Select(value => value.LookupValue).ToList();
                            }
                        }
                        else
                        {
                            String customerFieldValues = customerFieldsDataTable.Rows[i]["fieldValues"].ToString();
                            String[] values = customerFieldValues.Split('|');
                            fieldValuesList = new List<String>(values);
                        }

                        // Field Validation


                        // Field Length
                        string fieldLength = "999";
                        if (fieldName.CompareTo("_PHONE") == 0)
                        {
                            if (parafaitDefaultsDTOList.Where(x => x.DefaultValueName == "CUSTOMER_PHONE_NUMBER_WIDTH").FirstOrDefault() != null)
                                fieldLength = parafaitDefaultsDTOList.Where(x => x.DefaultValueName == "CUSTOMER_PHONE_NUMBER_WIDTH").FirstOrDefault().DefaultValue;
                        }
                        else if (fieldName.CompareTo("USERNAME") == 0)
                        {
                            if (parafaitDefaultsDTOList.Where(x => x.DefaultValueName == "CUSTOMER_USERNAME_LENGTH").FirstOrDefault() != null)
                                fieldLength = parafaitDefaultsDTOList.Where(x => x.DefaultValueName == "CUSTOMER_USERNAME_LENGTH").FirstOrDefault().DefaultValue;
                        }
						else if(fieldName.CompareTo("OPT_IN_PROMOTIONS") == 0 || fieldName.CompareTo("OPT_IN_PROMOTIONS_MODE") == 0 || fieldName.CompareTo("TERMS_AND_CONDITIONS") == 0)
						{
							int messageId = -1;
							if (fieldName.CompareTo("OPT_IN_PROMOTIONS") == 0)
								messageId = 1739;
							else if (fieldName  == "OPT_IN_PROMOTIONS_MODE")
								messageId = 1740;
							else if (fieldName.CompareTo("TERMS_AND_CONDITIONS") == 0)
								messageId = 1741;

							fieldCaption = MessageContainerList.GetMessage(ExecutionContext.GetExecutionContext(), messageId);
						}

						// int fieldOrder = i;// ((Convert.ToInt32(customerFieldsDataTable.Rows[i]["fieldOrder"]) < 200) ? (0) : (i));
						String fieldType = customerFieldsDataTable.Rows[i]["fieldType"].ToString();
                        int customAttrId = (Convert.IsDBNull(customerFieldsDataTable.Rows[i]["CustomAttributeId"])) ? (-1) : Convert.ToInt32(customerFieldsDataTable.Rows[i]["CustomAttributeId"]);

                        CustomAttributesDTO customAttributesDTO = new CustomAttributesDTO();
                        if (customAttrId != -1)
                        {
                            List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>>();
                            searchParams.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.APPLICABILITY, Applicability.CUSTOMER.ToString()));
                            searchParams.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.CUSTOM_ATTRIBUTE_ID, customAttrId.ToString()));
                            searchParams.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                            List<CustomAttributesDTO> customAttributsDTOList = customAttributesListBL.GetCustomAttributesDTOList(searchParams, true, true);
                            if ((customAttributsDTOList != null) && (customAttributsDTOList.Count > 0))
                                customAttributesDTO = customAttributsDTOList[0];
                        }

                        customerMetadatList.Add(new CustomerFieldStruct(fieldOrder++, fieldName, fieldValuesList, fieldType, validationType, fieldLength, ((customAttrId == -1) ? (0) : (1)), customAttrId, customAttributesDTO, fieldCaption));
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex.Message);
                throw;
            }
            log.LogMethodExit(null);
            return customerMetadatList;
        }

        /// <summary>
        /// GetCustomerUIMetadata method
        /// </summary>
        /// <returns></returns>
        public List<CustomerUIMetadataDTO> GetCustomerUIMetadataDTOList(int siteId, List<Semnox.Core.Utilities.ParafaitDefaultsDTO> parafaitDefaultsDTOList)
        {
            log.LogMethodEntry();
            List<CustomerUIMetadataDTO> customerMetadatList;
            try
            {
                String queryText = @"select ((case when PD.default_value_name like '%[_]name' then 100 else 200 end)+ROW_NUMBER() over (order by pd.default_value_id)) as fieldOrder, 
                                        PD.default_value_name as fieldName, 
                                        case 
                                        when PD.default_value_name in ('ADDRESS_TYPE','TITLE', 'GENDER','STATE','COUNTRY', 'OPT_IN_PROMOTIONS_MODE') then 'LIST' 
                                        when PD.default_value_name in ('VERIFIED', 'TEAMUSER','RIGHTHANDED','TERMS_AND_CONDITIONS' ,'OPT_IN_PROMOTIONS', 'DEFAULT_ADDRESS') then 'FLAG' 
                                        when PD.default_value_name in ('BIRTH_DATE', 'ANNIVERSARY', 'LASTLOGINTIME') then 'DATE'  
                                        when PD.default_value_name = CA.Name then CA.Type else 'TEXT' 
                                        end as fieldType,
                                        (SELECT STUFF((SELECT '|' + Value
                                        FROM CustomAttributeValueList CAV
                                        where CA.CustomAttributeId=CAV.CustomAttributeId
                                        and (ca.site_id = @site_id or @site_id = -1)
                                        FOR XML PATH('')), 1, 1, '')) as fieldValues,
                                        CA.CustomAttributeId
                                        from parafait_defaults PD
                                        left join defaults_datatype DDT
                                        on PD.datatype_id = DDT.datatype_id and (ddt.site_id = @site_id or @site_id = -1)
                                        left join CustomAttributes CA
                                        on PD.default_value_name = CA.Name and (ca.site_id = @site_id or @site_id = -1)
                                        where PD.active_flag='Y' 
                                        and PD.screen_group='Customer'
                                        and DDT.datatype='CustomFieldDisplayOptions'
                                        and PD.default_value_name NOT IN ('DOWNLOADBATCHID', 'CUSTOMER_PHOTO','LASTLOGINTIME','PASSWORD')
                                        and (pd.site_id = @site_id or @site_id = -1)
                                        order by fieldOrder";


                DataTable customerFieldsDataTable = new DataTable();
                List<SqlParameter> paramterList = new List<SqlParameter>();
                paramterList.Add(new SqlParameter("@site_id", siteId));
                customerFieldsDataTable = dataAccessHandler.executeSelectQuery(queryText, paramterList.ToArray());

                CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(ExecutionContext.GetExecutionContext());
                customerMetadatList = new List<CustomerUIMetadataDTO>();
                int fieldOrder = 0;
                for (int i = 0; i < customerFieldsDataTable.Rows.Count; i++)
                {
                    // Field Name
                    String fieldName = customerFieldsDataTable.Rows[i]["fieldName"].ToString();
                    String fieldCaption = "";
                    string validationType = "O";

                    if (parafaitDefaultsDTOList.Where(x => x.DefaultValueName == fieldName).FirstOrDefault() != null)
                        validationType = parafaitDefaultsDTOList.Where(x => x.DefaultValueName == fieldName).FirstOrDefault().DefaultValue;
                    if (validationType.CompareTo("") == 0)
                        validationType = "O";

                    if (validationType.CompareTo("N") != 0)
                    {
                        // Field Values
                        List<String> fieldValuesList = null;
                        if (Convert.IsDBNull(customerFieldsDataTable.Rows[i]["fieldValues"]))
                        {
                            if (fieldName.CompareTo("TITLE") == 0) // NOTE: this can be got from lookup CUSTOMER_TITLES
                                fieldValuesList = new List<String> { "Mr.", "Mrs.", "Ms." };
                            else if (fieldName.CompareTo("GENDER") == 0)
                                fieldValuesList = new List<String> { "Male", "Female", "Not Set" };
                            else if (fieldName.CompareTo("OPT_IN_PROMOTIONS_MODE") == 0)
                            {
                                LookupValuesList lookUpList = new LookupValuesList(null);
                                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookUpValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                                lookUpValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "PROMOTION_MODES"));
                                List<LookupValuesDTO> lookUpValuesList = lookUpList.GetAllLookupValues(lookUpValuesSearchParams);
                                if ((lookUpValuesList != null) && (lookUpValuesList.Count > 0))
                                    fieldValuesList = lookUpValuesList.Select(value => value.LookupValue).ToList();
                            }
                        }
                        else
                        {
                            String customerFieldValues = customerFieldsDataTable.Rows[i]["fieldValues"].ToString();
                            String[] values = customerFieldValues.Split('|');
                            fieldValuesList = new List<String>(values);
                        }

                        // Field Validation


                        // Field Length
                        string fieldLength = "999";
                        if (fieldName.CompareTo("_PHONE") == 0)
                        {
                            if (parafaitDefaultsDTOList.Where(x => x.DefaultValueName == "CUSTOMER_PHONE_NUMBER_WIDTH").FirstOrDefault() != null)
                                fieldLength = parafaitDefaultsDTOList.Where(x => x.DefaultValueName == "CUSTOMER_PHONE_NUMBER_WIDTH").FirstOrDefault().DefaultValue;
                        }
                        else if (fieldName.CompareTo("USERNAME") == 0)
                        {
                            if (parafaitDefaultsDTOList.Where(x => x.DefaultValueName == "CUSTOMER_USERNAME_LENGTH").FirstOrDefault() != null)
                                fieldLength = parafaitDefaultsDTOList.Where(x => x.DefaultValueName == "CUSTOMER_USERNAME_LENGTH").FirstOrDefault().DefaultValue;
                        }
                        else if (fieldName.CompareTo("OPT_IN_PROMOTIONS") == 0 || fieldName.CompareTo("OPT_IN_PROMOTIONS_MODE") == 0 || fieldName.CompareTo("TERMS_AND_CONDITIONS") == 0)
                        {
                            int messageId = -1;
                            if (fieldName.CompareTo("OPT_IN_PROMOTIONS") == 0)
                                messageId = 1739;
                            else if (fieldName == "OPT_IN_PROMOTIONS_MODE")
                                messageId = 1740;
                            else if (fieldName.CompareTo("TERMS_AND_CONDITIONS") == 0)
                                messageId = 1741;

                            fieldCaption = MessageContainerList.GetMessage(ExecutionContext.GetExecutionContext(), messageId);
                        }

                        // int fieldOrder = i;// ((Convert.ToInt32(customerFieldsDataTable.Rows[i]["fieldOrder"]) < 200) ? (0) : (i));
                        String fieldType = customerFieldsDataTable.Rows[i]["fieldType"].ToString();
                        int customAttrId = (Convert.IsDBNull(customerFieldsDataTable.Rows[i]["CustomAttributeId"])) ? (-1) : Convert.ToInt32(customerFieldsDataTable.Rows[i]["CustomAttributeId"]);

                        CustomAttributesDTO customAttributesDTO = new CustomAttributesDTO();
                        if (customAttrId != -1)
                        {
                            List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>>();
                            searchParams.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.APPLICABILITY, Applicability.CUSTOMER.ToString()));
                            searchParams.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.CUSTOM_ATTRIBUTE_ID, customAttrId.ToString()));
                            searchParams.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                            List<CustomAttributesDTO> customAttributsDTOList = customAttributesListBL.GetCustomAttributesDTOList(searchParams, true, true);
                            if ((customAttributsDTOList != null) && (customAttributsDTOList.Count > 0))
                                customAttributesDTO = customAttributsDTOList[0];
                        }

                        customerMetadatList.Add(new CustomerUIMetadataDTO(fieldOrder++, fieldName, fieldValuesList, fieldType, validationType, fieldLength, ((customAttrId == -1) ? (0) : (1)), customAttrId, customAttributesDTO, fieldCaption));
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex.Message);
                throw;
            }
            log.LogMethodExit(null);
            return customerMetadatList;
        }

        internal DateTime? GetParafaitDefaultModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate
                            FROM (
                            select max(LastUpdatedDate) LastUpdatedDate from parafait_defaults WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdatedDate) LastUpdatedDate from ParafaitOptionValues WHERE (site_id = @siteId or @siteId = -1)) a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, null);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
