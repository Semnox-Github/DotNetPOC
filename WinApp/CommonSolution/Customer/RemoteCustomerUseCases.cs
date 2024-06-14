/********************************************************************************************
 * Project Name - Customer
 * Description  - RemoteCustomerUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          05-Dec-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
 *2.120.0     15-Mar-2021      Prajwal S                 Modified: Added Get for CustomerSummaryDTO. 
 *2.130.10    08-Sep-2022      Nitin Pai                 Modified as part of customer delete enhancement.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
    public class RemoteCustomerUseCases : RemoteUseCases, ICustomerUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string CUSTOMER_URL = "api/Customer/Customers";
        private const string CUSTOMER_SUMMARY_URL = "api/Customer/{customerId}/Summary";
        private const string CUSTOMER_NICKNAME_URL = "api/Customer/{customerId}/Nickname";

        public RemoteCustomerUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<CustomerDTO>> GetCustomerDTOList(List<KeyValuePair<CustomerSearchByParameters, string>> searchParameters,
            bool loadChildRecords = false, bool activeChildRecords = true, bool loadSignedWaivers = false, bool loadSignedWaiverFileContent = false)
        {
            log.LogMethodEntry(searchParameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("buildChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeRecordsOnly".ToString(), activeChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadSignedWaivers".ToString(), loadSignedWaivers.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadSignedWaiverFileContent".ToString(), loadSignedWaiverFileContent.ToString()));
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<CustomerDTO> result = await Get<List<CustomerDTO>>(CUSTOMER_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<CustomerSearchByParameters, string>> redemptionSearchParams)
        {
            log.LogMethodEntry(redemptionSearchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<CustomerSearchByParameters, string> searchParameter in redemptionSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case CustomerSearchByParameters.CUSTOMER_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("orderId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerSearchByParameters.ISACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<CustomerSummaryDTO> GetCustomerSummaryDTO(int customerId)
        {
            log.LogMethodEntry(customerId);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("customerId".ToString(), customerId.ToString()));
            try
            {
                CustomerSummaryDTO result = await Get<CustomerSummaryDTO>(CUSTOMER_SUMMARY_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<CustomerDTO> SaveCustomerAddress(List<AddressDTO> addressDTOList, int customerId)
        {
            log.LogMethodEntry(addressDTOList);
            try
            {
                string CUSTOMER_ADDRESS_SAVE_URL = "api/Custome/customers/" + customerId + "/Address";
                CustomerDTO responseString = await Post<CustomerDTO>(CUSTOMER_ADDRESS_SAVE_URL, addressDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task DeleteCustomer(int customerId)
        {
            log.LogMethodEntry(customerId);
            try
            {
                string CUSTOMER_ADDRESS_SAVE_URL = "api/Customer/customers/" + customerId + "/Delete";
                await Post<CustomerDTO>(CUSTOMER_ADDRESS_SAVE_URL, null);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<string> SaveCustomerNickname(int customerId, string nickname)
        {
            log.LogMethodEntry(customerId, nickname);
            try
            {
                string responseString = await Post<string>(CUSTOMER_NICKNAME_URL.Replace("{customerId}", customerId.ToString()), nickname);
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
