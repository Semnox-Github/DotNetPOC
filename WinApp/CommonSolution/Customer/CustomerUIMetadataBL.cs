/********************************************************************************************
 * Project Name - Customers
 * Description  - Gives default UI fields
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.140.0    14-Sep-2021       Prajwal S      Modified : Added new UI fields to be shown.
 ********************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// CustomerUIMetadataBL class
    /// </summary>
    public class CustomerUIMetadataBL
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// default constructor
        /// </summary>
        public CustomerUIMetadataBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();

        }
        /// <summary>
        /// GetCustomerUIMetadata
        /// </summary>
        /// <returns>list of CustomerFieldStruct</returns>
        public List<CustomerFieldStruct> GetCustomerUIMetadata(int siteId)
        {
            log.LogMethodEntry(siteId);

            ParafaitDefaultsListBL parafaitDefaultsBL = new ParafaitDefaultsListBL(executionContext);
            List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.SCREEN_GROUP, "Customer"));
            searchParams.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.SITE_ID, siteId.ToString()));
            List<ParafaitDefaultsDTO> parafaitDefaultsDTOList = parafaitDefaultsBL.GetParafaitDefaultsDTOList(searchParams);

            CustomerUIMetadataDataHandler customerUIMetadataDataHandler = new CustomerUIMetadataDataHandler();
            log.LogMethodExit(null);

            return SortCustomerUIMetaData(customerUIMetadataDataHandler.GetCustomerUIMetadata(siteId, parafaitDefaultsDTOList), siteId);
        }


		/// <summary>
		/// SortCustomerUIMetaData
		/// </summary>
		/// <returns></returns>

		private List<CustomerFieldStruct> SortCustomerUIMetaData(List<CustomerFieldStruct> customerMetadatList, int siteId)
		{
			log.LogMethodEntry();
			List<CustomerFieldStruct> customerMetaDataListSorted = new List<CustomerFieldStruct>();

			try
			{

				if (customerMetadatList.Where(x => x.CustomerFieldName == "BIRTH_DATE").Count() > 0)
				{
					ParafaitDefaultsListBL parafaitDefaultsBL = new ParafaitDefaultsListBL(executionContext);
					List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>>();
					searchParams.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.SCREEN_GROUP, "Formats"));
					searchParams.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.SITE_ID, siteId.ToString()));
					List<ParafaitDefaultsDTO> parafaitDefaultsDTOList = parafaitDefaultsBL.GetParafaitDefaultsDTOList(searchParams);

					if (parafaitDefaultsDTOList != null && parafaitDefaultsDTOList.Where(x => x.DefaultValueName == "IGNORE_CUSTOMER_BIRTH_YEAR").FirstOrDefault() != null)
					{
						if (parafaitDefaultsDTOList.Where(x => x.DefaultValueName == "IGNORE_CUSTOMER_BIRTH_YEAR").FirstOrDefault().DefaultValue == "Y")
							customerMetadatList.Where(x => x.CustomerFieldName == "BIRTH_DATE").FirstOrDefault().DisplayFormat = "DD-MMM";
					}
				}

				List<KeyValuePair<string, string>> ListFormFields = new List<KeyValuePair<string, string>>
				{
						new KeyValuePair<string, string>("title", "Title|ProfileDTO.Title"),
						new KeyValuePair<string, string>("customer_name", "First Name|ProfileDTO.FirstName"),
						new KeyValuePair<string, string>("middle_name", "Middle Name|ProfileDTO.MiddleName"),
						new KeyValuePair<string, string>("last_name", "Last Name|ProfileDTO.LastName"),

						new KeyValuePair<string, string>("gender", "Gender|ProfileDTO.Gender"),
						new KeyValuePair<string, string>("birth_date", "Birth Date|ProfileDTO.DateOfBirth"),
						new KeyValuePair<string, string>("anniversary", "Anniversary|ProfileDTO.Anniversary"),

						new KeyValuePair<string, string>("CONTACT_PHONE", "Phone|PhoneNumber"),
						new KeyValuePair<string, string>("CONTACT_PHONE1", "Alternate Phone|SecondaryPhoneNumber"),

						new KeyValuePair<string, string>("address1", "Address 1|ProfileDTO.AddressDTOList[0].Line1"),
						new KeyValuePair<string, string>("address2", "Address 2|ProfileDTO.AddressDTOList[0].Line2"),
						new KeyValuePair<string, string>("address3", "Address 3|ProfileDTO.AddressDTOList[0].Line3"),
						new KeyValuePair<string, string>("city", "city|ProfileDTO.AddressDTOList[0].City"),
						new KeyValuePair<string, string>("state", "state|ProfileDTO.AddressDTOList[0].StateId"),
						new KeyValuePair<string, string>("country", "country|ProfileDTO.AddressDTOList[0].CountryId"),
						new KeyValuePair<string, string>("pin", "Post Code|ProfileDTO.AddressDTOList[0].PostalCode"),

						new KeyValuePair<string, string>("UNIQUE_ID", "Unique Identifier|ProfileDTO.UniqueIdentifier"),
						new KeyValuePair<string, string>("RIGHTHANDED", "Right Handed|ProfileDTO.RightHanded"),
						new KeyValuePair<string, string>("TEAMUSER", "Team User|ProfileDTO.TeamUser"),
						new KeyValuePair<string, string>("FBUSERID", "FaceBook UserId|FBUserId"),
						new KeyValuePair<string, string>("FBACCESSTOKEN", "FaceBook Token|FBAccessToken"),
						new KeyValuePair<string, string>("TWACCESSTOKEN", "Twitter Token|TWAccessToken"),
						new KeyValuePair<string, string>("TWACCESSSECRET", "Twitter Secret Key|TWAccessSecret"),
						new KeyValuePair<string, string>("WECHAT_ACCESS_TOKEN", "WeChat Access Token|WeChatAccessToken"),

						new KeyValuePair<string, string>("COMPANY", "Company|ProfileDTO.Company"),
						new KeyValuePair<string, string>("DESIGNATION", "Designation|ProfileDTO.Designation"),
						new KeyValuePair<string, string>("taxcode", "Tax Code|ProfileDTO.TaxCode"),
						new KeyValuePair<string, string>("notes", "Notes|ProfileDTO.Notes"),

						new KeyValuePair<string, string>("EMAIL", "Email|Email"),
						new KeyValuePair<string, string>("username", "username|ProfileDTO.UserName"),
						new KeyValuePair<string, string>("Password", "Password|ProfileDTO.Password"),

						new KeyValuePair<string, string>("opt_In_Promotions", "Would You like to receive Promotions ?|ProfileDTO.OptInPromotions"),
						new KeyValuePair<string, string>("opt_In_Promotions_Mode", "Promotion Mode|ProfileDTO.OptInPromotionsMode"),
						new KeyValuePair<string, string>("terms_and_conditions", "Click here to agree to the terms and conditions|ProfileDTO.PolicyTermsAccepted"),

				};



				int intRowIndex = -1;
				int ctr = 1;

				foreach (KeyValuePair<string, string> kVPField in ListFormFields)
				{
					intRowIndex = customerMetadatList.FindIndex(x => x.CustomerFieldName.ToLower() == kVPField.Key.ToLower());
					if (intRowIndex != -1 && customerMetadatList[intRowIndex].CustomAttributeFlag == 0)
					{
						customerMetadatList[intRowIndex].EntityFieldName = kVPField.Value.Split('|')[1];

						if(String.IsNullOrWhiteSpace(customerMetadatList[intRowIndex].EntityFieldCaption))
							customerMetadatList[intRowIndex].EntityFieldCaption = kVPField.Value.Split('|')[0];

						customerMetadatList[intRowIndex].CustomerFieldOrder = ctr;
						customerMetaDataListSorted.Add(customerMetadatList[intRowIndex]);
						customerMetadatList.RemoveAt(intRowIndex);
						ctr++;
					}
				}

				foreach (CustomerFieldStruct cfs in customerMetadatList)
				{
					cfs.CustomerFieldOrder = ctr;

					if (string.IsNullOrWhiteSpace(cfs.EntityFieldCaption))
						cfs.EntityFieldCaption = cfs.CustomerFieldName;

					if (string.IsNullOrWhiteSpace(cfs.EntityFieldName))
						cfs.EntityFieldName = cfs.CustomerFieldName;

					customerMetaDataListSorted.Add(cfs);
					ctr++;
				}
			}
			catch (Exception ex)
			{
				log.LogMethodExit(ex.Message);
				throw;
			}
			log.LogMethodExit(null);
			return customerMetaDataListSorted;
		}

        /// <summary>
        /// GetCustomerUIMetadata
        /// </summary>
        /// <returns>list of CustomerFieldStruct</returns>
        public List<CustomerUIMetadataDTO> GetCustomerUIMetadataDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);

            ParafaitDefaultsListBL parafaitDefaultsBL = new ParafaitDefaultsListBL(executionContext);
            List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.SCREEN_GROUP, "Customer"));
            searchParams.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.SITE_ID, siteId.ToString()));
            List<ParafaitDefaultsDTO> parafaitDefaultsDTOList = parafaitDefaultsBL.GetParafaitDefaultsDTOList(searchParams);

            CustomerUIMetadataDataHandler customerUIMetadataDataHandler = new CustomerUIMetadataDataHandler();
            log.LogMethodExit(null);

            return SortCustomerUIMetaDataDTOList(customerUIMetadataDataHandler.GetCustomerUIMetadataDTOList(siteId, parafaitDefaultsDTOList), siteId);
        }


        /// <summary>
        /// SortCustomerUIMetaData
        /// </summary>
        /// <returns></returns>

        private List<CustomerUIMetadataDTO> SortCustomerUIMetaDataDTOList(List<CustomerUIMetadataDTO> customerMetadatList, int siteId)
        {
            log.LogMethodEntry();
            List<CustomerUIMetadataDTO> customerMetaDataListSorted = new List<CustomerUIMetadataDTO>();

            try
            {

                if (customerMetadatList.Where(x => x.CustomerFieldName == "BIRTH_DATE").Count() > 0)
                {
                    ParafaitDefaultsListBL parafaitDefaultsBL = new ParafaitDefaultsListBL(executionContext);
                    List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>>();
                    searchParams.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.SCREEN_GROUP, "Formats"));
                    searchParams.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                    List<ParafaitDefaultsDTO> parafaitDefaultsDTOList = parafaitDefaultsBL.GetParafaitDefaultsDTOList(searchParams);

                    if (parafaitDefaultsDTOList != null && parafaitDefaultsDTOList.Where(x => x.DefaultValueName == "IGNORE_CUSTOMER_BIRTH_YEAR").FirstOrDefault() != null)
                    {
                        if (parafaitDefaultsDTOList.Where(x => x.DefaultValueName == "IGNORE_CUSTOMER_BIRTH_YEAR").FirstOrDefault().DefaultValue == "Y")
                            customerMetadatList.Where(x => x.CustomerFieldName == "BIRTH_DATE").FirstOrDefault().DisplayFormat = "DD-MMM";
                    }
                }

                List<KeyValuePair<string, string>> ListFormFields = new List<KeyValuePair<string, string>>
                {
                        new KeyValuePair<string, string>("title", "Title|ProfileDTO.Title"),
                        new KeyValuePair<string, string>("customer_name", "First Name|ProfileDTO.FirstName"),
                        new KeyValuePair<string, string>("middle_name", "Middle Name|ProfileDTO.MiddleName"),
                        new KeyValuePair<string, string>("last_name", "Last Name|ProfileDTO.LastName"),

                        new KeyValuePair<string, string>("gender", "Gender|ProfileDTO.Gender"),
                        new KeyValuePair<string, string>("birth_date", "Birth Date|ProfileDTO.DateOfBirth"),
                        new KeyValuePair<string, string>("anniversary", "Anniversary|ProfileDTO.Anniversary"),

                        new KeyValuePair<string, string>("CONTACT_PHONE", "Phone|PhoneNumber"),
                        new KeyValuePair<string, string>("CONTACT_PHONE1", "Alternate Phone|SecondaryPhoneNumber"),
                        new KeyValuePair<string, string>("ADDRESS_CONTACT_PHONE", "Contact Phone Number|ProfileDTO.AddressDTOList[0].ContactDTOList[0].Attribute1"),

                        new KeyValuePair<string, string>("address1", "Address 1|ProfileDTO.AddressDTOList[0].Line1"),
                        new KeyValuePair<string, string>("address2", "Address 2|ProfileDTO.AddressDTOList[0].Line2"),
                        new KeyValuePair<string, string>("address3", "Address 3|ProfileDTO.AddressDTOList[0].Line3"),
                        new KeyValuePair<string, string>("city", "city|ProfileDTO.AddressDTOList[0].City"),
                        new KeyValuePair<string, string>("state", "state|ProfileDTO.AddressDTOList[0].StateId"),
                        new KeyValuePair<string, string>("country", "country|ProfileDTO.AddressDTOList[0].CountryId"),
                        new KeyValuePair<string, string>("pin", "Post Code|ProfileDTO.AddressDTOList[0].PostalCode"),
                        new KeyValuePair<string, string>("DEFAULT_ADDRESS", "Default Address|ProfileDTO.AddressDTOList[0].IsDefault"),
                        new KeyValuePair<string, string>("ADDRESS_TYPE", "Address Type|ProfileDTO.AddressDTOList[0].AddressType"),

                        new KeyValuePair<string, string>("UNIQUE_ID", "Unique Identifier|ProfileDTO.UniqueIdentifier"),
                        new KeyValuePair<string, string>("RIGHTHANDED", "Right Handed|ProfileDTO.RightHanded"),
                        new KeyValuePair<string, string>("TEAMUSER", "Team User|ProfileDTO.TeamUser"),
                        new KeyValuePair<string, string>("FBUSERID", "FaceBook UserId|FBUserId"),
                        new KeyValuePair<string, string>("FBACCESSTOKEN", "FaceBook Token|FBAccessToken"),
                        new KeyValuePair<string, string>("TWACCESSTOKEN", "Twitter Token|TWAccessToken"),
                        new KeyValuePair<string, string>("TWACCESSSECRET", "Twitter Secret Key|TWAccessSecret"),
                        new KeyValuePair<string, string>("WECHAT_ACCESS_TOKEN", "WeChat Access Token|WeChatAccessToken"),

                        new KeyValuePair<string, string>("COMPANY", "Company|ProfileDTO.Company"),
                        new KeyValuePair<string, string>("DESIGNATION", "Designation|ProfileDTO.Designation"),
                        new KeyValuePair<string, string>("taxcode", "Tax Code|ProfileDTO.TaxCode"),
                        new KeyValuePair<string, string>("notes", "Notes|ProfileDTO.Notes"),

                        new KeyValuePair<string, string>("EMAIL", "Email|Email"),
                        new KeyValuePair<string, string>("username", "username|ProfileDTO.UserName"),
                        new KeyValuePair<string, string>("Password", "Password|ProfileDTO.Password"),

                        new KeyValuePair<string, string>("opt_In_Promotions", "Would You like to receive Promotions ?|ProfileDTO.OptInPromotions"),
                        new KeyValuePair<string, string>("opt_In_Promotions_Mode", "Promotion Mode|ProfileDTO.OptInPromotionsMode"),
                        new KeyValuePair<string, string>("terms_and_conditions", "Click here to agree to the terms and conditions|ProfileDTO.PolicyTermsAccepted"),

                };



                int intRowIndex = -1;
                int ctr = 1;

                foreach (KeyValuePair<string, string> kVPField in ListFormFields)
                {
                    intRowIndex = customerMetadatList.FindIndex(x => x.CustomerFieldName.ToLower() == kVPField.Key.ToLower());
                    if (intRowIndex != -1 && customerMetadatList[intRowIndex].CustomAttributeFlag == 0)
                    {
                        customerMetadatList[intRowIndex].EntityFieldName = kVPField.Value.Split('|')[1];

                        if (String.IsNullOrWhiteSpace(customerMetadatList[intRowIndex].EntityFieldCaption))
                            customerMetadatList[intRowIndex].EntityFieldCaption = kVPField.Value.Split('|')[0];

                        // message translation
                        if (!string.IsNullOrWhiteSpace(customerMetadatList[intRowIndex].EntityFieldCaption))
                            customerMetadatList[intRowIndex].EntityFieldCaption = MessageContainerList.GetMessage(executionContext, customerMetadatList[intRowIndex].EntityFieldCaption);

                        customerMetadatList[intRowIndex].CustomerFieldOrder = ctr;
                        customerMetaDataListSorted.Add(customerMetadatList[intRowIndex]);
                        customerMetadatList.RemoveAt(intRowIndex);
                        ctr++;
                    }
                }

                foreach (CustomerUIMetadataDTO cfs in customerMetadatList)
                {
                    cfs.CustomerFieldOrder = ctr;

                    if (string.IsNullOrWhiteSpace(cfs.EntityFieldCaption))
                        cfs.EntityFieldCaption = cfs.CustomerFieldName;

                    if (string.IsNullOrWhiteSpace(cfs.EntityFieldName))
                        cfs.EntityFieldName = cfs.CustomerFieldName;

                    customerMetaDataListSorted.Add(cfs);
                    ctr++;
                }
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex.Message);
                throw;
            }
            log.LogMethodExit(null);
            return customerMetaDataListSorted;
        }
        public DateTime? GetCustomerUIMetadataLastUpdateTime(int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId, sqlTransaction);
            CustomerUIMetadataDataHandler customerUIMetadataDataHandler = new CustomerUIMetadataDataHandler();
            DateTime? result = customerUIMetadataDataHandler.GetParafaitDefaultModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
