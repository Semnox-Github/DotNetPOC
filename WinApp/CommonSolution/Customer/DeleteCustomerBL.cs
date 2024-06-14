/********************************************************************************************
 * Project Name - Customer 
 * Description  - DeleteCustomerBL class to delete the customer data
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.130.10     15-Aug-2022     Nitin Pai                 Created: Initial version to delete customer data. 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.GenericUtilities;

namespace Semnox.Parafait.Customer
{
    public class DeleteCustomerBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private int customerId;

        public DeleteCustomerBL(ExecutionContext executionContext, int customerId)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.customerId = customerId;
            log.LogMethodExit();
        }

        public List<CustomerDTO> DeleteCustomer(SqlTransaction sqlTransaction)
        {
            List<CustomerDTO> deletedCustomers = new List<CustomerDTO>();
            String customerList = "";

            CustomerActivityUserLogDTO customerActivityUserLogDTO = new CustomerActivityUserLogDTO(-1, customerId, "",
                                    "CUSTOMER_DELETE", "Initiating delete of customer " + customerId, ServerDateTime.Now,
                                    "POS " + executionContext.GetPosMachineGuid(), customerId.ToString(),
                                    Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivityCategory), CustomerActivityUserLogDTO.ActivityCategory.PROFILE),
                                    Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivitySeverity), CustomerActivityUserLogDTO.ActivitySeverity.INFO));
            CustomerActivityUserLogBL customerActivityUserLogBL = new CustomerActivityUserLogBL(executionContext, customerActivityUserLogDTO);
            customerActivityUserLogBL.Save();

            try
            {
                CustomerBL customerBL = new CustomerBL(executionContext, customerId, true, false, null);
                if (customerBL.CustomerDTO == null || customerBL.CustomerDTO.Id == -1)
                {
                    log.Error("Invalid customer");
                    throw new ValidationException("Invalid customer id");
                }

                deletedCustomers.Add(customerBL.CustomerDTO);
                customerList = customerId.ToString();

                String childTypeIdList = "";
                CustomerRelationshipTypeListBL customerRelationshipTypeListBL = new CustomerRelationshipTypeListBL(executionContext);
                List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>> searchParameter = new List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>>();
                searchParameter.Add(new KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>(CustomerRelationshipTypeDTO.SearchByParameters.NAME, "CHILD"));
                List<CustomerRelationshipTypeDTO> customerRelationshipTypeDTOList = customerRelationshipTypeListBL.GetCustomerRelationshipTypeDTOList(searchParameter);
                if (customerRelationshipTypeDTOList != null && customerRelationshipTypeDTOList.Any())
                {
                    foreach (CustomerRelationshipTypeDTO temp in customerRelationshipTypeDTOList)
                        childTypeIdList += (childTypeIdList.Length > 0 ? "," : "") + temp.Id.ToString();

                    log.Debug("Customer child relationship type " + childTypeIdList);
                }

                // Now get the list of related child customers
                if (!string.IsNullOrWhiteSpace(childTypeIdList))
                {
                    List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>> relationsSearchParameters = new List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>>();
                    relationsSearchParameters.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.CUSTOMER_RELATIONSHIP_TYPE_ID_LIST, childTypeIdList));
                    relationsSearchParameters.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
                    relationsSearchParameters.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.IS_ACTIVE, "1"));
                    CustomerRelationshipListBL customerRelationshipListBL = new CustomerRelationshipListBL(executionContext);
                    List<CustomerRelationshipDTO> customerRelationshipDTOList = customerRelationshipListBL.GetCustomerRelationshipDTOList(relationsSearchParameters, false);
                    if (customerRelationshipDTOList != null && customerRelationshipDTOList.Any())
                    {
                        log.Debug("Got related customers ");
                        foreach (CustomerRelationshipDTO temp in customerRelationshipDTOList)
                        {
                            CustomerBL relatedCustomerBL = new CustomerBL(executionContext, temp.RelatedCustomerId, true, false, null);
                            deletedCustomers.Add(relatedCustomerBL.CustomerDTO);
                            customerList = customerList + "," + temp.RelatedCustomerId;
                        }
                        log.Debug("Related customers " + customerList);
                    }
                }

                if (!string.IsNullOrWhiteSpace(customerList))
                {
                    log.Debug("Getting cards for customers");
                    List<KeyValuePair<AccountDTO.SearchByParameters, string>> relatedCustSearchParameters = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                    relatedCustSearchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.CUSTOMER_ID_LIST, customerList));

                    AccountListBL accountListBL = new AccountListBL(executionContext);
                    List<AccountDTO> relatedCustomerAccountDTO = accountListBL.GetAccountDTOList(relatedCustSearchParameters, true, true);
                    if (relatedCustomerAccountDTO != null && relatedCustomerAccountDTO.Any())
                    {
                        foreach (AccountDTO tempDTO in relatedCustomerAccountDTO)
                        {
                            log.Debug("Unlinking account from customer" + tempDTO.AccountId + ":" + tempDTO.TagNumber + ":" + tempDTO.AccountIdentifier + ":" + tempDTO.CustomerId + ":" + tempDTO.CustomerName + " as part of account delete of " + customerId);
                            CustomerActivityUserLogDTO unlinkcustomerActivityUserLogDTO = new CustomerActivityUserLogDTO(-1, tempDTO.CustomerId, "",
                                        "ACCOUNT_UNLINK", "Unlinking account from customer " + tempDTO.TagNumber + ":" + tempDTO.CustomerId + " as part of account delete of " + customerId, ServerDateTime.Now,
                                        "POS " + executionContext.GetPosMachineGuid(), customerId.ToString(),
                                        Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivityCategory), CustomerActivityUserLogDTO.ActivityCategory.CARD),
                                        Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivitySeverity), CustomerActivityUserLogDTO.ActivitySeverity.INFO));
                            CustomerActivityUserLogBL unlinkcustomerActivityUserLogBL = new CustomerActivityUserLogBL(executionContext, unlinkcustomerActivityUserLogDTO);
                            unlinkcustomerActivityUserLogBL.Save();

                            AccountBL accountBL = new AccountBL(executionContext, tempDTO);
                            accountBL.RemoveCustomerLink(sqlTransaction);
                        }
                    }
                }

                foreach (CustomerDTO temp in deletedCustomers)
                {
                    log.Debug("Deleting related customers " + temp.Id);
                    CustomerBL relatedCustomerBL = new CustomerBL(executionContext, temp);
                    relatedCustomerBL.InactivateCustomer(sqlTransaction);

                    ParafaitMessageQueueDTO parafaitMessageQueueDTO = new ParafaitMessageQueueDTO(-1, relatedCustomerBL.CustomerDTO.Guid, ParafaitMessageQueueDTO.EntityNames.Customer.ToString(),
                                                   string.Empty, MessageQueueStatus.UnRead, true, ParafaitMessageQueueDTO.ActionTypes.DeleteCustomerFromBooking.ToString(), "Customer is being deleted as part of delete request for customer " + customerId, 0);

                    ParafaitMessageQueueBL parafaitMessageQueueBL = new ParafaitMessageQueueBL(executionContext, parafaitMessageQueueDTO);
                    parafaitMessageQueueBL.Save(sqlTransaction);

                    CustomerActivityUserLogDTO unlinkcustomerActivityUserLogDTO = new CustomerActivityUserLogDTO(-1, temp.Id, "",
                                        "CUSTOMER_DELETE", "Successfully deleted customer " + temp.Id + " as part of account delete of " + customerId, ServerDateTime.Now,
                                        "POS " + executionContext.GetPosMachineGuid(), customerId.ToString(),
                                        Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivityCategory), CustomerActivityUserLogDTO.ActivityCategory.PROFILE),
                                        Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivitySeverity), CustomerActivityUserLogDTO.ActivitySeverity.INFO));
                    CustomerActivityUserLogBL unlinkcustomerActivityUserLogBL = new CustomerActivityUserLogBL(executionContext, unlinkcustomerActivityUserLogDTO);
                    unlinkcustomerActivityUserLogBL.Save();
                }
            }
            catch(Exception ex)
            {
                log.Error("Eror occured while deleting customer " + ex);
                CustomerActivityUserLogDTO unlinkcustomerActivityUserLogDTO = new CustomerActivityUserLogDTO(-1, customerId, "",
                                        "CUSTOMER_DELETE", "Error occured while deleting customer ", ServerDateTime.Now,
                                        "POS " + executionContext.GetPosMachineGuid(), ex.Message,
                                        Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivityCategory), CustomerActivityUserLogDTO.ActivityCategory.PROFILE),
                                        Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivitySeverity), CustomerActivityUserLogDTO.ActivitySeverity.INFO));
                CustomerActivityUserLogBL unlinkcustomerActivityUserLogBL = new CustomerActivityUserLogBL(executionContext, unlinkcustomerActivityUserLogDTO);
                unlinkcustomerActivityUserLogBL.Save();
                throw;
            }

            return deletedCustomers;
        }
    }
}
