
/********************************************************************************************
 * Project Name -TET 
 * Description  - GetVisitor
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By                    Remarks          
 *********************************************************************************************
 *1.00        26-Feb-2022   Nagendra Prasad(Vidita)       Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;
using Newtonsoft.Json.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using System.Text.RegularExpressions;
using Semnox.Parafait.Customer.Accounts;

namespace Semnox.Parafait.ThirdParty.TET
{
    public class GetVisitor
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().ToString());
        private ExecutionContext executionContext = null;
        string responseMessage = string.Empty;
        string message = string.Empty;
        string orderId = string.Empty;
        string errorMessage = string.Empty;

        //Defult construtor for the Mapper method
        public GetVisitor(ExecutionContext executionContext, string OrderId)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.orderId = OrderId;
            log.LogMethodExit();
        }
        public JObject GetVisitorResponse()
        {
            log.LogMethodEntry();
            try
            {
                //Execution of the get method start
                GetVisitorResponse.GetVisitorResponseRnt Responsernt = new GetVisitorResponse.GetVisitorResponseRnt();
                JObject retrieveVisitor = new JObject();
                if (!string.IsNullOrEmpty(orderId))
                {
                    Utilities utilities = new Utilities();
                    // Below code is to get user details
                    Users user = new Users(executionContext, executionContext.GetUserId(), executionContext.GetSiteId());

                    if (String.IsNullOrEmpty(user.UserDTO.LoginId))
                    {
                        errorMessage = "Please setup the user " + executionContext.GetUserId() + ":" + executionContext.GetSiteId();
                        log.Error(errorMessage);
                        throw new ValidationException(errorMessage);
                    }
                    bool isCorporate = executionContext.GetIsCorporate();
                    utilities.ParafaitEnv.LoginID = user.UserDTO.LoginId;
                    utilities.ParafaitEnv.User_Id = user.UserDTO.UserId;
                    utilities.ParafaitEnv.RoleId = user.UserDTO.RoleId;
                    utilities.ParafaitEnv.IsCorporate = isCorporate;
                    utilities.ParafaitEnv.POSMachineId = executionContext.GetMachineId();
                    utilities.ParafaitEnv.POSMachineGuid = executionContext.GetPosMachineGuid();
                    utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
                    utilities.ExecutionContext.SetIsCorporate(isCorporate);
                    utilities.ExecutionContext.SetMachineId(executionContext.GetMachineId());
                    utilities.ParafaitEnv.Initialize();
                    //Get the transactionDTO
                    List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.GUID, orderId));
                    TransactionListBL transactionListBL = new TransactionListBL(executionContext);
                    List<TransactionDTO> transactionDTOList = transactionListBL.GetTransactionDTOList(searchParams, utilities, null, 0, 10, true);
                    log.LogVariableState("transactionDTOList", transactionDTOList);

                    if (transactionDTOList != null && transactionDTOList.Any())
                    {
                        foreach (TransactionDTO transactionDTO in transactionDTOList)
                        {
                            List<TransactionLineDTO> transactionLineDTOList = transactionDTO.TransactionLinesDTOList;
                            if (transactionLineDTOList != null && transactionLineDTOList.Any())
                            {
                                int? numberOfVisitor = 0;
                                bool isVirtualTicketEnabled = false;
                                foreach (TransactionLineDTO transLineDTO in transactionLineDTOList)
                                {
                                    try
                                    {
                                        //Get the productContainerDTO
                                        ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetProductsContainerDTO(executionContext.GetSiteId(), transLineDTO.ProductId);
                                        if (productsContainerDTO.ExternalSystemReference == "TET")
                                        {
                                            isVirtualTicketEnabled = true;
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error("Product :" + ex.Message);
                                    }
                                }
                                log.LogVariableState("isVirtualTicketEnabled", isVirtualTicketEnabled);

                                if (isVirtualTicketEnabled)
                                {
                                    List<TransactionLineDTO> result = transactionLineDTOList.Where(line => line.CardId > -1 && string.IsNullOrWhiteSpace(line.CardNumber) == false).ToList();
                                    log.LogVariableState("result", result);
                                    List<int> distinctCards = new List<int>();
                                    if (result != null && result.Any())
                                    {
                                        distinctCards = result.Select(line => line.CardId).ToList();
                                        log.LogVariableState("distinctCards", distinctCards);
                                        numberOfVisitor = distinctCards.Distinct().ToList().Count;
                                        log.LogVariableState("numberOfVisitor", numberOfVisitor);
                                    }
                                }
                                //check for if any vistor exist
                                if (numberOfVisitor > 0)
                                {
                                    try
                                    {
                                        Responsernt.NumberOfVisitor = Convert.ToInt32(numberOfVisitor);
                                        Responsernt.VisitorDate = transactionDTO.TransactionDate;
                                        log.LogVariableState("Responsernt", Responsernt);

                                    }
                                    catch (Exception ex)
                                    {
                                        log.Info("Line 145: " + ex.Message);
                                    }
                                }
                            }
                            try
                            {
                                //Get the CustomerDTOList
                                int customerId = -1;
                                if (transactionDTO.CustomerId > -1)
                                {
                                    customerId = transactionDTO.CustomerId;
                                    log.Debug("transactionDTO customerId : " + customerId);
                                }
                                else if (!string.IsNullOrEmpty(transactionDTO.PrimaryCard))
                                {
                                    AccountBL accountBL = new AccountBL(utilities.ExecutionContext, transactionDTO.PrimaryCard, false, false);
                                    if (accountBL != null && accountBL.AccountDTO != null && accountBL.AccountDTO.CustomerId > -1)
                                    {
                                        customerId = accountBL.AccountDTO.CustomerId;
                                        log.Debug("PrimaryCard customerId : " + customerId);
                                    }
                                }
                                if (customerId > -1)
                                {
                                    CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, customerId, true, true);
                                    if (customerBL != null && customerBL.CustomerDTO != null)
                                    {
                                        log.Debug("customer exists ");
                                        Responsernt.VisitorEmail = customerBL.CustomerDTO.Email;
                                        log.Debug("VisitorEmail : " + Responsernt.VisitorEmail);
                                    }
                                }
                                else if (!string.IsNullOrEmpty(transactionDTO.CustomerIdentifier))
                                {
                                    string decryptedCustomerReference = Encryption.Decrypt(transactionDTO.CustomerIdentifier);
                                    string[] customerIdentifierStringArray = decryptedCustomerReference.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                                    for (int i = 0; i < customerIdentifierStringArray.Length; i++)
                                    {
                                        if (Regex.IsMatch(customerIdentifierStringArray[i], @"^((([\w]+\.[\w]+)+)|([\w]+))@(([\w]+\.)+)([A-Za-z]{1,3})$"))
                                        {
                                            Responsernt.VisitorEmail = customerIdentifierStringArray[i];
                                            log.Debug("VisitorEmail : " + Responsernt.VisitorEmail);
                                        }
                                    }
                                }
                                else
                                {
                                    log.Info("No valid customer in transaction . Assigning the default email Id.");
                                    Responsernt.VisitorEmail = "abc@gmail.com";
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error("Error while creating the retrieveVisitor response . Please check the stack trace ");
                                log.Error(ex);
                            }
                        }
                    }
                }
                retrieveVisitor = JObject.FromObject(Responsernt);
                log.LogMethodExit(retrieveVisitor);
                return retrieveVisitor;
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                throw ex;
            }
        }

    }
}