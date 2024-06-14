/********************************************************************************************
 * Project Name - AccountCreditPlusConsumption
 * Description  - BL AccountCreditPlusConsumption
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        01-Aug-2019      Girish Kundar  Added Comments and removed unused namespaces.
 *2.80.0      21-May-2020      Girish Kundar  Modified : Made default constructor as Private   
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Category;
using Semnox.Parafait.DBSynch;
using Semnox.Parafait.Game;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Business logic for AccountCreditPlusConsumption class.
    /// </summary>
    public class AccountCreditPlusConsumptionBL
    {
        private AccountCreditPlusConsumptionDTO accountCreditPlusConsumptionDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of AccountCreditPlusConsumptionBL class
        /// </summary>
        private AccountCreditPlusConsumptionBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the accountCreditPlusConsumption id as the parameter
        /// Would fetch the accountCreditPlusConsumption object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public AccountCreditPlusConsumptionBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            AccountCreditPlusConsumptionDataHandler accountCreditPlusConsumptionDataHandler = new AccountCreditPlusConsumptionDataHandler(sqlTransaction);
            accountCreditPlusConsumptionDTO = accountCreditPlusConsumptionDataHandler.GetAccountCreditPlusConsumptionDTO(id);
            if (accountCreditPlusConsumptionDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AccountCreditPlusConsumption", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AccountCreditPlusConsumptionBL object using the AccountCreditPlusConsumptionDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="accountCreditPlusConsumptionDTO">AccountCreditPlusConsumptionDTO object</param>
        public AccountCreditPlusConsumptionBL(ExecutionContext executionContext, AccountCreditPlusConsumptionDTO accountCreditPlusConsumptionDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, accountCreditPlusConsumptionDTO);
            this.accountCreditPlusConsumptionDTO = accountCreditPlusConsumptionDTO;
            log.LogMethodExit();
        }

       
        /// <summary>
        /// Saves the AccountCreditPlusConsumption
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        internal void Save(int parentSiteId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(parentSiteId, sqlTransaction);
            AccountCreditPlusConsumptionDataHandler accountCreditPlusConsumptionDataHandler = new AccountCreditPlusConsumptionDataHandler(sqlTransaction);
            if (accountCreditPlusConsumptionDTO.IsChanged)
            {
                if (accountCreditPlusConsumptionDTO.IsActive)
                {
                    if (accountCreditPlusConsumptionDTO.AccountCreditPlusConsumptionId < 0)
                    {
                        accountCreditPlusConsumptionDTO = accountCreditPlusConsumptionDataHandler.InsertAccountCreditPlusConsumption(accountCreditPlusConsumptionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        accountCreditPlusConsumptionDTO.AcceptChanges();
                    }
                    else
                    {
                        if (accountCreditPlusConsumptionDTO.IsChanged)
                        {
                            accountCreditPlusConsumptionDTO = accountCreditPlusConsumptionDataHandler.UpdateAccountCreditPlusConsumption(accountCreditPlusConsumptionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                            accountCreditPlusConsumptionDTO.AcceptChanges();
                        }
                    }
                    CreateRoamingData(parentSiteId, sqlTransaction);
                }
                else
                {
                    if (accountCreditPlusConsumptionDTO.AccountCreditPlusConsumptionId >= 0)
                    {
                        accountCreditPlusConsumptionDataHandler.DeleteAccountCreditPlusConsumption(accountCreditPlusConsumptionDTO.AccountCreditPlusConsumptionId);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AccountCreditPlusConsumptionDTO AccountCreditPlusConsumptionDTO
        {
            get
            {
                return accountCreditPlusConsumptionDTO;
            }
        }

        /// <summary>
        /// Validates the customer relationship DTO
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if(accountCreditPlusConsumptionDTO.POSTypeId == -1 &&
               accountCreditPlusConsumptionDTO.ProductId == -1 &&
               accountCreditPlusConsumptionDTO.CategoryId == -1 &&
               accountCreditPlusConsumptionDTO.GameId == -1 &&
               accountCreditPlusConsumptionDTO.GameProfileId == -1)
            {
                validationErrorList.Add(new ValidationError("AccountCreditPlusConsumption", "POSTypeId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "POS Counter"))));
            }
            if(accountCreditPlusConsumptionDTO.DiscountPercentage.HasValue &&
                (accountCreditPlusConsumptionDTO.DiscountPercentage < 0 || accountCreditPlusConsumptionDTO.DiscountPercentage > 100))
            {
                validationErrorList.Add(new ValidationError("AccountCreditPlusConsumption", "DiscountPercentage", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Discount Percentage"))));
            }
            if (accountCreditPlusConsumptionDTO.DiscountedPrice.HasValue &&
                accountCreditPlusConsumptionDTO.DiscountedPrice < 0)
            {
                validationErrorList.Add(new ValidationError("AccountCreditPlusConsumption", "DiscountedPrice", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Discounted Price"))));
            }
            if (accountCreditPlusConsumptionDTO.DiscountAmount.HasValue &&
                accountCreditPlusConsumptionDTO.DiscountAmount < 0)
            {
                validationErrorList.Add(new ValidationError("AccountCreditPlusConsumption", "DiscountAmount", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Discount Amount"))));
            }
            if (accountCreditPlusConsumptionDTO.ConsumptionBalance.HasValue &&
                accountCreditPlusConsumptionDTO.ConsumptionBalance < 0)
            {
                validationErrorList.Add(new ValidationError("AccountCreditPlusConsumption", "ConsumptionBalance", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Balance"))));
            }
            if (accountCreditPlusConsumptionDTO.QuantityLimit.HasValue &&
                accountCreditPlusConsumptionDTO.QuantityLimit < 0)
            {
                validationErrorList.Add(new ValidationError("AccountCreditPlusConsumption", "QuantityLimit", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Daily Limit"))));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        private void CreateRoamingData(int parentSiteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parentSiteId, sqlTransaction);
            if (parentSiteId > -1 && parentSiteId != accountCreditPlusConsumptionDTO.SiteId && executionContext.GetSiteId() > -1
                    && accountCreditPlusConsumptionDTO.AccountCreditPlusConsumptionId > -1)
            {
                DBSynchLogDTO dBSynchLogDTO = new DBSynchLogDTO("I", accountCreditPlusConsumptionDTO.Guid, "CardCreditPlusConsumption", DateTime.Now, parentSiteId);
                DBSynchLogBL dBSynchLogBL = new DBSynchLogBL(executionContext, dBSynchLogDTO);
                dBSynchLogBL.Save(sqlTransaction);
            }
            log.LogMethodExit();
        }
        
        /// <summary>
        /// Gets AccountCreditPlusSummaryBL
        /// </summary>
        /// <returns></returns>
        internal AccountCreditPlusConsumptionSummaryBL GetAccountCreditPlusConsumptionSummaryBL()
        {
            log.LogMethodEntry();
            string categoryName = string.Empty;
            string orderTypeName = string.Empty;
            string gameProfileName = string.Empty;
            string gameName = string.Empty;
            string productName = string.Empty;

            if (accountCreditPlusConsumptionDTO.CategoryId > -1)
            {
                CategoryContainerDTO categoryContainerDTO = CategoryContainerList.GetCategoryContainerDTOOrDefault(executionContext.GetSiteId(), accountCreditPlusConsumptionDTO.CategoryId);
                if (categoryContainerDTO != null)
                {
                    categoryName = categoryContainerDTO.Name;
                }
            }

            if (accountCreditPlusConsumptionDTO.OrderTypeId > -1)
            {
                OrderTypeContainerDTO orderTypeContainerDTO = OrderTypeContainerList.GetOrderTypeContainerDTOOrDefault(executionContext.GetSiteId(), accountCreditPlusConsumptionDTO.OrderTypeId);
                if(orderTypeContainerDTO != null)
                {
                    orderTypeName = orderTypeContainerDTO.Name;
                }
            }

            if (accountCreditPlusConsumptionDTO.GameProfileId > -1)
            {
                GameProfileContainerDTO gameProfileContainerDTO = GameProfileContainerList.GetGameProfileContainerDTOOrDefault(executionContext, accountCreditPlusConsumptionDTO.GameProfileId);
                if (gameProfileContainerDTO != null)
                {
                    gameProfileName = gameProfileContainerDTO.ProfileName;
                }
            }

            if (accountCreditPlusConsumptionDTO.GameId > -1)
            {
                GameContainerDTO gameContainerDTO = GameContainerList.GetGameContainerDTOOrDefault(executionContext, accountCreditPlusConsumptionDTO.GameId);
                if (gameContainerDTO != null)
                {
                    gameName = gameContainerDTO.GameName;
                }

            }

            if (accountCreditPlusConsumptionDTO.ProductId > -1)
            {
                ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetProductsContainerDTOOrDefault(executionContext, accountCreditPlusConsumptionDTO.ProductId);
                if(productsContainerDTO != null)
                {
                    productName = productsContainerDTO.ProductName;
                }

            }
            AccountCreditPlusConsumptionSummaryDTO accountCreditPlusConsumptionSummaryDTO = new AccountCreditPlusConsumptionSummaryDTO(accountCreditPlusConsumptionDTO.AccountCreditPlusConsumptionId,
                                                                                                                                        accountCreditPlusConsumptionDTO.AccountCreditPlusId,
                                                                                                                                        accountCreditPlusConsumptionDTO.POSTypeId,
                                                                                                                                        accountCreditPlusConsumptionDTO.ExpiryDate,
                                                                                                                                        accountCreditPlusConsumptionDTO.ProductId,
                                                                                                                                        productName,
                                                                                                                                        accountCreditPlusConsumptionDTO.GameId,
                                                                                                                                        gameName,
                                                                                                                                        accountCreditPlusConsumptionDTO.GameProfileId,
                                                                                                                                        gameProfileName,
                                                                                                                                        accountCreditPlusConsumptionDTO.DiscountPercentage,
                                                                                                                                        accountCreditPlusConsumptionDTO.DiscountedPrice,
                                                                                                                                        accountCreditPlusConsumptionDTO.ConsumptionBalance,
                                                                                                                                        accountCreditPlusConsumptionDTO.QuantityLimit,
                                                                                                                                        accountCreditPlusConsumptionDTO.CategoryId,
                                                                                                                                        categoryName,
                                                                                                                                        accountCreditPlusConsumptionDTO.DiscountAmount,
                                                                                                                                        accountCreditPlusConsumptionDTO.OrderTypeId,
                                                                                                                                        orderTypeName,
                                                                                                                                        accountCreditPlusConsumptionDTO.ConsumptionQty);
            AccountCreditPlusConsumptionSummaryBL accountCreditPlusConsumptionSummaryBL = new AccountCreditPlusConsumptionSummaryBL(executionContext, accountCreditPlusConsumptionSummaryDTO);
            log.LogMethodExit();
            return accountCreditPlusConsumptionSummaryBL;
        }
    }

    /// <summary>
    /// Manages the list of AccountCreditPlusConsumption
    /// </summary>
    public class AccountCreditPlusConsumptionListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public AccountCreditPlusConsumptionListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the AccountCreditPlusConsumption list
        /// </summary>
        public List<AccountCreditPlusConsumptionDTO> GetAccountCreditPlusConsumptionDTOList(List<KeyValuePair<AccountCreditPlusConsumptionDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            AccountCreditPlusConsumptionDataHandler accountCreditPlusConsumptionDataHandler = new AccountCreditPlusConsumptionDataHandler(sqlTransaction);
            List<AccountCreditPlusConsumptionDTO> returnValue = accountCreditPlusConsumptionDataHandler.GetAccountCreditPlusConsumptionDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Returns the AccountCreditPlusConsumption list
        /// </summary>
        public List<AccountCreditPlusConsumptionDTO> GetAccountCreditPlusConsumptionDTOListByAccountIds(List<int> accountIdList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(accountIdList, sqlTransaction);
            AccountCreditPlusConsumptionDataHandler accountCreditPlusConsumptionDataHandler = new AccountCreditPlusConsumptionDataHandler(sqlTransaction);
            List<AccountCreditPlusConsumptionDTO> returnValue = accountCreditPlusConsumptionDataHandler.GetAccountCreditPlusConsumptionDTOListByAccountIdList(accountIdList);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}
