/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemoteVendorUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
2.110.0         30-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;


namespace Semnox.Parafait.Vendor
{
    public class RemoteVendorUseCases : RemoteUseCases, IVendorUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string VENDOR_URL = "api/Inventory/Vendors";
        private const string VENDOR_COUNT_URL = "api/Inventory/VendorCounts";

        public RemoteVendorUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<VendorDTO>> GetVendors(List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>
                          parameters, int currentPage = 0, int pageSize = 0)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("currentPage".ToString(), currentPage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize".ToString(), pageSize.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<VendorDTO> result = await Get<List<VendorDTO>>(VENDOR_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<int> GetVendorCount(List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>
                          parameters)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                int result = await Get<int>(VENDOR_COUNT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<VendorDTO.SearchByVendorParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case VendorDTO.SearchByVendorParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case VendorDTO.SearchByVendorParameters.NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("vendorName".ToString(), searchParameter.Value));
                        }
                        break;
                    case VendorDTO.SearchByVendorParameters.ADDRESS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("address".ToString(), searchParameter.Value));
                        }
                        break;
                    case VendorDTO.SearchByVendorParameters.VENDORMARKUPPERCENT:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("vendorMarkupPercent".ToString(), searchParameter.Value));
                        }
                        break;
                    case VendorDTO.SearchByVendorParameters.CONTACT_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("contactName".ToString(), searchParameter.Value));
                        }
                        break;
                    case VendorDTO.SearchByVendorParameters.VENDORCODE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("vendorCode".ToString(), searchParameter.Value));
                        }
                        break;
                    case VendorDTO.SearchByVendorParameters.VENDOR_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("vendorId".ToString(), searchParameter.Value));
                        }
                        break;
                    case VendorDTO.SearchByVendorParameters.CITY:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("city".ToString(), searchParameter.Value));
                        }
                        break;
                    case VendorDTO.SearchByVendorParameters.PHONE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("phone".ToString(), searchParameter.Value));
                        }
                        break;
                    case VendorDTO.SearchByVendorParameters.STATE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("state".ToString(), searchParameter.Value));
                        }
                        break;
                    case VendorDTO.SearchByVendorParameters.COUNTRY:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("country".ToString(), searchParameter.Value));
                        }
                        break;
                    case VendorDTO.SearchByVendorParameters.POSTAL_CODE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("postalCode".ToString(), searchParameter.Value));
                        }
                        break;
                    case VendorDTO.SearchByVendorParameters.EMAIL:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("email".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveVendors(List<VendorDTO> vendorDTOList)
        {
            log.LogMethodEntry(vendorDTOList);
            try
            {

                string content = JsonConvert.SerializeObject(vendorDTOList);
                string responseString = await Post<string>(VENDOR_URL, content);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

    }
}
