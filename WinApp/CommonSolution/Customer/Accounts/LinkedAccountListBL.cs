/********************************************************************************************
 * Project Name - Accounts
 * Description  - LinkedAccountListBL class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         23-Apr-2022       Nitin Pai                 Created : 
 2.130.11    14-Oct-2022     Yashodhara C H            Added new method to get LinkedAccountsummary details 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer.Accounts
{
    public class LinkedAccountListBL
    {
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public LinkedAccountListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public List<AccountDTO> GetLinkedAccounts(List<KeyValuePair<AccountDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);

            List<AccountDTO> linkedAccountDTOList = new List<AccountDTO>();
            List<KeyValuePair<AccountDTO.SearchByParameters, string>> customerSearchParameters = searchParameters.Where(x => x.Key == AccountDTO.SearchByParameters.CUSTOMER_ID).ToList();
            if (customerSearchParameters != null && customerSearchParameters.Any())
            {
                int customerId = Convert.ToInt32(customerSearchParameters[0].Value);
                if(customerId > -1)
                {
                    log.Debug("Getting related cards for customer " + customerId);
                    List<int> relatedCards = GetRelatedCards(customerId);
                    if (relatedCards != null && relatedCards.Any())
                    {
                        log.Debug("Cards found for the customer " + string.Join(",", relatedCards));
                        List<KeyValuePair<AccountDTO.SearchByParameters, string>> linkedCardSearchParameters = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                        linkedCardSearchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.ACCOUNT_ID_LIST, string.Join(",", relatedCards)));

                        AccountListBL accountListBL = new AccountListBL(executionContext);
                        linkedAccountDTOList = accountListBL.GetAccountDTOList(linkedCardSearchParameters, true, true);
                    }
                    else
                    {
                        log.Debug("No cards found for the customer");
                    }
                }
                else
                {
                    log.Debug("Customer not found in the input search parameters");
                }
            }
            else
            {
                log.Debug("Customer not found in the input search parameters");
            }

            log.LogMethodExit(linkedAccountDTOList);
            return linkedAccountDTOList;
        }

        /// <summary>
        /// Gets the List of AccountSummaryViewDTO's
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<AccountSummaryViewDTO> GetLinkedAccountSummaryView(List<KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);

            List<AccountSummaryViewDTO> linkedAccountSummaryDTOList = new List<AccountSummaryViewDTO>();
            List<KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>> customerSearchParameters = searchParameters.Where(x => x.Key == AccountSummaryViewDTO.SearchByParameters.CUSTOMER_ID).ToList();
            if (customerSearchParameters != null && customerSearchParameters.Any())
            {
                int customerId = Convert.ToInt32(customerSearchParameters[0].Value);
                if (customerId > -1)
                {
                    log.Debug("Getting related cards for customer " + customerId);
                    List<int> relatedCards = GetRelatedCards(customerId);
                    if (relatedCards != null && relatedCards.Any())
                    {
                        log.Debug("Cards found for the customer " + string.Join(",", relatedCards));
                        List<KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>> linkedCardSearchParameters = new List<KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>>();
                        linkedCardSearchParameters.Add(new KeyValuePair<AccountSummaryViewDTO.SearchByParameters, string>(AccountSummaryViewDTO.SearchByParameters.ACCOUNT_ID_LIST, string.Join(",", relatedCards)));

                        AccountSummaryViewListBL accountSummaryViewListBL = new AccountSummaryViewListBL(executionContext);
                        linkedAccountSummaryDTOList = accountSummaryViewListBL.GetAccountSummaryViewDTOList(linkedCardSearchParameters);
                    }
                    else
                    {
                        log.Debug("No cards found for the customer");
                    }
                }
                else
                {
                    log.Debug("Customer not found in the input search parameters");
                }
            }
            else
            {
                log.Debug("Customer not found in the input search parameters");
            }

            log.LogMethodExit(linkedAccountSummaryDTOList);
            return linkedAccountSummaryDTOList;
        }


        private List<int> GetRelatedCards(int customerId)
        {
            log.LogMethodEntry(customerId);
            List<int> relatedCards = new List<int>();

            AccountListBL accountListBL = new AccountListBL(executionContext);
            List<KeyValuePair<AccountDTO.SearchByParameters, string>> customerSearchParameters = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
            customerSearchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
            customerSearchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.VALID_FLAG, "Y"));
                    customerSearchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.REFUND_FLAG, "N"));
                    customerSearchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.TECHNICIAN_CARD, "N"));
            // Get accounts with base search params like valid flag, refund flag etc
            log.Debug("Gettting cards for the customer " + customerId);
            List<AccountDTO> linkedAccountsDTOList = accountListBL.GetAccountDTOList(customerSearchParameters, false, false);
                    if (linkedAccountsDTOList != null && linkedAccountsDTOList.Any())
                    {
                        log.Debug("Got Cards. Not getting linked child cards");
                        // get list of linked cards
                        String cardIdList = "";
                        foreach (AccountDTO tempAccountDTO in linkedAccountsDTOList)
                            cardIdList += (cardIdList.Length > 0 ? "," : "") + tempAccountDTO.AccountId.ToString();
                        log.Debug("Customer cards list " + cardIdList);
                        List<KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>> parentChildCardSearchParameters = new List<KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>>();
                        parentChildCardSearchParameters.Add(new KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>(AccountRelationshipDTO.SearchByParameters.ACCOUNT_ID_LIST, cardIdList));

                        AccountRelationshipListBL parentChildCardsListBL = new AccountRelationshipListBL(executionContext);

                        log.Debug("Getting linked cards");
                        List<AccountRelationshipDTO> parentChildCardsDTOList = parentChildCardsListBL.GetAccountRelationshipDTOList(parentChildCardSearchParameters);
                        if (parentChildCardsDTOList != null && parentChildCardsDTOList.Any())
                        {
                            String childCardsList = "";
                            foreach (AccountRelationshipDTO tempCardDTO in parentChildCardsDTOList)
                                childCardsList += (childCardsList.Length > 0 ? "," : "") + tempCardDTO.RelatedAccountId.ToString();

                            log.Debug("Customer linked cards list " + childCardsList);
                            List<KeyValuePair<AccountDTO.SearchByParameters, string>> linkedCardsSearchParameters = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                            linkedCardsSearchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.ACCOUNT_ID_LIST, childCardsList));
                            linkedCardsSearchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.VALID_FLAG, "Y"));
                            List<AccountDTO> linkedCardsAccountsDTOList = accountListBL.GetAccountDTOList(linkedCardsSearchParameters, false, false);
                            if(linkedCardsAccountsDTOList!= null && linkedCardsAccountsDTOList.Any())
                    {
                        linkedAccountsDTOList.AddRange(linkedCardsAccountsDTOList);
                        foreach (AccountDTO tempDTO in linkedCardsAccountsDTOList)
                            log.Debug("Related account DTO " + tempDTO.AccountId + ":" + tempDTO.TagNumber + ":" + tempDTO.AccountIdentifier + ":" + tempDTO.CustomerId + ":" + tempDTO.CustomerName);
                    }
                }
            }

                    // get cards for childs
                    // get list of child relations type
                    log.Debug("Getting relationship types");
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
                    String relatedCustomerList = "";
                    if (!string.IsNullOrWhiteSpace(childTypeIdList))
                    {
                        log.Debug("Getting relationships " + childTypeIdList + ":" + customerId);
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
                                // only take the children of the input customer. Do not consider other relationship types
                                if (temp.CustomerId == customerId)
                                    relatedCustomerList += (relatedCustomerList.Length > 0 ? "," : "") + temp.RelatedCustomerId.ToString();

                                log.Debug("Related customers list " + relatedCustomerList);
                            }
                        }
                    }

                    // now get cards for these customers
                    if (!string.IsNullOrWhiteSpace(relatedCustomerList))
                    {
                        log.Debug("Getting cards for relationships");
                        List<KeyValuePair<AccountDTO.SearchByParameters, string>> relatedCustSearchParameters = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                        relatedCustSearchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.CUSTOMER_ID_LIST, relatedCustomerList));
                relatedCustSearchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.VALID_FLAG, "Y"));

                List<AccountDTO> relatedCustomerAccountDTO = accountListBL.GetAccountDTOList(relatedCustSearchParameters, false, false);
                        if (relatedCustomerAccountDTO != null && relatedCustomerAccountDTO.Any())
                        {
                            linkedAccountsDTOList.AddRange(relatedCustomerAccountDTO);

                            foreach (AccountDTO tempDTO in relatedCustomerAccountDTO)
                                log.Debug("Related account DTO " + tempDTO.AccountId + ":" + tempDTO.TagNumber + ":" + tempDTO.AccountIdentifier + ":" + tempDTO.CustomerId + ":" + tempDTO.CustomerName);
                        }
                    }

            if(linkedAccountsDTOList != null && linkedAccountsDTOList.Any())
            {
                foreach(AccountDTO accountDTO in linkedAccountsDTOList)
                {
                    relatedCards.Add(accountDTO.AccountId);
                }
            }

            log.LogMethodExit(relatedCards);
            return relatedCards;
        }

    }

}
