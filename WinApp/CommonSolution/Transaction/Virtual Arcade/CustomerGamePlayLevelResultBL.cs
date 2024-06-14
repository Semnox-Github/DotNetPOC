/********************************************************************************************
 * Project Name - CustomerGamePlayLevelResultBL                                                                          
 * Description  - Business logic  class to manipulate game machine results level details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar    Created : Virtual Arcade changes
   *2.130.0    21-Oct-2021   Prajwal S         Modified : Virtual Arcade DbSynch log issues.
  *2.130.4    21-Feb-2022   Girish Kundar     Modified : Issue FIx - Card entitlement is in HOLD status, Auto load enable, Load tickets
 ********************************************************************************************/
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Game;
using Semnox.Parafait.Game.VirtualArcade;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.Product;
using Semnox.Parafait.DBSynch;
using Semnox.Parafait.Promotions;

namespace Semnox.Parafait.Transaction.VirtualArcade
{
    /// <summary>
    /// CustomerGamePlayLevelResultBL
    /// </summary>
    public class CustomerGamePlayLevelResultBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CustomerGamePlayLevelResultDTO customerGamePlayLevelResultDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        private CustomerGamePlayLevelResultBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// CustomerGamePlayLevelResultBL
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="parameterCustomerGamePlayLevelResultDTO"></param>
        /// <param name="sqlTransaction"></param>
        public CustomerGamePlayLevelResultBL(ExecutionContext executionContext, CustomerGamePlayLevelResultDTO parameterCustomerGamePlayLevelResultDTO, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parameterCustomerGamePlayLevelResultDTO, sqlTransaction);

            if (parameterCustomerGamePlayLevelResultDTO.CustomerGamePlayLevelResultId > -1)
            {
                LoadCustomerGamePlayLevelResultDTO(parameterCustomerGamePlayLevelResultDTO.CustomerGamePlayLevelResultId, sqlTransaction);//added sql
                ThrowIfUserDTOIsNull(parameterCustomerGamePlayLevelResultDTO.CustomerGamePlayLevelResultId);
                Update(parameterCustomerGamePlayLevelResultDTO);
            }
            else
            {
                Validate(sqlTransaction);
                if (parameterCustomerGamePlayLevelResultDTO.Points != null && parameterCustomerGamePlayLevelResultDTO.Points.Type == PointTypes.Point.ToString())
                {
                    parameterCustomerGamePlayLevelResultDTO.Score = parameterCustomerGamePlayLevelResultDTO.Points.Value;
                }
                else
                {
                    parameterCustomerGamePlayLevelResultDTO.Score = 0;
                }
                customerGamePlayLevelResultDTO = new CustomerGamePlayLevelResultDTO(-1, parameterCustomerGamePlayLevelResultDTO.GamePlayId,
                parameterCustomerGamePlayLevelResultDTO.GameMachineLevelId, parameterCustomerGamePlayLevelResultDTO.CustomerId, parameterCustomerGamePlayLevelResultDTO.Score,
                parameterCustomerGamePlayLevelResultDTO.CustomerXP, parameterCustomerGamePlayLevelResultDTO.Points, "", "", "", "", "", parameterCustomerGamePlayLevelResultDTO.IsActive);
            }
            log.LogMethodExit();
        }
        private void ThrowIfUserDTOIsNull(int id)
        {
            log.LogMethodEntry(id);
            if (CustomerGamePlayLevelResultDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "customerGamePlayLevelResult", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        private void LoadCustomerGamePlayLevelResultDTO(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, sqlTransaction);
            CustomerGamePlayLevelResultDataHandler customerGamePlayLevelResultDataHandler = new CustomerGamePlayLevelResultDataHandler(sqlTransaction);
            customerGamePlayLevelResultDTO = customerGamePlayLevelResultDataHandler.GetCustomerGamePlayLevelResultDTO(id);
            ThrowIfUserDTOIsNull(id);
            log.LogMethodExit();
        }

        private void Update(CustomerGamePlayLevelResultDTO parameterCustomerGamePlayLevelResultDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterCustomerGamePlayLevelResultDTO);
            customerGamePlayLevelResultDTO.GamePlayId = parameterCustomerGamePlayLevelResultDTO.GamePlayId;
            customerGamePlayLevelResultDTO.GameMachineLevelId = parameterCustomerGamePlayLevelResultDTO.GameMachineLevelId;
            customerGamePlayLevelResultDTO.CustomerId = parameterCustomerGamePlayLevelResultDTO.CustomerId;
            customerGamePlayLevelResultDTO.Score = parameterCustomerGamePlayLevelResultDTO.Score;
            customerGamePlayLevelResultDTO.CustomerXP = parameterCustomerGamePlayLevelResultDTO.CustomerXP;
            customerGamePlayLevelResultDTO.IsActive = parameterCustomerGamePlayLevelResultDTO.IsActive;
            log.LogMethodExit();
        }
        private void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            // Validation code here 
            // return validation exceptions
            log.LogMethodExit();
        }


        /// <summary>
        /// CustomerGamePlayLevelResultBL
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="sqlTransaction"></param>
        public CustomerGamePlayLevelResultBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id);
            LoadCustomerGamePlayLevelResultDTO(id, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Save
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction, executionContext);
            GameMachineLevelBL gameMachineLevelBL = new GameMachineLevelBL(executionContext, customerGamePlayLevelResultDTO.GameMachineLevelId, sqlTransaction);
            if (gameMachineLevelBL.GameMachineLevelDTO != null)
            {
                decimal? qualifyingScore = gameMachineLevelBL.GameMachineLevelDTO.QualifyingScore;
                if (qualifyingScore.HasValue && customerGamePlayLevelResultDTO.Points.Type == PointTypes.Point.ToString()
                     && customerGamePlayLevelResultDTO.Score < qualifyingScore)
                {
                    log.Debug("The score is less than the qualifying score set in the game level setup");
                    return;
                }
            }
            if (customerGamePlayLevelResultDTO.CustomerGamePlayLevelResultId < 0)
            {
                //insert the record only if the result for the game level is does not exists in the DB
                // If exits , check for the score , if score is less than existing then do not update the gameresult table
                //update only gameplayinfo and create creditplus lines
                // if score is greater than the existing then update records in the gameresult table and other tables
                CustomerGamePlayLevelResultListBL customerGamePlayLevelResultListBL = new CustomerGamePlayLevelResultListBL(executionContext);
                List<KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>(CustomerGamePlayLevelResultDTO.SearchByParameters.GAME_MACHINE_LEVEL_ID, customerGamePlayLevelResultDTO.GameMachineLevelId.ToString()));
                searchParameters.Add(new KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>(CustomerGamePlayLevelResultDTO.SearchByParameters.CUSTOMER_ID, customerGamePlayLevelResultDTO.CustomerId.ToString()));
                List<CustomerGamePlayLevelResultDTO> customerGamePlayLevelResultDTOList = customerGamePlayLevelResultListBL.GetCustomerGamePlayLevelResults(searchParameters, sqlTransaction);
                if (customerGamePlayLevelResultDTOList != null && customerGamePlayLevelResultDTOList.Any())
                {
                    //Update existing 
                    customerGamePlayLevelResultDTO.CustomerGamePlayLevelResultId = customerGamePlayLevelResultDTOList[0].CustomerGamePlayLevelResultId;
                    //Item count check
                    if (customerGamePlayLevelResultDTO.Points.Type == PointTypes.Item.ToString())
                    {
                        customerGamePlayLevelResultDTO.Score = customerGamePlayLevelResultDTOList[0].Score += 1;  // Increment the count by 1 . has won the one more product
                        SaveImpl(sqlTransaction);
                    }
                    //score check
                    // if greater then update gameresult - gameplayinfo/creditplus
                    // if less update gameplayinfo - creditplus
                    // Always there will be single record for each level for each customer
                    else if (customerGamePlayLevelResultDTO.Points.Type == PointTypes.Point.ToString() &&
                             customerGamePlayLevelResultDTO.Score > customerGamePlayLevelResultDTOList[0].Score)
                    {
                        SaveImpl(sqlTransaction);
                    }
                    else
                    {
                        //Update only the gameplay info and create the credit plusline with type VP
                        InsertGamePlayInfoDetails(sqlTransaction);
                        CreateCreditPlusLines(sqlTransaction);
                    }
                }
                else
                {
                    //Insert new record
                    if (customerGamePlayLevelResultDTO.Points.Type == "Item")
                    {
                        customerGamePlayLevelResultDTO.Score = 1;  // Set to 1 . Customer has won a product
                    }
                    SaveImpl(sqlTransaction);
                }

            }
            log.LogMethodExit();
        }
        private void SaveImpl(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            CustomerGamePlayLevelResultDataHandler customerGamePlayLevelResultDataHandler = new CustomerGamePlayLevelResultDataHandler(sqlTransaction);
            if (customerGamePlayLevelResultDTO.CustomerGamePlayLevelResultId < 0)
            {
                customerGamePlayLevelResultDTO = customerGamePlayLevelResultDataHandler.Insert(customerGamePlayLevelResultDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                customerGamePlayLevelResultDTO.AcceptChanges();
            }
            else
            {
                if (customerGamePlayLevelResultDTO.IsChanged)
                {
                    customerGamePlayLevelResultDTO = customerGamePlayLevelResultDataHandler.UpdateCustomerGamePlayLevelResultDTO(CustomerGamePlayLevelResultDTO);
                    customerGamePlayLevelResultDTO.AcceptChanges();
                }
            }
            InsertGamePlayInfoDetails(sqlTransaction);
            CreateCreditPlusLines(sqlTransaction);
            log.LogMethodExit();
        }

        private void CreateCreditPlusLines(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction, executionContext);
            decimal virtualLoyaltyPointsRatio = 1;
            decimal virtualPoints = 0;
            string message = "";
            Utilities parafaitUtility = GetUtility();
            GamePlayBL gamePlayBL = new GamePlayBL(executionContext, customerGamePlayLevelResultDTO.GamePlayId, sqlTransaction);
            GamePlayDTO gamePlayDTO = gamePlayBL.GamePlayDTO;
            ProductsList productList = new ProductsList();
            List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParamsforProducts = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
            Transaction transaction = new Transaction(parafaitUtility);
            if (customerGamePlayLevelResultDTO.Points.Type == PointTypes.Point.ToString())
            {
                try
                {
                    GameMachineLevelBL gameMachineLevelBL = new GameMachineLevelBL(executionContext, customerGamePlayLevelResultDTO.GameMachineLevelId, sqlTransaction);
                    virtualLoyaltyPointsRatio = gameMachineLevelBL.GetScoreToVirtualPointRatio();

                    log.Debug("virtualLoyaltyPointsRatio : " + virtualLoyaltyPointsRatio);
                    virtualPoints = customerGamePlayLevelResultDTO.Points.Value / virtualLoyaltyPointsRatio;
                    log.Debug("virtualPoints : " + virtualPoints);
                    searchParamsforProducts.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_EXACT_NAME, "VirtualPointProduct"));
                    searchParamsforProducts.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
                    List<ProductsDTO> productDTOList = productList.GetProductsDTOList(searchParamsforProducts, false, true, sqlTransaction);
                    log.LogVariableState("productDTOList", productDTOList);
                    if (productDTOList != null && productDTOList.Any())
                    {
                        Card card = new Card(gamePlayDTO.CardId, executionContext.GetUserId(), parafaitUtility, sqlTransaction);
                        AccountBL accountBL = new AccountBL(executionContext, gamePlayDTO.CardId, true, true, sqlTransaction);
                        Loyalty loyalty = new Loyalty(parafaitUtility);
                        log.LogVariableState("card", card);
                        log.Debug("last_update_time : " + card.last_update_time);
                        log.Debug("last_played_time : " + card.last_played_time);
                        log.Debug("transaction GameCardReadTime: " + transaction.GameCardReadTime);
                        log.Debug("transaction TransactionDate: " + transaction.TransactionDate);
                        int trxLineReturnvalue = transaction.createTransactionLine(card, productDTOList.FirstOrDefault().ProductId, 0, 1, ref message);
                        log.Debug("trxLineReturnvalue : " + trxLineReturnvalue);
                        if (trxLineReturnvalue == 0)
                        {
                            transaction.SaveOrder(ref message, sqlTransaction);
                            accountBL = new AccountBL(executionContext, gamePlayDTO.CardId, true, true, sqlTransaction);
                            if (accountBL.AccountDTO != null &&
                                             (accountBL.AccountDTO.AccountCreditPlusDTOList != null &&
                                                accountBL.AccountDTO.AccountCreditPlusDTOList.Any())
                                            ||
                                             (accountBL.AccountDTO.AccountCreditPlusDTOList == null
                                                        || (accountBL.AccountDTO.AccountCreditPlusDTOList != null
                                                             && accountBL.AccountDTO.AccountCreditPlusDTOList.Count == 0))
                               )
                            {
                                AccountCreditPlusDTO creditPlusDTO = null;

                                if (accountBL.AccountDTO.AccountCreditPlusDTOList != null)
                                {
                                    creditPlusDTO = accountBL.AccountDTO.AccountCreditPlusDTOList.Where
                                                                   (x => x.CreditPlusType == CreditPlusType.VIRTUAL_POINT &&
                                                                    x.CreationDate.Date == ServerDateTime.Now.Date).FirstOrDefault();
                                }
                                log.Debug("creditPlusDTO : " + creditPlusDTO);
                                if (creditPlusDTO != null)
                                {
                                    log.Debug("Credit plus of type virtual is already created for this day");
                                    log.Debug("creditPlusDTO : " + creditPlusDTO);
                                    log.Debug("creditPlusDTO  CreditPlus: " + creditPlusDTO.CreditPlus);
                                    creditPlusDTO.CreditPlus += virtualPoints;
                                    log.Debug("creditPlusDTO CreditPlus after adding v points : " + creditPlusDTO.CreditPlus);
                                    log.Debug("creditPlusDTO CreditPlusBalance before adding v points : " + creditPlusDTO.CreditPlusBalance);
                                    creditPlusDTO.CreditPlusBalance += virtualPoints;
                                    log.Debug("creditPlusDTO  CreditPlusBalance after adding v points : " + creditPlusDTO.CreditPlusBalance);
                                    int index = accountBL.AccountDTO.AccountCreditPlusDTOList.IndexOf(creditPlusDTO);
                                    log.Debug("creditPlusDTO  index : " + index);
                                    accountBL.AccountDTO.AccountCreditPlusDTOList[index] = creditPlusDTO;
                                    accountBL.Save(sqlTransaction);
                                }
                                else //  Create new credit plus line
                                {
                                var trxLine = transaction.TrxLines.Where(x => x.ProductID == productDTOList.FirstOrDefault().ProductId).FirstOrDefault();
                                int lineId = -1;
                                if (trxLine != null)
                                {
                                    lineId = trxLine.DBLineId;
                                }
                                int creditPlusId = loyalty.CreateGenericCreditPlusLine(card.card_id, CreditPlusTypeConverter.ToString(CreditPlusType.VIRTUAL_POINT),
                                                                                            (double)virtualPoints, true, 0, "N", parafaitUtility.ParafaitEnv.LoginID,
                                                                                            "Virtual Loyalty points", sqlTransaction, null, transaction.Trx_id, lineId, false);
                                log.Debug("creditPlusId :" + creditPlusId);
                                AccountCreditPlusBL accountCreditPlusBL = null;
                                if (creditPlusId > -1)
                                {
                                    accountCreditPlusBL = new AccountCreditPlusBL(executionContext, creditPlusId, true, true, sqlTransaction);
                                }
                                log.Debug("AccountDTO DBSYnch ");
                                    if (accountBL.AccountDTO.SiteId != executionContext.GetSiteId() && accountCreditPlusBL != null)
                                    {
                                        CreateDBSynchLogs(sqlTransaction, accountBL.AccountDTO, accountCreditPlusBL.AccountCreditPlusDTO);
                                    }
                                }
                            }
                            transaction.GameCardReadTime = ServerDateTime.Now;
                            int trxReturnvalue = transaction.SaveTransacation(sqlTransaction, ref message);
                            log.Debug("trxReturnvalue : " + trxReturnvalue);
                            if (trxReturnvalue != 0)
                            {
                                log.Error("Error while creating transaction ");
                                throw new Exception(message);
                            }
                            AutoLoadEntitlement(transaction.Trx_id, card, virtualPoints, sqlTransaction);
                        }
                        else
                        {
                            log.Error("Error while creating CreateCreditPlusLines");
                            throw new Exception(message);
                        }
                    }
                    else
                    {
                        log.Error("VirtualPointProduct is not set , Failed to create transaction");
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.Error("Error while creating transaction line");
                    throw new Exception(message);
                }
            }
            else if (customerGamePlayLevelResultDTO.Points.Type == PointTypes.Item.ToString())
            {
                try
                {
                    searchParamsforProducts.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
                    searchParamsforProducts.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_EXACT_NAME, "VirtualArcadeDefaultProduct"));
                    List<ProductsDTO> productDTOList = productList.GetProductsDTOList(searchParamsforProducts);
                    log.LogVariableState("productDTOList", productDTOList);
                    ProductBL product = new ProductBL(executionContext, customerGamePlayLevelResultDTO.Points.Value, false, false, sqlTransaction);
                    ProductDTO productDTO = product.getProductDTO;
                    Card card = new Card(gamePlayDTO.CardId, executionContext.GetUserId(), parafaitUtility);
                    log.LogVariableState("card", card);
                    log.Debug("last_update_time : " + card.last_update_time);
                    log.Debug("last_played_time : " + card.last_played_time);
                    log.Debug("transaction GameCardReadTime: " + transaction.GameCardReadTime);
                    log.Debug("transaction TransactionDate: " + transaction.TransactionDate);

                    int trxLineReturnvalue = transaction.createTransactionLine(card, productDTOList[0].ProductId, 0, 1, ref message);
                    log.Debug("trxLineReturnvalue : " + trxLineReturnvalue);
                    if (trxLineReturnvalue == 0)
                    {
                        transaction.GameCardReadTime = ServerDateTime.Now;
                        int trxReturnvalue = transaction.SaveTransacation(sqlTransaction, ref message);
                        log.Debug("trxReturnvalue : " + trxReturnvalue);
                        if (trxReturnvalue != 0)
                        {
                            log.Error("Error while creating transaction");
                            throw new Exception(message);
                        }
                        if (card.siteId != executionContext.GetSiteId())
                        {
                            AccountBL accountBL = new AccountBL(executionContext, card.card_id, true, true, sqlTransaction);
                            if (accountBL.AccountDTO.AccountCreditPlusDTOList != null && accountBL.AccountDTO.AccountCreditPlusDTOList.Any())
                            {
                                List<AccountCreditPlusDTO> accountCreditPlusDTOList = accountBL.AccountDTO.AccountCreditPlusDTOList.Where(c => c.TransactionId == transaction.Trx_id && c.CreditPlusType == CreditPlusType.COUNTER_ITEM).ToList();
                                if (accountCreditPlusDTOList != null && accountCreditPlusDTOList.Any())
                                {
                                    AccountCreditPlusDTO accountCreditPlusDTO = accountCreditPlusDTOList.OrderByDescending(x => x.AccountCreditPlusId).FirstOrDefault();
                                    CreateDBSynchLogs(sqlTransaction, accountBL.AccountDTO, accountCreditPlusDTO);
                                }
                            }
                        }
                    }
                    else
                    {
                        log.Error("Error while creating transaction line");
                        throw new Exception(message);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw ex;
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// This method loads the entitlelment to the card based on the configurations 
        /// The entitlement type may be set as Tickets and credit, bonus etc
        /// </summary>
        /// <param name="card"></param>
        /// <param name="virtualPoints"></param>
        /// <param name="sqlTransaction"></param>
        /// </summary>
        private void AutoLoadEntitlement(int transactionId, Card card, decimal virtualPoints, SqlTransaction sqlTransaction)
        {
            try
            {
                log.LogMethodEntry(transactionId, card, virtualPoints, sqlTransaction);
                string message = string.Empty;
                string creditPlusType = string.Empty;
                Utilities utilities = GetUtility();
                GameMachineLevelBL gameMachineLevelBL = new GameMachineLevelBL(executionContext, customerGamePlayLevelResultDTO.GameMachineLevelId);
                if (gameMachineLevelBL.GameMachineLevelDTO != null && gameMachineLevelBL.GameMachineLevelDTO.AutoLoadEntitlement)
                {
                    log.Debug(" AutoLoad is set as true");
                    if (string.IsNullOrWhiteSpace(gameMachineLevelBL.GameMachineLevelDTO.EntitlementType) == false)
                    {
                        log.Debug("Entitlement Type   : " + gameMachineLevelBL.GameMachineLevelDTO.EntitlementType);
                        LoyaltyRedemptionRuleListBL loyaltyRedemptionRuleListBL = new LoyaltyRedemptionRuleListBL(executionContext);
                        switch (gameMachineLevelBL.GameMachineLevelDTO.EntitlementType.ToLower())
                        {
                            case "credits":
                                {
                                    creditPlusType = CreditPlusTypeConverter.ToString(CreditPlusType.CARD_BALANCE);
                                    break;
                                }
                            case "Bonus":
                            case "bonus":
                                {
                                    creditPlusType = CreditPlusTypeConverter.ToString(CreditPlusType.GAME_PLAY_BONUS);
                                    break;
                                }
                            case "time":
                                {
                                    creditPlusType = CreditPlusTypeConverter.ToString(CreditPlusType.TIME);
                                    break;
                                }
                            case "tickets":
                                {
                                    creditPlusType = CreditPlusTypeConverter.ToString(CreditPlusType.TICKET);
                                    break;
                                }
                            default:
                                {
                                    creditPlusType = CreditPlusTypeConverter.ToString(CreditPlusType.CARD_BALANCE);
                                    break;
                                }
                        }
                        decimal convertedvalue  = loyaltyRedemptionRuleListBL.GetRedemptionValueForEntilementType(virtualPoints, creditPlusType, true, sqlTransaction);
                        Loyalty loyalty = new Loyalty(utilities);
                        TaskProcs taskProcs = new TaskProcs(utilities);
                        loyalty.RedeemVirtualPoints(card.card_id, card.CardNumber, Convert.ToDouble(virtualPoints), gameMachineLevelBL.GameMachineLevelDTO.EntitlementType.ToLower(), Convert.ToDouble(convertedvalue), utilities.ParafaitEnv.POSMachine, utilities.ParafaitEnv.User_Id, sqlTransaction);
                        taskProcs.createTask(card.card_id, TaskProcs.REDEEMVIRTUALPOINTS, Convert.ToDouble(virtualPoints), -1, -1, -1, -1, creditPlusType[0], -1, "Auto Load", sqlTransaction, -1, -1, -1, -1, transactionId, Convert.ToDecimal(-1 * virtualPoints));
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private void CreateDBSynchLogs(SqlTransaction sqlTransaction, AccountDTO accountDTO, AccountCreditPlusDTO accountCreditPlusDTO)
        {
            try
            {
                log.LogMethodEntry(sqlTransaction, accountDTO, accountCreditPlusDTO);
                DBSynchLogService dBSynchLogService = new DBSynchLogService(executionContext, "Cards", accountDTO.Guid, accountDTO.SiteId);
                dBSynchLogService.CreateRoamingData(sqlTransaction);
                log.Debug("AccountCreditPlusDTO DBSYnch ");
                if (accountCreditPlusDTO != null)
                {
                    dBSynchLogService = new DBSynchLogService(executionContext, "CardCreditPlus", accountCreditPlusDTO.Guid, accountCreditPlusDTO.SiteId);
                    dBSynchLogService.CreateRoamingData(sqlTransaction);
                    if (accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList != null && accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList.Any())
                    {
                        foreach (AccountCreditPlusConsumptionDTO accountCreditPlusConsumptionDTO in accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList)
                        {
                            dBSynchLogService = new DBSynchLogService(executionContext, "CardCreditPlusConsumption", accountCreditPlusConsumptionDTO.Guid, accountCreditPlusConsumptionDTO.SiteId);
                            dBSynchLogService.CreateRoamingData(sqlTransaction);
                        }
                    }
                    if (accountCreditPlusDTO.AccountCreditPlusPurchaseCriteriaDTOList != null && accountCreditPlusDTO.AccountCreditPlusPurchaseCriteriaDTOList.Any())
                    {
                        foreach (AccountCreditPlusPurchaseCriteriaDTO accountCreditPlusPurchaseCriteriaDTO in accountCreditPlusDTO.AccountCreditPlusPurchaseCriteriaDTOList)
                        {
                            dBSynchLogService = new DBSynchLogService(executionContext, "CardCreditPlusPurchaseCriteria", accountCreditPlusPurchaseCriteriaDTO.Guid, accountCreditPlusPurchaseCriteriaDTO.SiteId);
                            dBSynchLogService.CreateRoamingData(sqlTransaction);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit();
        }

        internal Utilities GetUtility()
        {
            log.LogMethodEntry();
            Utilities utilities = new Utilities();
            utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
            utilities.ParafaitEnv.POSMachineId = executionContext.GetMachineId();
            POSMachineContainerDTO pOSMachineContainerDTO = POSMachineContainerList.GetPOSMachineContainerDTOOrDefault(executionContext.GetSiteId(), "VirtualArcade", "", -1);
            if (pOSMachineContainerDTO != null)
            {
                utilities.ParafaitEnv.SetPOSMachine("", pOSMachineContainerDTO.POSName);
            }
            else
            {
                utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
            }
            utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
            log.Debug("executionContext - siteId" + executionContext.GetSiteId());
            utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
            utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
            utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
            UserContainerDTO user = UserContainerList.GetUserContainerDTOOrDefault(executionContext.GetUserId(), "", executionContext.GetSiteId());
            utilities.ParafaitEnv.User_Id = user.UserId;
            utilities.ParafaitEnv.RoleId = user.RoleId;
            utilities.ExecutionContext.SetUserId(user.LoginId);
            utilities.ParafaitEnv.Initialize();
            log.LogMethodExit(utilities);
            return utilities;
        }

        private void InsertGamePlayInfoDetails(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            GamePlayInfoDTO gamePlayInfoDTO = new GamePlayInfoDTO(-1, customerGamePlayLevelResultDTO.GamePlayId, "N", DateTime.MinValue, -1, DateTime.MinValue,
                                                                 -1, -1, customerGamePlayLevelResultDTO.Points.Value.ToString(), "VirtualPlay result post",
                                                                  customerGamePlayLevelResultDTO.CustomerGamePlayLevelResultId, customerGamePlayLevelResultDTO.Points.Type,//Attribute 1 specifies whether the score is a point or an item
                                                                  customerGamePlayLevelResultDTO.Attribute2, customerGamePlayLevelResultDTO.Attribute3,
                                                                  customerGamePlayLevelResultDTO.Attribute4, customerGamePlayLevelResultDTO.Attribute5);
            GamePlayInfoBL gamePlayInfoBL = new GamePlayInfoBL(executionContext, gamePlayInfoDTO);
            log.Debug(gamePlayInfoDTO);
            gamePlayInfoBL.Save(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// get CustomerGamePlayLevelResultDTO Object
        /// </summary>
        public CustomerGamePlayLevelResultDTO CustomerGamePlayLevelResultDTO
        {
            get
            {
                CustomerGamePlayLevelResultDTO result = new CustomerGamePlayLevelResultDTO(customerGamePlayLevelResultDTO);
                return result;
            }
        }

    }

    /// <summary>
    /// CustomerGamePlayLevelResultListBL
    /// </summary>
    public class CustomerGamePlayLevelResultListBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<CustomerGamePlayLevelResultDTO> customerGamePlayLevelResultDTOList;

        /// <summary>
        /// default constructor
        /// </summary>
        public CustomerGamePlayLevelResultListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public CustomerGamePlayLevelResultListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.customerGamePlayLevelResultDTOList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// CustomerGamePlayLevelResultListBL
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="customerGamePlayLevelResultDTOList"></param>
        public CustomerGamePlayLevelResultListBL(ExecutionContext executionContext, List<CustomerGamePlayLevelResultDTO> customerGamePlayLevelResultDTOList)
        {
            log.LogMethodEntry(executionContext, customerGamePlayLevelResultDTOList);
            this.customerGamePlayLevelResultDTOList = customerGamePlayLevelResultDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetCustomerGamePlayLevelResults
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<CustomerGamePlayLevelResultDTO> GetCustomerGamePlayLevelResults(List<KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>> searchParameters,
                                           SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CustomerGamePlayLevelResultDataHandler customerGamePlayLevelResultDataHandler = new CustomerGamePlayLevelResultDataHandler(sqlTransaction);
            List<CustomerGamePlayLevelResultDTO> customerGamePlayLevelResultDTOList = customerGamePlayLevelResultDataHandler.GetCustomerGamePlayLevelResults(searchParameters);
            log.LogMethodExit(customerGamePlayLevelResultDTOList);
            return customerGamePlayLevelResultDTOList;
        }


        /// <summary>
        /// GetCustomerGamePlayWinnings
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<GamePlayWinningsDTO> GetCustomerGamePlayWinnings(int customerId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customerId, sqlTransaction);
            List<GamePlayWinningsDTO> result = new List<GamePlayWinningsDTO>();
            List<KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>(CustomerGamePlayLevelResultDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>(CustomerGamePlayLevelResultDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
            CustomerGamePlayLevelResultDataHandler customerGamePlayLevelResultDataHandler = new CustomerGamePlayLevelResultDataHandler(sqlTransaction);
            List<CustomerGamePlayLevelResultDTO> customerGamePlayLevelResultDTOList = customerGamePlayLevelResultDataHandler.GetCustomerGamePlayLevelResults(searchParameters);
            if (customerGamePlayLevelResultDTOList != null && customerGamePlayLevelResultDTOList.Any())
            {
                // Get Card Info ,Customer Info and level Info
                foreach (CustomerGamePlayLevelResultDTO customerGamePlayLevelResultDTO in customerGamePlayLevelResultDTOList)
                {
                    GamePlayWinningDetailDTO gamePlayWinningDetailDTO = null;
                    GamePlayBL gamePlayBL = new GamePlayBL(executionContext, customerGamePlayLevelResultDTO.GamePlayId);
                    GamePlayDTO gamePlayDTO = gamePlayBL.GamePlayDTO;
                    log.LogVariableState("gamePlayDTO", gamePlayDTO);

                    AccountBL accountBL = new AccountBL(executionContext, gamePlayDTO.CardId, true, true);
                    AccountDTO accountDTO = accountBL.AccountDTO;
                    log.LogVariableState("accountDTO", accountDTO);

                    GameMachineLevelBL gameMachineLevelBL = new GameMachineLevelBL(executionContext, customerGamePlayLevelResultDTO.GameMachineLevelId);
                    GamePlayWinningsDTO gamePlayWinningsDTO = new GamePlayWinningsDTO(accountDTO.TagNumber, gameMachineLevelBL.GameMachineLevelDTO.LevelName, customerGamePlayLevelResultDTO.GameMachineLevelId, Decimal.Truncate(customerGamePlayLevelResultDTO.CustomerXP).ToString());
                    log.LogVariableState("gamePlayWinningsDTO", gamePlayWinningsDTO);


                    gamePlayWinningsDTO.Winnings = new List<GamePlayWinningDetailDTO>();
                    //AccountCreditPlusDTO creditPlusDTO = accountDTO.AccountCreditPlusDTOList.OrderByDescending(x => x.AccountCreditPlusId).FirstOrDefault();
                    GamePlayInfoList gamePlayInfoListBL = new GamePlayInfoList(executionContext);
                    List<KeyValuePair<GamePlayInfoDTO.SearchByParameters, string>> searchParametersofGamePlayInfo = new List<KeyValuePair<GamePlayInfoDTO.SearchByParameters, string>>();
                    searchParametersofGamePlayInfo.Add(new KeyValuePair<GamePlayInfoDTO.SearchByParameters, string>(GamePlayInfoDTO.SearchByParameters.CUSTOMER_GAME_PLAY_LEVEL_RESULT_ID, customerGamePlayLevelResultDTO.CustomerGamePlayLevelResultId.ToString()));
                    List<GamePlayInfoDTO> gamePlayInfoDTOList = gamePlayInfoListBL.GetGamePlayInfoDTOList(searchParametersofGamePlayInfo);
                    log.LogVariableState("gamePlayInfoDTOList", gamePlayInfoDTOList);

                    if (gamePlayInfoDTOList != null && gamePlayInfoDTOList.Any())
                    {
                        if (gamePlayInfoDTOList[0].Attribute1 == "Point")
                        {
                            decimal virtualLoyaltyPointsRatio = 1;
                            decimal virtualPoints;
                            decimal sumOfVP = gamePlayInfoDTOList.Sum(x => Convert.ToDecimal(x.GamePlayData));

                            log.Debug("sumOfVP : " + sumOfVP);
                            log.Debug("Level  : " + customerGamePlayLevelResultDTO.GameMachineLevelId);

                            virtualLoyaltyPointsRatio = gameMachineLevelBL.GetScoreToVirtualPointRatio();
                            virtualPoints = sumOfVP / virtualLoyaltyPointsRatio;
                            gamePlayWinningDetailDTO = new GamePlayWinningDetailDTO("Virtual Points", virtualPoints);
                        }
                        else
                        {
                            gamePlayWinningDetailDTO = new GamePlayWinningDetailDTO("ItemWon", (decimal)customerGamePlayLevelResultDTO.Score);
                        }
                    }
                    else
                    {
                        gamePlayWinningDetailDTO = new GamePlayWinningDetailDTO("Virtual Points", 0);
                    }
                    gamePlayWinningsDTO.Winnings.Add(gamePlayWinningDetailDTO);
                    result.Add(gamePlayWinningsDTO);
                    log.LogVariableState("result", result);
                }
            }
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// GetLeaderBoard
        /// </summary>
        /// <param name="gameMachineLevelId"></param>
        /// <returns></returns>
        public List<LeaderBoardDTO> GetLeaderBoard(int gameMachineLevelId)
        {
            log.LogMethodEntry(gameMachineLevelId);
            List<CustomerGamePlayLevelResultDTO> leaderList;
            List<LeaderBoardDTO> result = new List<LeaderBoardDTO>();

            GameMachineLevelBL gameMachineLevelBL = new GameMachineLevelBL(executionContext, gameMachineLevelId);
            GameMachineLevelDTO gameMachineLevelDTO = gameMachineLevelBL.GameMachineLevelDTO;
            log.LogVariableState("gameMachineLevelDTO", gameMachineLevelDTO);

            Machine machine = new Machine(gameMachineLevelDTO.MachineId, executionContext);
            MachineDTO machineDTO = machine.GetMachineDTO;
            log.LogVariableState("machineDTO", machineDTO);

            Semnox.Parafait.Game.Game game = new Semnox.Parafait.Game.Game(machineDTO.GameId, executionContext);
            GameDTO gameDTO = game.GetGameDTO;
            log.LogVariableState("gameDTO", gameDTO);

            List<KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>(CustomerGamePlayLevelResultDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>(CustomerGamePlayLevelResultDTO.SearchByParameters.GAME_MACHINE_LEVEL_ID, gameMachineLevelId.ToString()));
            CustomerGamePlayLevelResultDataHandler customerGamePlayLevelResultDataHandler = new CustomerGamePlayLevelResultDataHandler(null);
            List<CustomerGamePlayLevelResultDTO> customerGamePlayLevelResultDTOList = customerGamePlayLevelResultDataHandler.GetCustomerGamePlayLevelResults(searchParameters);
            log.LogVariableState("customerGamePlayLevelResultDTOList", customerGamePlayLevelResultDTOList);

            if (customerGamePlayLevelResultDTOList != null && customerGamePlayLevelResultDTOList.Any())
            {
                decimal score = customerGamePlayLevelResultDTOList.FirstOrDefault().Score;
                log.LogVariableState("score", score);

                List<CustomerGamePlayLevelResultDTO> list = customerGamePlayLevelResultDTOList.OrderByDescending(x => x.Score).ToList();
                leaderList = list.Take(20).ToList();
                log.LogVariableState("leaderList", leaderList);

                int rank = 1;
                string base64ImageRepresentation = string.Empty;
                foreach (CustomerGamePlayLevelResultDTO customerGamePlayLevelResultDTO in leaderList)
                {
                    CustomerBL customerBL = new CustomerBL(executionContext, customerGamePlayLevelResultDTO.CustomerId);
                    CustomerDTO customerDTO = customerBL.CustomerDTO;
                    log.LogVariableState("customerDTO", customerDTO);

                    if (string.IsNullOrWhiteSpace(customerDTO.PhotoURL) == false)
                    {
                        byte[] imageArray = System.IO.File.ReadAllBytes(customerDTO.PhotoURL);
                        base64ImageRepresentation = Convert.ToBase64String(imageArray);
                        log.LogVariableState("base64ImageRepresentation", base64ImageRepresentation);
                    }
                    result.Add(new LeaderBoardDTO(customerDTO.FirstName, base64ImageRepresentation, rank++, gameDTO.GameName, gameMachineLevelDTO.LevelName, customerGamePlayLevelResultDTO.Score));
                }
            }
            log.LogVariableState("result", result);
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// Save
        /// </summary>
        /// <returns></returns>
        public List<GamePlayWinningsDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<GamePlayWinningsDTO> gamePlayWinningsDTOList = new List<GamePlayWinningsDTO>();
            try
            {
                if (customerGamePlayLevelResultDTOList != null && customerGamePlayLevelResultDTOList.Any())
                {
                    string tagNumber = "";
                    foreach (CustomerGamePlayLevelResultDTO customerGamePlayLevelResultDTO in customerGamePlayLevelResultDTOList)
                    {
                        CustomerGamePlayLevelResultBL customerGamePlayLevelResultBL = new CustomerGamePlayLevelResultBL(executionContext, customerGamePlayLevelResultDTO, sqlTransaction);
                        customerGamePlayLevelResultBL.Save(sqlTransaction);
                        CustomerGamePlayLevelResultDTO result = customerGamePlayLevelResultBL.CustomerGamePlayLevelResultDTO;
                        log.LogVariableState("CustomerGamePlayLevelResultDTO", result);

                        List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<GamePlayDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.GAME_PLAY_ID, result.GamePlayId.ToString()));
                        GamePlayListBL gamePlayBL = new GamePlayListBL(executionContext);
                        List<GamePlayDTO> gamePlayDTOList = gamePlayBL.GetGamePlayDTOListWithTagNumber(searchParameters, sqlTransaction);
                        log.LogVariableState("gamePlayDTOList", gamePlayDTOList);

                        if (gamePlayDTOList != null && gamePlayDTOList.Any())
                        {
                            tagNumber = gamePlayDTOList.FirstOrDefault().CardNumber;
                            log.LogVariableState("tagNumber", tagNumber);
                        }
                        GameMachineLevelBL gameMachineLevelBL = new GameMachineLevelBL(executionContext, result.GameMachineLevelId);
                        GamePlayWinningsDTO gamePlayWinningsDTO = new GamePlayWinningsDTO(tagNumber, gameMachineLevelBL.GameMachineLevelDTO.LevelName, result.GameMachineLevelId, Decimal.Truncate(result.CustomerXP).ToString());
                        GamePlayWinningDetailDTO gamePlayWinningDetailDTO = new GamePlayWinningDetailDTO();

                        if (customerGamePlayLevelResultDTO.Points.Type == PointTypes.Point.ToString())
                        {
                            if (result.CustomerGamePlayLevelResultId != -1) // Created creditplus line
                            {
                                decimal virtualLoyaltyPointsRatio = gameMachineLevelBL.GetScoreToVirtualPointRatio();
                                log.Debug("virtualLoyaltyPointsRatio : " + virtualLoyaltyPointsRatio);
                                decimal virtualPoints = result.Score / virtualLoyaltyPointsRatio;
                                log.Debug("virtualPoints : " + virtualPoints);
                                gamePlayWinningDetailDTO = new GamePlayWinningDetailDTO("Virtual Points", virtualPoints);
                                log.LogVariableState("gamePlayWinningDetailDTO", gamePlayWinningDetailDTO);
                            }
                            else
                            {
                                gamePlayWinningDetailDTO = new GamePlayWinningDetailDTO("Virtual Points", 0);
                            }

                        }
                        else if (customerGamePlayLevelResultDTO.Points.Type == PointTypes.Item.ToString())
                        {
                            gamePlayWinningDetailDTO = new GamePlayWinningDetailDTO("ItemWon", (decimal)result.Score);
                        }
                        log.LogVariableState("gamePlayWinningDetailDTO", gamePlayWinningDetailDTO);
                        gamePlayWinningsDTO.Winnings = new List<GamePlayWinningDetailDTO>();
                        gamePlayWinningsDTO.Winnings.Add(gamePlayWinningDetailDTO);
                        gamePlayWinningsDTOList.Add(gamePlayWinningsDTO);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                log.Error(sqlEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                if (sqlEx.Number == 2601)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                }
                else
                {
                    throw;
                }
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }

            log.LogMethodExit(gamePlayWinningsDTOList);
            return gamePlayWinningsDTOList;
        }

    }
}
