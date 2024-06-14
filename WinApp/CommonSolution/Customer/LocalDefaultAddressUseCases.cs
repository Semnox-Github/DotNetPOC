/********************************************************************************************
 * Project Name - Site
 * Description  - LocalDefaultAddressUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.0      09-07-2021     Prajwal S               Created : F&B web design
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    class LocalDefaultAddressUseCases : LocalUseCases, IDefaultAddressUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LocalDefaultAddressUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<CustomerDTO> SetDefaultAddress(int addressId)
        {
            return await Task<CustomerDTO>.Factory.StartNew(() =>
            {
                CustomerDTO result = null;
                log.LogMethodEntry(addressId);
                try
                {
                    AddressBL addressBL = new AddressBL(executionContext, addressId, null);
                    List<AddressDTO> addressDTOList = new List<AddressDTO>();
                    addressDTOList.Add(addressBL.AddressDTO);
                    if (addressDTOList[0].ProfileId == -1)
                    {
                        log.Error("Address not linked to a customer");
                        throw new ValidationException("Address not linked to a customer");
                    }
                    addressDTOList[0].IsDefault = true;
                    CustomerSearchCriteria searchCriteria = new CustomerSearchCriteria();
                    searchCriteria.And(CustomerSearchByParameters.CUSTOMER_PROFILE_ID, Operator.EQUAL_TO, addressDTOList[0].ProfileId.ToString());
                    CustomerListBL customerListBL = new CustomerListBL(executionContext);
                    List<CustomerDTO> customerDTOList = customerListBL.GetCustomerDTOList(searchCriteria, true, true);
                    if (customerDTOList == null || customerDTOList.Count == 0)
                    {
                        log.Error("Address not linked to a customer");
                        throw new ValidationException("Address not linked to a customer");
                    }
                    List<CustomerDTO> customerDTOs = customerDTOList.Where(x => x.ProfileId == addressDTOList[0].ProfileId).ToList();
                    List<AddressDTO> addressDTOs = customerDTOs[0].ProfileDTO.AddressDTOList.Where(x => x.IsDefault == true && x.Id != addressId).ToList();
                    if (addressDTOs.Any())
                    {
                        foreach (AddressDTO address in addressDTOs)
                        {
                            address.IsDefault = false;
                            addressDTOList.Add(address);
                        }
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            foreach (AddressDTO addressDTO in addressDTOList)
                            {
                                AddressBL addressDTOBL = new AddressBL(executionContext, addressDTO);
                                addressDTOBL.Save(parafaitDBTrx.SQLTrx);
                            }
                            parafaitDBTrx.EndTransaction();
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
                    CustomerBL customerBL = new CustomerBL(executionContext, customerDTOs[0].Id, true, true, null);
                    result = customerBL.CustomerDTO;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = null;
                    throw new ValidationException(ex.Message);
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
