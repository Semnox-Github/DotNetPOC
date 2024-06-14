/*/********************************************************************************************
 * Project Name - POS
 * Description  - Business logic File for LegacyCard
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks 
 *********************************************************************************************
 *2.70.3      10-June-2019  Girish Kundar            Created 
 *2.100.0     03-Sep-2020   Dakshakh                 Modified 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Game;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Parafait.POS;
using Semnox.Parafait.Product;
using Semnox.Parafait.Category;
using Semnox.Parafait.Discounts;
using Semnox.Core.GenericUtilities;

namespace Parafait_POS
{
    /// <summary>
    /// Class LegacyCardBL
    /// </summary>
    public class LegacyCardBL
    {
        private LegacyCardDTO legacyCardDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        Utilities Utilities;
        List<LookupValuesDTO> lookupValuesDTOList;
        private AccountDTO accountDTO;
        /// <summary>
        /// Default constructor of LegacyCardBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private LegacyCardBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates LegacyCardBL object using the LegacyCardDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="legacyCardDTO">legacyCardDTO object</param>
        /// <param name="utilities">Utilities object</param>
        public LegacyCardBL(ExecutionContext executionContext, LegacyCardDTO legacyCardDTO, Utilities utilities)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, legacyCardDTO, accountDTO, utilities);
            this.legacyCardDTO = legacyCardDTO;
            this.Utilities = utilities;
        }

        /// <summary>
        /// get LegacyCardDTO Object
        /// </summary>
        public LegacyCardDTO GetLegacyCardDTO
        {
            get { return legacyCardDTO; }
        }

        /// <summary>
        /// Get Legacy Card Details
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public LegacyCardDTO GetLegacyCardDetails(bool loadChildRecords = true)
        {
            log.LogMethodEntry();
            ExternalCardSystem externalCardSystem = ExternalCardSystemFactory.GetInstance(legacyCardDTO).GetExternalCardSystem(executionContext);
            if (externalCardSystem == null)
            {
                List<KeyValuePair<LegacyCardDTO.SearchByParameters, string>> SearchByParameters = new List<KeyValuePair<LegacyCardDTO.SearchByParameters, string>>();
                SearchByParameters.Add(new KeyValuePair<LegacyCardDTO.SearchByParameters, string>(LegacyCardDTO.SearchByParameters.CARD_NUMBER_OR_PRINTED_CARD_NUMBER, legacyCardDTO.CardNumber));
                List<LegacyCardDTO> legacyCardDTOList = new List<LegacyCardDTO>();
                LegacyCardDataHandler legacyCardDataHandler = new LegacyCardDataHandler();
                legacyCardDTOList = legacyCardDataHandler.GetLegacyCardDTO(SearchByParameters);
                if (legacyCardDTOList != null && legacyCardDTOList.Any() && loadChildRecords)
                {
                    LegacyCardBuilderBL legacyCardBuilderBL = new LegacyCardBuilderBL(executionContext, legacyCardDTOList);
                    legacyCardDTOList = legacyCardBuilderBL.Build(true);
                    this.legacyCardDTO = legacyCardDTOList[0];
                }
                else
                {
                    legacyCardDTO = null;
                }
            }
            else
            {
                externalCardSystem.Initialize();
                legacyCardDTO = externalCardSystem.GetCardInformation();
                if (isClubSpeed())
                {
                    List<ValidationError> validationErrorList = ValidateExternalCards(legacyCardDTO);
                    if (validationErrorList != null & validationErrorList.Count > 0)
                    {
                        throw new ValidationException("Validation Failed", validationErrorList);
                    }
                }

            }
            log.LogMethodExit(legacyCardDTO);
            return legacyCardDTO;
        }

        /// <summary>
        /// Saves the LegacyCard
        /// Checks if the LegacyCardId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (legacyCardDTO.ParafaitCardNumber != null)
            {
                SaveLegacyTransaction(sqlTransaction);
            }
            LegacyCardDataHandler legacyCardDataHandler = new LegacyCardDataHandler(sqlTransaction);
            if (legacyCardDTO.CardId < 0)
            {
                legacyCardDTO = legacyCardDataHandler.Insert(legacyCardDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                legacyCardDTO.AcceptChanges();
            }
            else
            {
                legacyCardDTO = legacyCardDataHandler.Update(legacyCardDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                legacyCardDTO.AcceptChanges();
            }
            if (legacyCardDTO.LegacyCardCreditPlusDTOList != null && legacyCardDTO.LegacyCardCreditPlusDTOList.Any())
            {
                foreach (var legacyCardCreditPlusDTO in legacyCardDTO.LegacyCardCreditPlusDTOList)
                {
                    if (legacyCardCreditPlusDTO.IsChanged)
                    {
                        LegacyCardCreditPlusBL legacyCardCreditPlusBL = new LegacyCardCreditPlusBL(executionContext, legacyCardCreditPlusDTO);
                        legacyCardCreditPlusBL.Save(sqlTransaction);
                    }
                }
            }
            if (legacyCardDTO.LegacyCardGamesDTOsList != null && legacyCardDTO.LegacyCardGamesDTOsList.Any())
            {
                foreach (var legacyCardGamesDTO in legacyCardDTO.LegacyCardGamesDTOsList)
                {
                    if (legacyCardGamesDTO.IsChanged)
                    {
                        LegacyCardGamesBL legacyCardGamesBL = new LegacyCardGamesBL(executionContext, legacyCardGamesDTO);
                        legacyCardGamesBL.Save(sqlTransaction);
                    }
                }
            }
            if (legacyCardDTO.LegacyCardDiscountsDTOList != null && legacyCardDTO.LegacyCardDiscountsDTOList.Any())
            {
                foreach (var legacyCardDiscountDTO in legacyCardDTO.LegacyCardDiscountsDTOList)
                {
                    if (legacyCardDiscountDTO.IsChanged)
                    {
                        LegacyCardDiscountsBL legacyCardDiscountsBL = new LegacyCardDiscountsBL(executionContext, legacyCardDiscountDTO);
                        legacyCardDiscountsBL.Save(sqlTransaction);
                    }
                }
            }
            UpdateLegacyCards(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the LegacyCard
        /// Checks if the LegacyCardId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void SaveTempLegacyCards(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            LegacyCardDataHandler legacyCardDataHandler = new LegacyCardDataHandler(sqlTransaction);
            if (legacyCardDTO.CardId > 0)
            {
                legacyCardDTO = legacyCardDataHandler.Update(legacyCardDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                legacyCardDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves legacy card transaction
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void SaveLegacyTransaction(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                lookupValuesDTOList = GetLegacyConfiguration(executionContext);
                string str = (lookupValuesDTOList.Where(x => x.LookupValue.Equals("LEGACY_CARD_TRANSFER_PRODUCT_ID")).ToList<LookupValuesDTO>()[0].Description); //Properties.Settings.Default.LoadPackages;
                int legacyCardTransferProductId = -1;
                bool result = int.TryParse(str, out legacyCardTransferProductId); //i now = 108  
                if (!result || legacyCardTransferProductId == -1)
                {
                    throw new Exception("Legacy card product not defined.! Please update the Lookups.");
                }
                string message = string.Empty;
                Transaction trx = new Transaction(Utilities);
                Card card = new Card(legacyCardDTO.ParafaitCardNumber, "", Utilities);
                trx.createTransactionLine(card, Convert.ToInt32(legacyCardTransferProductId), 1, ref message);
                int retVal = trx.SaveTransacation(sqlTransaction, ref message);
                if (retVal != 0)
                {
                    throw new Exception("Unable to Save Transaction");
                }
                legacyCardDTO.TrxId = trx.Trx_id;
                SaveAccountDetails(executionContext, sqlTransaction);
            }
            catch (Exception ex)
            { 
                log.Error("Ends-SaveLegacyTransaction() Method with an Exception:" , ex);
                log.LogMethodExit(null);
                throw ex;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves Account Details
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="sqlTransaction"></param>
        private void SaveAccountDetails(ExecutionContext executionContext, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, sqlTransaction);
            try
            {
                decimal face_value = 0;
                try
                {
                    face_value = Convert.ToDecimal(lookupValuesDTOList.Where(x => (bool)x.LookupValue.Equals("ParafaitCardFaceValue")).ToList<LookupValuesDTO>()[0].Description);/*Convert.ToDecimal(POSStatic.Utilities.getParafaitDefaults("CARD_FACE_VALUE"));*/
                }
                catch (Exception ex) { log.Error(ex); }
                if (accountDTO == null)
                {
                    AccountBL accountBL = new AccountBL(executionContext, legacyCardDTO.ParafaitCardNumber, true, true, sqlTransaction);
                    accountDTO = accountBL.AccountDTO;
                }
                if (legacyCardDTO.TransferToCard > 0)
                {
                    if (legacyCardDTO.RevisedTicketCount <= 0)
                    {
                        accountDTO.TicketCount += legacyCardDTO.TicketCount;
                    }
                    else
                    {
                        accountDTO.TicketCount += legacyCardDTO.RevisedTicketCount;
                    }
                    accountDTO.Notes = "Transferred from legacy card# " + legacyCardDTO.CardNumber;
                    accountDTO.LastUpdatedBy = "Semnox";
                    if (legacyCardDTO.RevisedCreditsPlayed <= 0)
                    {
                        accountDTO.CreditsPlayed += legacyCardDTO.CreditsPlayed;
                    }
                    else
                    {
                        accountDTO.CreditsPlayed += legacyCardDTO.RevisedCreditsPlayed;
                    }
                }
                else
                {
                    accountDTO.FaceValue = (legacyCardDTO.RevisedFaceValue <= 0 ? face_value : legacyCardDTO.RevisedFaceValue);
                    accountDTO.TicketCount = (legacyCardDTO.RevisedTicketCount <= 0 ? legacyCardDTO.TicketCount : legacyCardDTO.RevisedTicketCount);
                    accountDTO.Notes = "Transferred from legacy card# " + legacyCardDTO.CardNumber;
                    accountDTO.CreditsPlayed = (legacyCardDTO.RevisedCreditsPlayed <= 0 ? legacyCardDTO.CreditsPlayed : legacyCardDTO.RevisedCreditsPlayed);
                    accountDTO.LastPlayedTime = (legacyCardDTO.RevisedLastPlayedTime == null ? legacyCardDTO.LastPlayedTime : legacyCardDTO.RevisedLastPlayedTime);
                }
                if (accountDTO != null)
                {
                    accountDTO.AccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
                    if (isClubSpeed() && (legacyCardDTO.Credits > 0 || legacyCardDTO.RevisedCredits > 0))
                    {
                        AccountCreditPlusDTO accountCreditPlusDTO = new AccountCreditPlusDTO(-1, (legacyCardDTO.RevisedCredits <= 0 ? legacyCardDTO.Credits : legacyCardDTO.RevisedCredits), CreditPlusType.COUNTER_ITEM, true,
                        "Legacy card transfer", accountDTO.AccountId, Convert.ToInt32(legacyCardDTO.TrxId), 1, (legacyCardDTO.RevisedCredits <= 0 ? legacyCardDTO.Credits : legacyCardDTO.RevisedCredits), ServerDateTime.Now, null, null, null, null, true, true,
                        true, true, true, true, true, null, -1, false, null, true, false, false, -1, -1, true, true, -1, AccountDTO.AccountValidityStatus.Valid, -1);
                        accountDTO.Credits = 0;
                        accountDTO.AccountCreditPlusDTOList.Add(accountCreditPlusDTO);
                    }
                    else
                    {
                        if ((legacyCardDTO.Credits > 0 || legacyCardDTO.RevisedCredits > 0))
                        {
                            AccountCreditPlusDTO accountCreditPlusDTO = new AccountCreditPlusDTO(-1, (legacyCardDTO.RevisedCredits <= 0 ? legacyCardDTO.Credits : legacyCardDTO.RevisedCredits), CreditPlusType.CARD_BALANCE, true,
                            "Legacy card transfer", accountDTO.AccountId, Convert.ToInt32(legacyCardDTO.TrxId), 1, (legacyCardDTO.RevisedCredits <= 0 ? legacyCardDTO.Credits : legacyCardDTO.RevisedCredits), ServerDateTime.Now, null, null, null, null, true, true,
                            true, true, true, true, true, null, -1, false, null, true, false, false, -1, -1, true, true, -1, AccountDTO.AccountValidityStatus.Valid, -1);
                            accountDTO.AccountCreditPlusDTOList.Add(accountCreditPlusDTO);
                        }
                        if ((legacyCardDTO.Bonus > 0 || legacyCardDTO.RevisedBonus > 0))
                        {
                            AccountCreditPlusDTO accountCreditPlusDTO = new AccountCreditPlusDTO(-1, (legacyCardDTO.RevisedBonus <= 0 ? legacyCardDTO.Bonus : legacyCardDTO.RevisedBonus), CreditPlusType.GAME_PLAY_BONUS, true,
                            "Legacy card transfer", accountDTO.AccountId, Convert.ToInt32(legacyCardDTO.TrxId), 1, (legacyCardDTO.RevisedBonus <= 0 ? legacyCardDTO.Bonus : legacyCardDTO.RevisedBonus), ServerDateTime.Now, null, null, null, null, true, true,
                            true, true, true, true, true, null, -1, false, null, true, false, false, -1, -1, true, true, -1, AccountDTO.AccountValidityStatus.Valid, -1);
                            accountDTO.AccountCreditPlusDTOList.Add(accountCreditPlusDTO);
                        }
                        if ((legacyCardDTO.Courtesy > 0 || legacyCardDTO.RevisedCourtesy > 0))
                        {
                            AccountCreditPlusDTO accountCreditPlusDTO = new AccountCreditPlusDTO(-1, (legacyCardDTO.RevisedCourtesy <= 0 ? legacyCardDTO.Courtesy : legacyCardDTO.RevisedCourtesy), CreditPlusType.COUNTER_ITEM, true,
                            "Legacy card transfer", accountDTO.AccountId, Convert.ToInt32(legacyCardDTO.TrxId), 1, (legacyCardDTO.RevisedCourtesy <= 0 ? legacyCardDTO.Courtesy : legacyCardDTO.RevisedCourtesy), ServerDateTime.Now, null, null, null, null, true, true,
                            true, true, true, true, true, null, -1, false, null, true, false, false, -1, -1, true, true, -1, AccountDTO.AccountValidityStatus.Valid, -1);
                            accountDTO.AccountCreditPlusDTOList.Add(accountCreditPlusDTO);
                        }
                    }
                    MapChildRecords();
                    AccountBL accountBL = new AccountBL(executionContext, accountDTO);
                    accountBL.Save(sqlTransaction);
                    legacyCardDTO.TransferToCard = accountDTO.AccountId;
                }
            }
            catch (Exception ex)
            { 
                log.Error("Ends-SaveAccountDetails() Method with an Exception:", ex);
                log.LogMethodExit(null);
                throw ex;
            }
        }

        private void MapChildRecords()
        {
            log.LogMethodEntry();
            try
            {
                if (legacyCardDTO.LegacyCardCreditPlusDTOList != null && legacyCardDTO.LegacyCardCreditPlusDTOList.Any())
                {
                    foreach (LegacyCardCreditPlusDTO legacyCardCreditPlusDTO in legacyCardDTO.LegacyCardCreditPlusDTOList)
                    {
                        AccountCreditPlusDTO accountCreditPlusDTO = CreateAccountCreditPlusLine(legacyCardCreditPlusDTO);
                        accountDTO.AccountCreditPlusDTOList.Add(accountCreditPlusDTO);
                    }
                }
                if (legacyCardDTO.LegacyCardGamesDTOsList != null && legacyCardDTO.LegacyCardGamesDTOsList.Any())
                {
                    accountDTO.AccountGameDTOList = new List<AccountGameDTO>();
                    foreach (LegacyCardGamesDTO legacyCardGamesDTO in legacyCardDTO.LegacyCardGamesDTOsList)
                    {
                        AccountGameDTO accountGameDTO = CreateAccountGameLine(legacyCardGamesDTO, legacyCardDTO.TrxId);
                        accountDTO.AccountGameDTOList.Add(accountGameDTO);
                    }
                }
                if (legacyCardDTO.LegacyCardDiscountsDTOList != null && legacyCardDTO.LegacyCardDiscountsDTOList.Any())
                {
                    accountDTO.AccountDiscountDTOList = new List<AccountDiscountDTO>();
                    foreach (LegacyCardDiscountsDTO legacyCardDiscountsDTO in legacyCardDTO.LegacyCardDiscountsDTOList)
                    {
                        AccountDiscountDTO accountDiscountDTO = CreateCardDiscountLine(legacyCardDiscountsDTO, legacyCardDTO.TrxId);
                        accountDTO.AccountDiscountDTOList.Add(accountDiscountDTO);
                    }
                }
            }
            catch (Exception ex)
            { 
                log.Error("Ends-MapChildRecords() Method with an Exception:" , ex);
                log.LogMethodExit(null);
                throw ex;
            }
            log.LogMethodExit();
        }

        private AccountCreditPlusDTO CreateAccountCreditPlusLine(LegacyCardCreditPlusDTO legacyCardCreditPlusDTO)
        {
            log.LogMethodEntry();
            try
            {
                AccountCreditPlusDTO accountCreditPlusDTO = new AccountCreditPlusDTO(-1, (legacyCardCreditPlusDTO.RevisedLegacyCreditPlus <= 0 ? legacyCardCreditPlusDTO.LegacyCreditPlus : legacyCardCreditPlusDTO.RevisedLegacyCreditPlus), legacyCardCreditPlusDTO.CreditPlusType, legacyCardCreditPlusDTO.Refundable,
                                legacyCardCreditPlusDTO.Remarks, accountDTO.AccountId, Convert.ToInt32(legacyCardDTO.TrxId), 1, (legacyCardCreditPlusDTO.RevisedLegacyCreditPlus <= 0 ? legacyCardCreditPlusDTO.LegacyCreditPlus : legacyCardCreditPlusDTO.RevisedLegacyCreditPlus), legacyCardCreditPlusDTO.PeriodFrom, legacyCardCreditPlusDTO.PeriodTo, legacyCardCreditPlusDTO.TimeFrom, legacyCardCreditPlusDTO.TimeTo, legacyCardCreditPlusDTO.NumberOfDays, legacyCardCreditPlusDTO.Monday, legacyCardCreditPlusDTO.Tuesday,
                                legacyCardCreditPlusDTO.Wednesday, legacyCardCreditPlusDTO.Thursday, legacyCardCreditPlusDTO.Friday, legacyCardCreditPlusDTO.Saturday, legacyCardCreditPlusDTO.Sunday, legacyCardCreditPlusDTO.MinimumSaleAmount, -1, false, null, legacyCardCreditPlusDTO.TicketAllowed, false, legacyCardCreditPlusDTO.ExpireWithMembership, -1, -1, legacyCardCreditPlusDTO.PauseAllowed, legacyCardCreditPlusDTO.IsActive, -1, AccountDTO.AccountValidityStatus.Valid, -1);
                log.LogMethodExit(accountCreditPlusDTO);
                accountCreditPlusDTO = CreateCardCreditPlusConsumptionLines(legacyCardCreditPlusDTO, accountCreditPlusDTO);
                return accountCreditPlusDTO;
            }
            catch (Exception ex)
            { 
                log.Error("Ends-CreateAccountCreditPlusLine() Method with an Exception:", ex);
                log.LogMethodExit(null);
                throw ex;
            }
        }

        private AccountCreditPlusDTO CreateCardCreditPlusConsumptionLines(LegacyCardCreditPlusDTO legacyCardCreditPlusDTO, AccountCreditPlusDTO accountCreditPlusDTO)
        {
            log.LogMethodEntry();
            try
            {
                if (legacyCardCreditPlusDTO.LegacyCardCreditPlusConsumptionDTOList != null && legacyCardCreditPlusDTO.LegacyCardCreditPlusConsumptionDTOList.Any())
                {

                    foreach (LegacyCardCreditPlusConsumptionDTO legacyCardCreditPlusConsumptionDTO in legacyCardCreditPlusDTO.LegacyCardCreditPlusConsumptionDTOList)
                    {
                        int cardGameId = -1;
                        int cardGameProfileId = -1;
                        int posCounterTypeId = -1;
                        int productId = -1;
                        int categoryId = -1;
                        ValidateLegacyCardCreditPlusConsumptionDTO(legacyCardCreditPlusConsumptionDTO);
                        if (!string.IsNullOrWhiteSpace(legacyCardCreditPlusConsumptionDTO.GameName))
                        {
                            cardGameId = GetCardGameId(legacyCardCreditPlusConsumptionDTO.GameName);
                        }
                        if (!string.IsNullOrWhiteSpace(legacyCardCreditPlusConsumptionDTO.GameProfileName))
                        {
                            cardGameProfileId = GetCardGameProfileId(legacyCardCreditPlusConsumptionDTO.GameProfileName);
                        }
                        if (!string.IsNullOrWhiteSpace(legacyCardCreditPlusConsumptionDTO.PosCounterName))
                        {
                            posCounterTypeId = GetCardPOSCounterId(legacyCardCreditPlusConsumptionDTO.PosCounterName);
                        }
                        if (!string.IsNullOrWhiteSpace(legacyCardCreditPlusConsumptionDTO.ProductName))
                        {
                            productId = GetProductId(legacyCardCreditPlusConsumptionDTO.ProductName);
                        }
                        if (!string.IsNullOrWhiteSpace(legacyCardCreditPlusConsumptionDTO.Categoryname))
                        {
                            categoryId = GetCategoryId(legacyCardCreditPlusConsumptionDTO.Categoryname);
                        }
                        AccountCreditPlusConsumptionDTO accountCreditPlusConsumptionDTO = new AccountCreditPlusConsumptionDTO(-1, accountCreditPlusDTO.AccountCreditPlusId, posCounterTypeId, legacyCardCreditPlusConsumptionDTO.ExpiryDate, productId,
                                                                                            cardGameProfileId, cardGameId, legacyCardCreditPlusConsumptionDTO.DiscountPercentage, legacyCardCreditPlusConsumptionDTO.DiscountedPrice, legacyCardCreditPlusConsumptionDTO.ConsumptionBalance,
                                                                                            legacyCardCreditPlusConsumptionDTO.QuantityLimit, categoryId, legacyCardCreditPlusConsumptionDTO.DiscountAmount, -1, legacyCardCreditPlusConsumptionDTO.IsActive, legacyCardCreditPlusConsumptionDTO.ConsumptionQty);
                        accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList.Add(accountCreditPlusConsumptionDTO);
                    }

                }
            }
            catch (Exception ex)
            { 
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(accountCreditPlusDTO);
            return accountCreditPlusDTO;
        }

        private void ValidateLegacyCardCreditPlusConsumptionDTO(LegacyCardCreditPlusConsumptionDTO legacyCardCreditPlusConsumptionDTO)
        {
            log.LogMethodEntry();
            int idCount = 0;
            if (!string.IsNullOrEmpty(legacyCardCreditPlusConsumptionDTO.GameName))
            {
                idCount++;
            }
            if (!string.IsNullOrEmpty(legacyCardCreditPlusConsumptionDTO.GameProfileName))
            {
                idCount++;
            }
            if (!string.IsNullOrEmpty(legacyCardCreditPlusConsumptionDTO.PosCounterName))
            {
                idCount++;
            }
            if (!string.IsNullOrEmpty(legacyCardCreditPlusConsumptionDTO.ProductName))
            {
                idCount++;
            }
            if (!string.IsNullOrEmpty(legacyCardCreditPlusConsumptionDTO.Categoryname))
            {
                idCount++;
            }
            if (idCount > 1)
            {
                throw new Exception("Legacy Card CreditPlus Consumption has mulitple id's, Please check the legacy card data");
            }
            log.LogMethodExit();
        }

        private AccountGameDTO CreateAccountGameLine(LegacyCardGamesDTO legacyCardGamesDTO, int? trxId)
        {
            log.LogMethodEntry(legacyCardGamesDTO, trxId);
            try
            {
                ValidateLegacyCardGamesDTO(legacyCardGamesDTO);
                int cardGameId = -1;
                int cardGameProfileId = -1;
                if (!string.IsNullOrWhiteSpace(legacyCardGamesDTO.LegacycardGame_name))
                {
                    cardGameId = GetCardGameId(legacyCardGamesDTO.LegacycardGame_name);
                }
                if (!string.IsNullOrWhiteSpace(legacyCardGamesDTO.GameProfileName))
                {
                    cardGameProfileId = GetCardGameProfileId(legacyCardGamesDTO.GameProfileName);
                }
                AccountGameDTO accountGameDTO = new AccountGameDTO(-1, accountDTO.AccountId, cardGameId,
                                                                  (legacyCardGamesDTO.RevisedQuantity <= 0 ? legacyCardGamesDTO.Quantity : legacyCardGamesDTO.RevisedQuantity)
                                                                  , legacyCardGamesDTO.ExpiryDate, cardGameProfileId, legacyCardGamesDTO.Frequency, null
                                                                  , (legacyCardGamesDTO.RevisedQuantity <= 0 ? Convert.ToInt32(legacyCardGamesDTO.Quantity) : Convert.ToInt32(legacyCardGamesDTO.RevisedQuantity))
                                                                  , Convert.ToInt32(trxId), 1, null, null, -1, legacyCardGamesDTO.TicketAllowed
                                                                  , legacyCardGamesDTO.FromDate, legacyCardGamesDTO.Monday, legacyCardGamesDTO.Tuesday, legacyCardGamesDTO.Wednesday, legacyCardGamesDTO.Thursday
                                                                  , legacyCardGamesDTO.Friday, legacyCardGamesDTO.Saturday, legacyCardGamesDTO.Sunday, false, -1, -1, legacyCardGamesDTO.IsActive, AccountDTO.AccountValidityStatus.Valid, -1);
                log.LogMethodExit(accountGameDTO);
                accountGameDTO = CreateCardGamesExtendedLines(legacyCardGamesDTO, accountGameDTO);
                return accountGameDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex); 
                log.LogMethodExit(null);
                throw;
            }
        }

        private void ValidateLegacyCardGamesDTO(LegacyCardGamesDTO legacyCardGamesDTO)
        {
            log.LogMethodEntry(legacyCardGamesDTO);
            int idCount = 0;
            if (!string.IsNullOrEmpty(legacyCardGamesDTO.LegacycardGame_name))
            {
                idCount++;
            }
            if (!string.IsNullOrEmpty(legacyCardGamesDTO.GameProfileName))
            {
                idCount++;
            }
            if (idCount > 1)
            {
                throw new Exception("Legacy Card Game has mulitple id's linked, Please check the legacy card data");
            }
            log.LogMethodExit();
        }

        private AccountGameDTO CreateCardGamesExtendedLines(LegacyCardGamesDTO legacyCardGamesDTO, AccountGameDTO accountGameDTO)
        {
            log.LogMethodEntry(legacyCardGamesDTO, accountGameDTO);
            try
            {

                if (legacyCardGamesDTO.LegacyCardGameExtendedDTOList != null && legacyCardGamesDTO.LegacyCardGameExtendedDTOList.Any())
                {
                    accountGameDTO.AccountGameExtendedDTOList = new List<AccountGameExtendedDTO>();
                    foreach (LegacyCardGameExtendedDTO legacyCardGameExtendedDTO in legacyCardGamesDTO.LegacyCardGameExtendedDTOList)
                    {
                        int cardGameId = -1;
                        int cardGameProfileId = -1;
                        ValidateLegacyCardGameExtendedDTO(legacyCardGameExtendedDTO);
                        if (!string.IsNullOrWhiteSpace(legacyCardGameExtendedDTO.GameName))
                        {
                            cardGameId = GetCardGameId(legacyCardGameExtendedDTO.GameName);
                        }
                        if (!string.IsNullOrWhiteSpace(legacyCardGameExtendedDTO.GameProfileName))
                        {
                            cardGameProfileId = GetCardGameProfileId(legacyCardGameExtendedDTO.GameProfileName);
                        }
                        AccountGameExtendedDTO accountGameExtendedDTO = new AccountGameExtendedDTO(-1, accountGameDTO.AccountGameId, cardGameId, cardGameProfileId, legacyCardGameExtendedDTO.Exclude, legacyCardGameExtendedDTO.PlayLimitPerGame, legacyCardGameExtendedDTO.IsActive);
                        accountGameDTO.AccountGameExtendedDTOList.Add(accountGameExtendedDTO);
                        log.LogMethodExit(accountGameDTO);
                        return accountGameDTO;
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null);
                throw;
            }
            log.LogMethodExit(accountGameDTO);
            return accountGameDTO;
        }

        private void ValidateLegacyCardGameExtendedDTO(LegacyCardGameExtendedDTO legacyCardGameExtendedDTO)
        {
            log.LogMethodEntry(legacyCardGameExtendedDTO);
            int idCount = 0;
            if (!string.IsNullOrEmpty(legacyCardGameExtendedDTO.GameName))
            {
                idCount++;
            }
            if (!string.IsNullOrEmpty(legacyCardGameExtendedDTO.GameProfileName))
            {
                idCount++;
            }
            if (idCount > 1)
            {
                throw new Exception("Legacy Card Game Extended has mulitple id's linked, Please check the legacy card data");
            }
            log.LogMethodExit();
        }

        private AccountDiscountDTO CreateCardDiscountLine(LegacyCardDiscountsDTO legacyCardDiscountsDTO, int? trxId)
        {
            log.LogMethodEntry(legacyCardDiscountsDTO, trxId);
            try
            {
                int disountId = -1;
                if (!string.IsNullOrWhiteSpace(legacyCardDiscountsDTO.Discount_name))
                {
                    disountId = GetDiscountId(legacyCardDiscountsDTO.Discount_name);
                }
                AccountDiscountDTO accountDiscountDTO = new AccountDiscountDTO(-1, accountDTO.AccountId, disountId, legacyCardDiscountsDTO.ExpiryDate, Convert.ToInt32(trxId), 1, -1, -1, legacyCardDiscountsDTO.IsActive, null,
                    -1, -1, AccountDTO.AccountValidityStatus.Valid, -1);
                log.LogMethodExit(accountDiscountDTO);
                return accountDiscountDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex); 
                log.LogMethodExit(null);
                throw;
            }
        }

        private int GetCardGameId(string gameName)
        {
            log.LogMethodEntry(gameName);
            int id = -1;
            List<GameDTO> gameDTOList = new List<GameDTO>();
            GameList gameList = new GameList(executionContext);
            List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters = new List<KeyValuePair<GameDTO.SearchByGameParameters, string>>();
            searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.GAME_NAME, gameName));
            gameDTOList = gameList.GetGameList(searchParameters);
            if (gameDTOList != null && gameDTOList.Any())
            {
                id = gameDTOList[0].GameId;
            }
            else
            {
                throw new Exception(gameName + "Game not found");
            }

            log.LogMethodExit(id);
            return id;
        }

        private int GetCardGameProfileId(string gameProfileName)
        {
            log.LogMethodEntry(gameProfileName);
            int id = -1;
            List<GameProfileDTO> GameProfileDTOList = new List<GameProfileDTO>();
            GameProfileList gameProfileList = new GameProfileList(executionContext);
            List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> searchParameters = new List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>>();
            searchParameters.Add(new KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>(GameProfileDTO.SearchByGameProfileParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>(GameProfileDTO.SearchByGameProfileParameters.GAMEPROFILE_NAME, gameProfileName));
            GameProfileDTOList = gameProfileList.GetGameProfileDTOList(searchParameters);
            if (GameProfileDTOList != null && GameProfileDTOList.Any())
            {
                id = GameProfileDTOList[0].GameProfileId;
            }
            else
            {
                throw new Exception(gameProfileName + "Game profile not found");
            }
            log.LogMethodExit(id);
            return id;

        }

        private int GetCardPOSCounterId(string posCounterName)
        {
            log.LogMethodEntry(posCounterName);
            int id = -1;
            List<POSTypeDTO> pOSTypeDTOList = new List<POSTypeDTO>();
            POSTypeListBL pOSTypeListBL = new POSTypeListBL(executionContext);
            List<KeyValuePair<POSTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<POSTypeDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<POSTypeDTO.SearchByParameters, string>(POSTypeDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<POSTypeDTO.SearchByParameters, string>(POSTypeDTO.SearchByParameters.POS_TYPE_NAME, posCounterName));
            pOSTypeDTOList = pOSTypeListBL.GetPOSTypeDTOList(searchParameters);
            if (pOSTypeDTOList != null && pOSTypeDTOList.Any())
            {
                id = pOSTypeDTOList[0].POSTypeId;
            }
            else
            {
                throw new Exception(posCounterName + "POS Type not found");
            }
            log.LogMethodExit(id);
            return id;

        }

        private int GetProductId(string productName)
        {
            log.LogMethodEntry(productName);
            int id = -1;
            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new ArgumentNullException("ProductName", " Product Name is Empty");
            }
            IEnumerable<ProductsContainerDTO> productsContainerDTOList = ProductsContainerList.GetActiveProductsContainerDTOList(executionContext, ManualProductType.SELLABLE.ToString(), x => productName.Equals(x.ProductName, StringComparison.InvariantCultureIgnoreCase));
            if (productsContainerDTOList.Any() == false)
            {
                throw new Exception(productName + " Product not found");
            }
            ProductsContainerDTO productsContainerDTO = productsContainerDTOList.FirstOrDefault();
            id = productsContainerDTO.ProductId;
            log.LogMethodExit(id);
            return id;

        }

        private int GetCategoryId(string categoryName)
        {
            log.LogMethodEntry(categoryName);
            int id = -1;
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                throw new ArgumentNullException("CategoryName", " Category Name is Empty");
            }
            IEnumerable<CategoryContainerDTO> categoryContainerDTOList = CategoryContainerList.GetActiveCategoryContainerDTOList(executionContext.SiteId, x => categoryName.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase));
            if (categoryContainerDTOList.Any() == false)
            {
                throw new Exception(categoryName + " Category not found");
            }
            CategoryContainerDTO categoryContainerDTO = categoryContainerDTOList.FirstOrDefault();
            id = categoryContainerDTO.CategoryId;

            log.LogMethodExit(id);
            return id;
        }

        private int GetDiscountId(string discountName)
        {
            log.LogMethodEntry(discountName);
            int id = -1;
            List<DiscountsDTO> DiscountDTOList = null;
            using(UnitOfWork unitOfWork = new UnitOfWork())
            {
                DiscountsListBL discountsListBL = new DiscountsListBL(executionContext, unitOfWork);
                SearchParameterList<DiscountsDTO.SearchByParameters> searchParameters = new SearchParameterList<DiscountsDTO.SearchByParameters>();
                searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.ACTIVE_FLAG, "1"));
                searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.DISCOUNT_NAME, discountName));
                DiscountDTOList = discountsListBL.GetDiscountsDTOList(searchParameters);
            }
            
            if (DiscountDTOList != null && DiscountDTOList.Any())
            {
                id = DiscountDTOList[0].DiscountId;
            }
            else
            {
                throw new Exception("Error occurred while retrieving the discounts." + discountName);
            }
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Get Legacy Configuration
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        private List<LookupValuesDTO> GetLegacyConfiguration(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "LEGACY_CARD_TRANSFER_CONFIGURATIONS"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                log.LogMethodExit(lookupValuesDTOList);
                return lookupValuesDTOList;
            }
            catch (Exception e)
            {
                log.Error(e); 
                log.LogMethodExit(null);
                return null;
            }
        }

        /// <summary>
        /// Update Legacy Cards
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void UpdateLegacyCards(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ExternalCardSystem externalCardSystem = ExternalCardSystemFactory.GetInstance(legacyCardDTO).GetExternalCardSystem(executionContext);
            if (ChkIncludePackages())
            {
                legacyCardDTO.TransferredCardgames = 'Y';
            }
            else
            {
                legacyCardDTO.TransferredCardgames = 'N';
            }
            if (externalCardSystem != null)
            {
                externalCardSystem.ProcessCardData(sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Check Include Packages
        /// </summary>
        /// <returns></returns>
        public bool ChkIncludePackages()
        {
            log.LogMethodEntry();
            bool chkIncludePackages = false;
            try
            {
                chkIncludePackages = Convert.ToBoolean(lookupValuesDTOList.Where(x => (bool)x.LookupValue.Equals("LoadPackages")).ToList<LookupValuesDTO>()[0].Description); //Properties.Settings.Default.LoadPackages;
                log.LogMethodExit(chkIncludePackages);
                return chkIncludePackages;
            }
            catch
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// Get Sacoa Packages
        /// </summary>
        /// <param name="legacyCardNumber"></param>
        /// <param name="cardTime"></param>
        /// <returns></returns>
        public DataTable GetSacoaPackages(string legacyCardNumber, int cardTime = 0)
        {
            log.LogMethodEntry(legacyCardNumber, cardTime);
            DataTable dtGetSacoaPackages = new DataTable();
            LegacyCardDataHandler legacyCardDataHandler = new LegacyCardDataHandler();
            dtGetSacoaPackages = legacyCardDataHandler.GetSacoaPackages(legacyCardNumber, cardTime);
            log.LogMethodExit(dtGetSacoaPackages);
            return dtGetSacoaPackages;
        }



        /// <summary>
        /// Update Legacy Card Games
        /// </summary>
        /// <param name="trxId"></param>
        /// <param name="parafaitCardId"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <param name="TrxLineId"></param>
        /// <param name="sqlTransaction"></param>
        public void UpdateLegacyCardGames(int trxId, int parafaitCardId, object productId, object quantity, int TrxLineId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(trxId, parafaitCardId, productId, quantity, TrxLineId, sqlTransaction);
            try
            {
                AccountGameListBL accountGameListBL = new AccountGameListBL(executionContext);
                List<KeyValuePair<AccountGameDTO.SearchByParameters, string>> cardGamesSearchParam;
                cardGamesSearchParam = new List<KeyValuePair<AccountGameDTO.SearchByParameters, string>>();
                cardGamesSearchParam.Add(new KeyValuePair<AccountGameDTO.SearchByParameters, string>(AccountGameDTO.SearchByParameters.ACCOUNT_ID, Convert.ToString(parafaitCardId)));
                cardGamesSearchParam.Add(new KeyValuePair<AccountGameDTO.SearchByParameters, string>(AccountGameDTO.SearchByParameters.TRANSACTION_ID, Convert.ToString(trxId)));
                List<AccountGameDTO> cardGamesDTOList = new List<AccountGameDTO>();
                cardGamesDTOList = accountGameListBL.GetAccountGameDTOList(cardGamesSearchParam);
                if (cardGamesDTOList != null && cardGamesDTOList.Count > 0)
                {
                    AccountGameDTO accountGameDTO = cardGamesDTOList[0];
                    AccountBL accountBL = new AccountBL(executionContext, parafaitCardId);
                    var obj = accountBL.AccountDTO.AccountGameDTOList.FirstOrDefault(x => x.AccountGameId == accountGameDTO.AccountGameId);
                    if (obj != null)
                    {
                        obj.Quantity = Convert.ToInt32(quantity);
                        obj.BalanceGames = Convert.ToInt32(quantity);
                    }
                    accountBL.Save(sqlTransaction);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Update Card Credit Plus
        /// </summary>
        /// <param name="trxId"></param>
        /// <param name="parafaitCardId"></param>
        /// <param name="productId"></param>
        /// <param name="cardTime"></param>
        /// <param name="TrxLineId"></param>
        /// <param name="sqlTransaction"></param>
        public void UpdateCardCreditPlus(int trxId, int parafaitCardId, object productId, Decimal cardTime, int TrxLineId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(trxId, parafaitCardId, productId, cardTime, TrxLineId, sqlTransaction);
            try
            {
                AccountBL accountBL = new AccountBL(executionContext, parafaitCardId);
                var obj = accountBL.AccountDTO.AccountCreditPlusDTOList.FirstOrDefault(c => c.CreditPlusType.Equals("M") && c.TransactionId == trxId && c.TransactionLineId == TrxLineId);
                obj.CreditPlus = cardTime;
                obj.CreditPlusBalance = cardTime;
                accountBL.Save(sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Check if ClubSpeed
        /// </summary>
        /// <returns></returns>
        private bool isClubSpeed()
        {
            log.LogMethodEntry();
            bool isClubSpeed = false;
            lookupValuesDTOList = GetLegacyConfiguration(executionContext);
            if (lookupValuesDTOList != null && lookupValuesDTOList.Count > 0)
            {
                isClubSpeed = lookupValuesDTOList.Where(x => (bool)x.LookupValue.Equals("IS_CLUB_SPEED_ENVIRONMENT")).ToList<LookupValuesDTO>()[0].Description.Equals("Y");
            }
            log.LogMethodExit(isClubSpeed);
            return isClubSpeed;
        }

        /// <summary>
        /// Validating External Cards
        /// </summary>
        /// <param name="legacyCardsDTO"></param>
        /// <returns></returns>
        private List<ValidationError> ValidateExternalCards(LegacyCardDTO legacyCardsDTO)
        {
            log.LogMethodEntry(legacyCardsDTO);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (legacyCardsDTO == null)
            {
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Validation Error"),
                                                             MessageContainerList.GetMessage(executionContext, "ClubSpeedCardSystem"),
                                                             MessageContainerList.GetMessage(executionContext, "Card not found in Club Speed")));
            }
            if (legacyCardsDTO != null && legacyCardsDTO.Credits <= 0)
            {
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Validation Error"),
                                                            MessageContainerList.GetMessage(executionContext, "ClubSpeedCardSystem"),
                                                            MessageContainerList.GetMessage(executionContext, "Card cannot be converted as balance in Club Speed is 0")));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
    }

    /// <summary>
    /// Manages the list of LegacyCardListBL
    /// </summary>
    public class LegacyCardListBL
    {
        SqlTransaction sqlTransaction = null;
        private List<LegacyCardDTO> legacyCardDTOList;
        private ExecutionContext executionContext;
        Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public LegacyCardListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.legacyCardDTOList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="legacyCardDTOList"></param>
        /// <param name="executionContext"></param>
        public LegacyCardListBL(ExecutionContext executionContext, List<LegacyCardDTO> legacyCardDTOList, Utilities utilities)
            : this(executionContext)
        {
            log.LogMethodEntry(legacyCardDTOList, executionContext);
            this.legacyCardDTOList = legacyCardDTOList;
            this.utilities = utilities;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get all the LegacyCardDTO list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>List of LegacyCardDTO</returns>
        public List<LegacyCardDTO> GetLegacyCardDTOList(List<KeyValuePair<LegacyCardDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null,
                                                        bool loadChildRecords = true, bool activeChildRecords = true)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction, loadChildRecords, activeChildRecords);
            LegacyCardDataHandler legacyCardDataHandler = new LegacyCardDataHandler(sqlTransaction);
            List<LegacyCardDTO> legacyCardDTOList = legacyCardDataHandler.GetLegacyCardDTO(searchParameters);
            if (legacyCardDTOList != null && legacyCardDTOList.Any() && loadChildRecords)
            {
                LegacyCardBuilderBL legacyCardBuilderBL = new LegacyCardBuilderBL(executionContext, legacyCardDTOList);
                legacyCardDTOList = legacyCardBuilderBL.Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(legacyCardDTOList);
            return legacyCardDTOList;
        }

        /// <summary>
        /// Get all the LegacyCardDTO list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>List of LegacyCardDTO</returns>
        public DataTable GetLegacyCardsDTOList(string legacyCardNumber, string parafaitCardNumber)
        {
            log.LogMethodEntry(legacyCardNumber, parafaitCardNumber);
            DataTable dtLegacyCardsDTOList = new DataTable();
            LegacyCardDataHandler legacyCardDataHandler = new LegacyCardDataHandler();
            dtLegacyCardsDTOList = legacyCardDataHandler.GetLegacyCardsDTOList(legacyCardNumber, parafaitCardNumber);
            log.LogMethodExit(dtLegacyCardsDTOList);
            return dtLegacyCardsDTOList;

        }



        /// <summary>
        /// Save or update records with inner collections
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(sqlTransaction);
                if (legacyCardDTOList != null)
                {
                    foreach (LegacyCardDTO legacyCardDTO in legacyCardDTOList)
                    {
                        LegacyCardBL legacyCardBL = new LegacyCardBL(executionContext, legacyCardDTO, utilities);
                        legacyCardBL.Save(sqlTransaction);
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

    }

    /// <summary>
    /// Legacy Card Builder BL
    /// </summary>
    public class LegacyCardBuilderBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        List<LegacyCardDTO> legacyCardDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public LegacyCardBuilderBL(ExecutionContext executionContext, List<LegacyCardDTO> legacyCardDTOList)
        {
            log.LogMethodEntry(executionContext, legacyCardDTOList);
            this.legacyCardDTOList = legacyCardDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Build
        /// </summary>
        /// <param name="LegacyCardCreditPlusDTO"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        public void Build(LegacyCardCreditPlusDTO LegacyCardCreditPlusDTO, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(LegacyCardCreditPlusDTO, activeChildRecords, sqlTransaction);
            if (LegacyCardCreditPlusDTO != null && LegacyCardCreditPlusDTO.LegacyCardCreditPlusId != -1)
            {
                LegacyCreditPlusConsumptionListBL LegacyCardCreditPlusConsumptionListBL = new LegacyCreditPlusConsumptionListBL(executionContext);
                List<KeyValuePair<LegacyCardCreditPlusConsumptionDTO.SearchByParameters, string>> LegacyCardCreditPlusConsumptionSearchParams = new List<KeyValuePair<LegacyCardCreditPlusConsumptionDTO.SearchByParameters, string>>();
                LegacyCardCreditPlusConsumptionSearchParams.Add(new KeyValuePair<LegacyCardCreditPlusConsumptionDTO.SearchByParameters, string>(LegacyCardCreditPlusConsumptionDTO.SearchByParameters.LEGACY_CARD_CREDIT_PLUS_ID, LegacyCardCreditPlusDTO.LegacyCardCreditPlusId.ToString()));
                if (LegacyCardCreditPlusDTO.LegacyCardCreditPlusConsumptionDTOList == null)
                {
                    LegacyCardCreditPlusDTO.LegacyCardCreditPlusConsumptionDTOList = new List<LegacyCardCreditPlusConsumptionDTO>();
                }
                LegacyCardCreditPlusDTO.LegacyCardCreditPlusConsumptionDTOList = LegacyCardCreditPlusConsumptionListBL.GetLegacyCardCreditPlusConsumptionDTOList(LegacyCardCreditPlusConsumptionSearchParams, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Build
        /// </summary>
        /// <param name="legacyCardDTOList"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        public List<LegacyCardDTO> Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            if (legacyCardDTOList != null && legacyCardDTOList.Count > 0)
            {
                Dictionary<int, LegacyCardDTO> LegacyCardIDDictionary = new Dictionary<int, LegacyCardDTO>();
                HashSet<int> legacyCardIdSet = new HashSet<int>();
                string cardIdList;
                for (int i = 0; i < legacyCardDTOList.Count; i++)
                {
                    if (legacyCardDTOList[i].CardId != -1)
                    {
                        legacyCardIdSet.Add(legacyCardDTOList[i].CardId);
                        LegacyCardIDDictionary.Add(legacyCardDTOList[i].CardId, legacyCardDTOList[i]);
                    }
                }
                cardIdList = string.Join<int>(",", legacyCardIdSet);

                LegacyCardCreditPlusListBL legacyCardCreditPlusListBL = new LegacyCardCreditPlusListBL(executionContext);
                List<KeyValuePair<LegacyCardCreditPlusDTO.SearchByParameters, string>> LegacyCardCreditPlusConsumptionSearchParams = new List<KeyValuePair<LegacyCardCreditPlusDTO.SearchByParameters, string>>();
                LegacyCardCreditPlusConsumptionSearchParams.Add(new KeyValuePair<LegacyCardCreditPlusDTO.SearchByParameters, string>(LegacyCardCreditPlusDTO.SearchByParameters.CARD_ID_LIST, cardIdList));
                List<LegacyCardCreditPlusDTO> legacyCardCreditPlusDTOList = legacyCardCreditPlusListBL.GetLegacyCardCreditPlusDTOList(LegacyCardCreditPlusConsumptionSearchParams, true, true, sqlTransaction);
                if (legacyCardCreditPlusDTOList != null && legacyCardCreditPlusDTOList.Count > 0)
                {
                    foreach (var legacyCardCreditPlusDTO in legacyCardCreditPlusDTOList)
                    {
                        if (LegacyCardIDDictionary.ContainsKey(legacyCardCreditPlusDTO.LegacyCard_id))
                        {
                            if (LegacyCardIDDictionary[legacyCardCreditPlusDTO.LegacyCard_id].LegacyCardCreditPlusDTOList == null)
                            {
                                LegacyCardIDDictionary[legacyCardCreditPlusDTO.LegacyCard_id].LegacyCardCreditPlusDTOList = new List<LegacyCardCreditPlusDTO>();
                            }
                            LegacyCardIDDictionary[legacyCardCreditPlusDTO.LegacyCard_id].LegacyCardCreditPlusDTOList.Add(legacyCardCreditPlusDTO);
                        }
                    }
                }

                LegacyCardGameListBL legacyCardGameListBL = new LegacyCardGameListBL(executionContext);
                List<KeyValuePair<LegacyCardGamesDTO.SearchByParameters, string>> legacyCardGameSearchParams = new List<KeyValuePair<LegacyCardGamesDTO.SearchByParameters, string>>();
                legacyCardGameSearchParams.Add(new KeyValuePair<LegacyCardGamesDTO.SearchByParameters, string>(LegacyCardGamesDTO.SearchByParameters.CARD_ID_LIST, cardIdList));
                List<LegacyCardGamesDTO> legacyCardGamesDTOList = legacyCardGameListBL.GetLegacyCardGamesDTOList(legacyCardGameSearchParams, true, activeChildRecords, sqlTransaction);
                if (legacyCardGamesDTOList != null)
                {
                    foreach (var legacyCardGamesDTO in legacyCardGamesDTOList)
                    {
                        if (LegacyCardIDDictionary.ContainsKey(legacyCardGamesDTO.LegacyCard_id))
                        {
                            if (LegacyCardIDDictionary[legacyCardGamesDTO.LegacyCard_id].LegacyCardGamesDTOsList == null)
                            {
                                LegacyCardIDDictionary[legacyCardGamesDTO.LegacyCard_id].LegacyCardGamesDTOsList = new List<LegacyCardGamesDTO>();
                            }
                            LegacyCardIDDictionary[legacyCardGamesDTO.LegacyCard_id].LegacyCardGamesDTOsList.Add(legacyCardGamesDTO);
                        }
                    }
                }

                LegacyCardDiscountListBL legacyCardDiscountListBL = new LegacyCardDiscountListBL(executionContext);
                List<KeyValuePair<LegacyCardDiscountsDTO.SearchByParameters, string>> legacyCardDiscountSearchParams = new List<KeyValuePair<LegacyCardDiscountsDTO.SearchByParameters, string>>();
                legacyCardDiscountSearchParams.Add(new KeyValuePair<LegacyCardDiscountsDTO.SearchByParameters, string>(LegacyCardDiscountsDTO.SearchByParameters.CARD_ID_LIST, cardIdList));
                List<LegacyCardDiscountsDTO> legacyCardDiscountsDTOList = legacyCardDiscountListBL.GetAccountDiscountDTOList(legacyCardDiscountSearchParams, sqlTransaction);
                if (legacyCardDiscountsDTOList != null)
                {
                    foreach (var legacyCardDiscountsDTO in legacyCardDiscountsDTOList)
                    {
                        if (LegacyCardIDDictionary.ContainsKey(legacyCardDiscountsDTO.Legacycard_id))
                        {
                            if (LegacyCardIDDictionary[legacyCardDiscountsDTO.Legacycard_id].LegacyCardDiscountsDTOList == null)
                            {
                                LegacyCardIDDictionary[legacyCardDiscountsDTO.Legacycard_id].LegacyCardDiscountsDTOList = new List<LegacyCardDiscountsDTO>();
                            }
                            LegacyCardIDDictionary[legacyCardDiscountsDTO.Legacycard_id].LegacyCardDiscountsDTOList.Add(legacyCardDiscountsDTO);
                        }
                    }
                }
                legacyCardDTOList = LoadLegacyCardSummary(legacyCardDTOList);
            }
            log.LogMethodExit(legacyCardDTOList);
            return legacyCardDTOList;
        }

        /// <summary>
        /// Load Legacy Car dSummary
        /// </summary>
        /// <param name="legacyCardDTOList"></param>
        public List<LegacyCardDTO> LoadLegacyCardSummary(List<LegacyCardDTO> legacyCardDTOList)
        {
            log.LogMethodEntry(legacyCardDTOList);
            foreach (LegacyCardDTO legacyCardDTO in legacyCardDTOList)
            {
                if (legacyCardDTO.LegacyCardCreditPlusDTOList != null && legacyCardDTO.LegacyCardCreditPlusDTOList.Any())
                {
                    legacyCardDTO.LegacyCardSummaryDTO.TotalCredits = GetTotalCredits(legacyCardDTO.LegacyCardCreditPlusDTOList);
                    legacyCardDTO.LegacyCardSummaryDTO.TotalBonus = GetTotalBonus(legacyCardDTO.LegacyCardCreditPlusDTOList);
                    legacyCardDTO.LegacyCardSummaryDTO.TotalTime = GetTotalTime(legacyCardDTO.LegacyCardCreditPlusDTOList);
                    legacyCardDTO.LegacyCardSummaryDTO.TotalTickets = GetTotalTickets(legacyCardDTO.LegacyCardCreditPlusDTOList);
                    legacyCardDTO.LegacyCardSummaryDTO.TotalLoyaltyPoints = GetTotalLoyaltyPoints(legacyCardDTO.LegacyCardCreditPlusDTOList);
                }
                if (legacyCardDTO.LegacyCardGamesDTOsList != null && legacyCardDTO.LegacyCardGamesDTOsList.Any())
                {
                    legacyCardDTO.LegacyCardSummaryDTO.TotalGames = GetTotalGames(legacyCardDTO.LegacyCardGamesDTOsList);
                }
            }
            log.LogMethodExit(legacyCardDTOList);
            return legacyCardDTOList;
        }

        /// <summary>
        /// Get Total Credits
        /// </summary>
        /// <param name="legacyCardCreditPlusDTOList"></param>
        /// <returns></returns>
        private decimal GetTotalCredits(List<LegacyCardCreditPlusDTO> legacyCardCreditPlusDTOList)
        {
            log.LogMethodEntry(legacyCardCreditPlusDTOList);
            decimal totalCredits = 0;
            List<LegacyCardCreditPlusDTO> legacyCardCreditPlusList = legacyCardCreditPlusDTOList.FindAll(l => l.CreditPlusType.Equals(CreditPlusType.CARD_BALANCE) || l.CreditPlusType.Equals(CreditPlusType.GAME_PLAY_CREDIT) || l.CreditPlusType.Equals(CreditPlusType.COUNTER_ITEM));
            foreach (LegacyCardCreditPlusDTO legacyCardCreditPlusDTO in legacyCardCreditPlusList)
            {
                totalCredits = totalCredits + (legacyCardCreditPlusDTO.RevisedLegacyCreditPlus > 0 ? legacyCardCreditPlusDTO.RevisedLegacyCreditPlus : legacyCardCreditPlusDTO.LegacyCreditPlus);
            }
            log.LogMethodExit(totalCredits);
            return totalCredits;
        }

        /// <summary>
        /// Get Total Bonus
        /// </summary>
        /// <param name="legacyCardCreditPlusDTOList"></param>
        /// <returns></returns>
        private decimal GetTotalBonus(List<LegacyCardCreditPlusDTO> legacyCardCreditPlusDTOList)
        {
            log.LogMethodEntry(legacyCardCreditPlusDTOList);
            decimal totalBonus = 0;
            List<LegacyCardCreditPlusDTO> legacyCardCreditPlusList = legacyCardCreditPlusDTOList.FindAll(l => l.CreditPlusType.Equals(CreditPlusType.GAME_PLAY_BONUS));
            foreach (LegacyCardCreditPlusDTO legacyCardCreditPlusDTO in legacyCardCreditPlusList)
            {
                totalBonus = totalBonus + (legacyCardCreditPlusDTO.RevisedLegacyCreditPlus > 0 ? legacyCardCreditPlusDTO.RevisedLegacyCreditPlus : legacyCardCreditPlusDTO.LegacyCreditPlus);
            }
            log.LogMethodExit(totalBonus);
            return totalBonus;
        }

        /// <summary>
        /// Get Total Time
        /// </summary>
        /// <param name="legacyCardCreditPlusDTOList"></param>
        /// <returns></returns>
        private decimal GetTotalTime(List<LegacyCardCreditPlusDTO> legacyCardCreditPlusDTOList)
        {
            log.LogMethodEntry(legacyCardCreditPlusDTOList);
            decimal totalTime = 0;
            List<LegacyCardCreditPlusDTO> legacyCardCreditPlusList = legacyCardCreditPlusDTOList.FindAll(l => l.CreditPlusType.Equals(CreditPlusType.TIME));
            foreach (LegacyCardCreditPlusDTO legacyCardCreditPlusDTO in legacyCardCreditPlusList)
            {
                totalTime = totalTime + (legacyCardCreditPlusDTO.RevisedLegacyCreditPlus > 0 ? legacyCardCreditPlusDTO.RevisedLegacyCreditPlus : legacyCardCreditPlusDTO.LegacyCreditPlus);
            }
            log.LogMethodExit(totalTime);
            return totalTime;
        }

        /// <summary>
        /// Get Total Ticketes
        /// </summary>
        /// <param name="legacyCardCreditPlusDTOList"></param>
        /// <returns></returns>
        private decimal GetTotalTickets(List<LegacyCardCreditPlusDTO> legacyCardCreditPlusDTOList)
        {
            log.LogMethodEntry(legacyCardCreditPlusDTOList);
            decimal totalTickets = 0;
            List<LegacyCardCreditPlusDTO> legacyCardCreditPlusList = legacyCardCreditPlusDTOList.FindAll(l => l.CreditPlusType.Equals(CreditPlusType.TICKET));
            foreach (LegacyCardCreditPlusDTO legacyCardCreditPlusDTO in legacyCardCreditPlusList)
            {
                totalTickets = totalTickets + (legacyCardCreditPlusDTO.RevisedLegacyCreditPlus > 0 ? legacyCardCreditPlusDTO.RevisedLegacyCreditPlus : legacyCardCreditPlusDTO.LegacyCreditPlus);
            }
            log.LogMethodExit(totalTickets);
            return totalTickets;
        }

        /// <summary>
        /// Get Total LoyaltyPoints
        /// </summary>
        /// <param name="legacyCardCreditPlusDTOList"></param>
        /// <returns></returns>
        private decimal GetTotalLoyaltyPoints(List<LegacyCardCreditPlusDTO> legacyCardCreditPlusDTOList)
        {
            log.LogMethodEntry(legacyCardCreditPlusDTOList);
            decimal totalLoyaltyPoints = 0;
            List<LegacyCardCreditPlusDTO> legacyCardCreditPlusList = legacyCardCreditPlusDTOList.FindAll(l => l.CreditPlusType.Equals(CreditPlusType.LOYALTY_POINT));
            foreach (LegacyCardCreditPlusDTO legacyCardCreditPlusDTO in legacyCardCreditPlusList)
            {
                totalLoyaltyPoints = totalLoyaltyPoints + (legacyCardCreditPlusDTO.RevisedLegacyCreditPlus > 0 ? legacyCardCreditPlusDTO.RevisedLegacyCreditPlus : legacyCardCreditPlusDTO.LegacyCreditPlus);
            }
            log.LogMethodExit(totalLoyaltyPoints);
            return totalLoyaltyPoints;
        }

        /// <summary>
        /// Get Total Games
        /// </summary>
        /// <param name="legacyCardGamesDTOList"></param>
        /// <returns></returns>
        private decimal GetTotalGames(List<LegacyCardGamesDTO> legacyCardGamesDTOList)
        {
            log.LogMethodEntry(legacyCardGamesDTOList);
            decimal totalGames = 0;
            foreach (LegacyCardGamesDTO legacyCardGamesDTO in legacyCardGamesDTOList)
            {
                totalGames = totalGames + (legacyCardGamesDTO.RevisedQuantity > 0 ? legacyCardGamesDTO.RevisedQuantity : legacyCardGamesDTO.Quantity);
            }
            log.LogMethodExit(totalGames);
            return totalGames;
        }
    }
}
