/********************************************************************************************
 * Project Name - Customer 
 * Description  - LocalCustomerUseCases class to get the data  from local DB 
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          05-Dec-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
 *2.120.0     15-Mar-2021      Prajwal S                 Modified: Added Get for CustomerSummaryDTO. 
 *2.140.0     14-Sep-2021      Prajwal S                 Modified : Added Use case to save Address separately.
 *2.130.10    08-Sep-2022      Nitin Pai                 Modified as part of customer delete enhancement.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer.Accounts;

namespace Semnox.Parafait.Customer
{
    public class LocalCustomerUseCases : LocalUseCases, ICustomerUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalCustomerUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<List<CustomerDTO>> GetCustomerDTOList(List<KeyValuePair<CustomerSearchByParameters, string>> searchParameters,
            bool loadChildRecords = false, bool activeChildRecords = true, bool loadSignedWaivers = false, bool loadSignedWaiverFileContent = false)
        {
            return await Task<List<CustomerDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                CustomerListBL customerListBL = new CustomerListBL(executionContext);
                int siteId = GetSiteId();
                List<CustomerDTO> customerDTOList = customerListBL.GetCustomerDTOList(searchParameters, loadChildRecords, activeChildRecords, loadSignedWaivers, null, loadSignedWaiverFileContent, null);
                log.LogMethodExit(customerDTOList);
                return customerDTOList;
            });
        }

        public async Task<CustomerSummaryDTO> GetCustomerSummaryDTO(int customeId)
        {
            return await Task<CustomerSummaryDTO>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(customeId);
                CustomerSummaryBL customerSummaryBL = new CustomerSummaryBL(executionContext, customeId);
                CustomerSummaryDTO customerSummaryDTO = customerSummaryBL.CustomerSummaryDTO;
                log.LogMethodExit(customerSummaryDTO);
                return customerSummaryDTO;
            });
        }
        public async Task<CustomerDTO> SaveCustomerAddress(List<AddressDTO> addressDTOList, int customerId)
        {
            return await Task<CustomerDTO>.Factory.StartNew(() =>
            {
                CustomerDTO result = new CustomerDTO();
                try
                {
                    log.LogMethodEntry(addressDTOList);
                    if (addressDTOList == null)
                    {
                        throw new ValidationException("addressDTOList is Empty");
                    }
                    List<ValidationError> validationErrorList = new List<ValidationError>();
                    foreach (AddressDTO addressdTO in addressDTOList)
                    {
                        AddressBL addressBL = new AddressBL(executionContext, addressdTO);
                        validationErrorList = addressBL.ValidateAddress(null);
                        if (validationErrorList.Count > 0)
                        {
                            log.Error(validationErrorList);
                            throw new ValidationException("Invalid Input", validationErrorList);
                        }
                    }
                    CustomerBL customerBL = new CustomerBL(executionContext, customerId, true, false, null);
                    if (customerBL.CustomerDTO.ProfileDTO != null)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                if (addressDTOList.Any(x => x.IsDefault == true))
                                {
                                    if (customerBL.CustomerDTO.ProfileDTO.AddressDTOList != null && customerBL.CustomerDTO.ProfileDTO.AddressDTOList.Any(x => x.IsDefault == true))
                                    {
                                        foreach (AddressDTO addressDTO in customerBL.CustomerDTO.ProfileDTO.AddressDTOList)
                                            if (addressDTO.IsDefault == true)
                                            {
                                                addressDTO.IsDefault = false;
                                                addressDTOList.Add(addressDTO);

                                            }
                                    }
                                }
                                foreach (AddressDTO addressDTO in addressDTOList)
                                {
                                    addressDTO.ProfileId = customerBL.CustomerDTO.ProfileDTO.Id;
                                    AddressBL address = new AddressBL(executionContext, addressDTO);
                                    address.Save(parafaitDBTrx.SQLTrx);
                                }
                                parafaitDBTrx.EndTransaction();
                                customerBL = new CustomerBL(executionContext, customerId, true, true, null);
                                result = customerBL.CustomerDTO;
                            }
                            catch (ValidationException valEx)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(valEx);
                                throw valEx;
                            }
                            catch (Exception ex)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(ex);
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw ex;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = new CustomerDTO();
                    throw new ValidationException(ex.Message);
                }
                log.LogMethodExit(result);
                return result;
            });
        }
        private int GetSiteId()
        {
            log.LogMethodEntry();
            int siteId = -1;
            if (executionContext.GetIsCorporate())
            {
                siteId = executionContext.GetSiteId();
            }
            log.LogMethodExit(siteId);
            return siteId;
        }

        public async Task DeleteCustomer(int customerId)
        {
            log.LogMethodEntry(customerId);
            List<CustomerDTO> deletedCustomers = null;


            await Task.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        DeleteCustomerBL deleteCustomerBL = new DeleteCustomerBL(executionContext, customerId);
                        deleteCustomerBL.DeleteCustomer(parafaitDBTrx.SQLTrx);
                        parafaitDBTrx.EndTransaction();
                    }
                    catch (ValidationException valEx)
                    {
                        CustomerActivityUserLogDTO customerActivityUserLogDTO = new CustomerActivityUserLogDTO(-1, customerId, "",
                                    "CUSTOMER_DELETE", "Got validation exception while deleting customer" + customerId, ServerDateTime.Now,
                                    "POS " + executionContext.GetPosMachineGuid(), valEx.ToString(),
                                    Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivityCategory), CustomerActivityUserLogDTO.ActivityCategory.PROFILE),
                                    Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivitySeverity), CustomerActivityUserLogDTO.ActivitySeverity.ERROR));
                        CustomerActivityUserLogBL customerActivityUserLogBL = new CustomerActivityUserLogBL(executionContext, customerActivityUserLogDTO);
                        customerActivityUserLogBL.Save();

                        parafaitDBTrx.RollBack();
                        log.Error(valEx);
                        throw valEx;
                    }
                    catch (Exception ex)
                    {
                        CustomerActivityUserLogDTO customerActivityUserLogDTO = new CustomerActivityUserLogDTO(-1, customerId, "",
                                    "CUSTOMER_DELETE", "Got exception while deleting customer" + customerId, ServerDateTime.Now,
                                    "POS " + executionContext.GetPosMachineGuid(), ex.ToString(),
                                    Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivityCategory), CustomerActivityUserLogDTO.ActivityCategory.PROFILE),
                                    Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivitySeverity), CustomerActivityUserLogDTO.ActivitySeverity.ERROR));
                        CustomerActivityUserLogBL customerActivityUserLogBL = new CustomerActivityUserLogBL(executionContext, customerActivityUserLogDTO);
                        customerActivityUserLogBL.Save();

                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw ex;
                    }
                }

                // Create roaming entries outside of the SQl transaction as the entire SQL transaction is long one and the entries made in this may be skipped by the upload download job.
                try
                {
                    if (deletedCustomers != null)
                    {
                        foreach (CustomerDTO customerDTO in deletedCustomers)
                        {
                            // create roaming data for customer outside of transaction so that the upload job can pick up the full update in one batch
                            CustomerBL customerBLForDBSync = new CustomerBL(this.executionContext, customerDTO);
                            customerBLForDBSync.CreateRoamingDataForCustomer();
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error while creating DB synch entries for Customer " + ex);
                }
            });

            log.LogMethodExit();
        }

        public async Task<string> SaveCustomerNickname(int customerId, string nickname)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(customerId, nickname);
                CustomerBL customerBL = new CustomerBL(executionContext, customerId);
                //if (customerBL.CustomerDTO == null || customerBL.CustomerDTO.Id < 0)
                //{
                //    log.Error("Customers does not exist for the customer Ids:" + customerId);
                //    string message = MessageContainerList.GetMessage(executionContext, 2524, customerId);
                //    throw new ValidationException(message);
                //}
                string result = string.Empty;
                if (customerBL.CustomerDTO.ProfileDTO != null)
                {
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            ProfileDTO profileDTO = customerBL.CustomerDTO.ProfileDTO;
                            profileDTO.NickName = nickname;
                            customerBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                            result = MessageContainerList.GetMessage(executionContext, 5177);
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ex;
                        }
                    }
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
