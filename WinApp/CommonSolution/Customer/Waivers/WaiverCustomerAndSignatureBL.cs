/********************************************************************************************
 * Project Name - WaiverCustomerAndSignatureBL
 * Description  - BL of WaiverCustomerAndSignatureDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By       Remarks          
 *********************************************************************************************
 *2.70.2      03-Dec-2019      Guru S A          Created for waiver phase 2
 *2.100       19-Oct-2020      Guru S A          Enabling minor signature option for waiver
 ********************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Customer.Waivers
{
    public class WaiverCustomerAndSignatureBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Create Customer Content For Waiver
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="waiverCustomerAndSignatureDTO"></param>
        /// <returns></returns>
        public static WaiverCustomerAndSignatureDTO CreateCustomerContentForWaiver(ExecutionContext executionContext, WaiverCustomerAndSignatureDTO waiverCustomerAndSignatureDTO)
        {
            log.LogMethodEntry(waiverCustomerAndSignatureDTO);
            List<LookupValuesDTO> custAttributesInWaiverLookUpValueDTOList = WaiverCustomerUtils.GetWaiverCustomerAttributeLookup(executionContext);
            waiverCustomerAndSignatureDTO.CustomerContentDTOList = new List<CustomerContentForWaiverDTO>();
            foreach (CustomerDTO signForCustomer in waiverCustomerAndSignatureDTO.SignForCustomerDTOList)
            {
                CustomerContentForWaiverDTO customerContentForWaiverDTO = new CustomerContentForWaiverDTO();
                customerContentForWaiverDTO.CustomerId = signForCustomer.Id;
                customerContentForWaiverDTO.CustomerName = signForCustomer.FirstName + " " + (string.IsNullOrEmpty(signForCustomer.LastName) ? string.Empty : signForCustomer.LastName);
                customerContentForWaiverDTO.PhoneNumber = signForCustomer.PhoneNumber;
                customerContentForWaiverDTO.EmailId = signForCustomer.Email;
                customerContentForWaiverDTO.CustomerDOB = signForCustomer.DateOfBirth;
                if (custAttributesInWaiverLookUpValueDTOList != null && custAttributesInWaiverLookUpValueDTOList.Any())
                {
                    string[] parts = custAttributesInWaiverLookUpValueDTOList[0].Description.Split('|');
                    if (parts != null && parts.Count() > 0)
                    {
                        customerContentForWaiverDTO.Attribute1Name = parts[0];
                        if (parts.Count() == 2)
                        {
                            customerContentForWaiverDTO.Attribute2Name = parts[1];
                        }
                    }
                }
                customerContentForWaiverDTO.WaiverCustomAttributeList = BuildCustomAttributeDetails(executionContext, signForCustomer, custAttributesInWaiverLookUpValueDTOList);
                waiverCustomerAndSignatureDTO.CustomerContentDTOList.Add(customerContentForWaiverDTO);
            }
            log.LogMethodExit(waiverCustomerAndSignatureDTO);
            return waiverCustomerAndSignatureDTO;
        }


        private static List<KeyValuePair<string, string>> BuildCustomAttributeDetails(ExecutionContext executionContext, CustomerDTO customerDTO, List<LookupValuesDTO> custAttributesInWaiverLookUpValueDTOList)
        {
            log.LogMethodEntry();
            List<KeyValuePair<string, string>> custAttributeList = new List<KeyValuePair<string, string>>();
            if (custAttributesInWaiverLookUpValueDTOList != null)
            {
                string[] parts = custAttributesInWaiverLookUpValueDTOList[0].Description.Split('|');
                foreach (LookupValuesDTO lookupValueDTO in custAttributesInWaiverLookUpValueDTOList)
                {
                    int partsLength = 0;
                    parts = lookupValueDTO.LookupValue.Split('|');
                    string attribute1 = string.Empty;
                    string attribute2 = string.Empty;
                    bool attributesFound = false;
                    while (partsLength < parts.Length)
                    {
                        CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(executionContext);
                        List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.APPLICABILITY, Applicability.CUSTOMER.ToString()));
                        searchParameters.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.NAME, parts[partsLength]));
                        List<CustomAttributesDTO> customAttributesDTOList = customAttributesListBL.GetCustomAttributesDTOList(searchParameters);
                        if (customAttributesDTOList != null && customerDTO.CustomDataSetDTO.CustomDataDTOList != null)
                        {
                            CustomDataDTO customDataDTO = customerDTO.CustomDataSetDTO.CustomDataDTOList.FirstOrDefault(x => x.CustomAttributeId == customAttributesDTOList[0].CustomAttributeId);
                            if (customDataDTO != null
                                && (customDataDTO.CustomDataNumber != null || (customDataDTO.CustomDataText != null && string.IsNullOrEmpty(customDataDTO.CustomDataText) == false)))
                            {
                                attributesFound = true;
                                if (partsLength == 0)
                                {
                                    if (customAttributesDTOList[0].Type == "NUMBER")
                                    {
                                        attribute1 = customDataDTO.CustomDataNumber.ToString();
                                    }
                                    else
                                    {
                                        attribute1 = customDataDTO.CustomDataText;
                                    }
                                }

                                if (partsLength == 1)
                                {
                                    if (customAttributesDTOList[0].Type == "NUMBER")
                                    {
                                        attribute2 = customDataDTO.CustomDataNumber.ToString();
                                    }
                                    else
                                    {
                                        attribute2 = customDataDTO.CustomDataText;
                                    }
                                }
                            }
                        }
                        partsLength++;
                    }
                    if (attributesFound)
                    {
                        custAttributeList.Add(new KeyValuePair<string, string>(attribute1, attribute2));
                    }
                }
            }
            log.LogMethodExit(custAttributeList);
            return custAttributeList;
        }
    }
}
