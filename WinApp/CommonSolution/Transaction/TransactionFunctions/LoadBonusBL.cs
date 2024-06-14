/********************************************************************************************
 * Project Name - AccountService
 * Description  - Load Bonus to card
 * 
 **************
 **Version Log
 **************
 *Version    Date          Modified By            Remarks          
 *********************************************************************************************
 *2.80.0     12-Mar-2020   Girish Kundar          Created
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
namespace Semnox.Parafait.Transaction
{

    /// <summary>
    /// Load bonus class
    /// </summary>
    public class LoadBonusBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Utilities utilities;
        private TransactionServiceDTO transactionServiceDTO;
        private AccountBL accountBL;
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private LoadBonusBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            utilities = new Utilities();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="transactionServiceDTO"></param>
        public LoadBonusBL(ExecutionContext executionContext, TransactionServiceDTO transactionServiceDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.transactionServiceDTO = transactionServiceDTO;
            log.LogMethodExit();
        }


        /// <summary>
        /// Method to do load bonus, games to card
        /// </summary>
        public void LoadBonusToCard(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            SqlConnection sqlConnection = null;
            SqlTransaction parafaitDBTrx;
            if (sqlTransaction == null)
            {
                sqlConnection = utilities.createConnection();
                parafaitDBTrx = sqlConnection.BeginTransaction();
            }
            else
            {
                parafaitDBTrx = sqlTransaction;
            }
            string message = string.Empty;
            TaskProcs.EntitlementType entitlementType = TaskProcs.EntitlementType.Bonus;
            HelperClassBL.SetParafairEnvValues(executionContext, utilities);
            TaskProcs taskProcs = new TaskProcs(utilities);
            try
            {
                AccountCreditPlusDTO accountCreditPlusDTO = null;
                if (transactionServiceDTO.SourceAccountDTO.AccountCreditPlusDTOList != null && transactionServiceDTO.SourceAccountDTO.AccountCreditPlusDTOList.Count > 0)
                {
                    var list = transactionServiceDTO.SourceAccountDTO.AccountCreditPlusDTOList.Where(x => x.AccountCreditPlusId == -1).ToList();
                    if (list != null && list.Count > 1)
                    {
                        log.LogMethodExit("Trying add more than one credit plus value");
                        throw new Exception("Trying add more than one credit plus value");
                    }
                    accountCreditPlusDTO = transactionServiceDTO.SourceAccountDTO.AccountCreditPlusDTOList.Where(x => x.AccountCreditPlusId == -1).FirstOrDefault();
                    if (accountCreditPlusDTO != null)
                    {
                        int expiryDays = accountCreditPlusDTO.PeriodTo != null ? (Convert.ToDateTime(accountCreditPlusDTO.PeriodTo) - Convert.ToDateTime(accountCreditPlusDTO.PeriodFrom)).Days : 0;
                        entitlementType = taskProcs.getEntitlementType(CreditPlusTypeConverter.ToString(accountCreditPlusDTO.CreditPlusType));
                        Card currentCard = new Card(transactionServiceDTO.SourceAccountDTO.AccountId, executionContext.GetUserId(), utilities, parafaitDBTrx);
                        if (accountCreditPlusDTO.CreditPlusType == CreditPlusType.TICKET)
                        {
                            bool managerApprovalReceived = (utilities.ParafaitEnv.ManagerId != -1);
                            int manualTicketsToLoad = Convert.ToInt32(accountCreditPlusDTO.CreditPlus);
                            int managerApprovalLimit = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "ADD_TICKET_LIMIT_FOR_MANAGER_APPROVAL_REDEMPTION");
                            if ((manualTicketsToLoad > managerApprovalLimit && managerApprovalLimit != 0 && managerApprovalReceived == false))
                            {
                                throw new Exception(MessageContainerList.GetMessage(executionContext, 268));
                            }
                            if (manualTicketsToLoad > ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "MAX_MANUAL_TICKETS_PER_REDEMPTION"))
                            {
                                throw new Exception(MessageContainerList.GetMessage(executionContext, 2495, ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "MAX_MANUAL_TICKETS_PER_REDEMPTION")));
                            }
                            if (!taskProcs.loadTickets(currentCard, Convert.ToInt32(accountCreditPlusDTO.CreditPlus), transactionServiceDTO.Remarks, -1, ref message, parafaitDBTrx))
                            {
                                message = "ERROR" + message;
                                throw new Exception(message);
                            }
                        }
                        else if (!(taskProcs.LoadGenericEntitlement(currentCard, Convert.ToDouble(accountCreditPlusDTO.CreditPlus), entitlementType, accountCreditPlusDTO.Refundable, transactionServiceDTO.GamePlayId, transactionServiceDTO.Remarks,
                                                         ref message, accountCreditPlusDTO.PeriodFrom, expiryDays <= 0 ? new int?() : expiryDays, null)))
                        {
                            message = "Error" + message;
                            log.LogMethodExit(message);
                            throw new Exception(message);
                        }
                    }
                }
                // Both can be allowed 
                if (transactionServiceDTO.SourceAccountDTO.AccountGameDTOList != null && transactionServiceDTO.SourceAccountDTO.AccountGameDTOList.Count > 0)
                {
                    //throw new Exception("This is not implemented completly...returning ");
                    //int transactionId = -1;
                    //Card card = new Card(transactionServiceDTO.SourceAccountDTO.AccountId, executionContext.GetUserId(), utilities);
                    //Transaction transaction = new Transaction(utilities);
                    //List<AccountGameDTO> tempList = transactionServiceDTO.SourceAccountDTO.AccountGameDTOList.Where(x => x.AccountGameId == -1).ToList();
                    //accountBL = new AccountBL(executionContext, transactionServiceDTO.SourceAccountDTO.AccountId, true, true, parafaitDBTrx);
                    //if (accountBL.AccountDTO.AccountGameDTOList == null)
                    //{
                    //    accountBL.AccountDTO.AccountGameDTOList = new List<AccountGameDTO>();
                    //}
                    //accountBL.AccountDTO.AccountGameDTOList.AddRange(tempList);

                    //int productId = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "LOAD_BONUS_TASK_PRODUCT"));
                    //if (productId == -1)
                    //{
                    //    //throw new Exception("Consolidation task product value is not set");
                    //    throw new Exception(utilities.MessageUtils.getMessage("Unable to find the product for load bonus task. Please verify the card set up"));
                    //}
                    //List<KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>> searchParameters = new List<KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>>();
                    //searchParameters.Add(new KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>(ProductGamesDTO.SearchByProductGamesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    //searchParameters.Add(new KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>(ProductGamesDTO.SearchByProductGamesParameters.PRODUCT_ID, productId.ToString()));
                    //ProductGamesListBL productGamesListBL = new ProductGamesListBL(executionContext);
                    //List<ProductGamesDTO> productGamesDTOList = productGamesListBL.GetProductGamesDTOList(searchParameters, sqlTransaction);
                    //if (productGamesDTOList == null)
                    //{
                    //    SaveProductGames(productId, parafaitDBTrx);
                    //}
                    //else
                    //{
                    //    productGamesDTOList.ForEach(x => x.ISActive = false);
                    //    productGamesListBL = new ProductGamesListBL(executionContext, productGamesDTOList);
                    //    productGamesListBL.DeleteProductGamesList();
                    //    SaveProductGames(productId, parafaitDBTrx);
                    //}
                    //if (tempList != null && tempList.Count > 0)
                    //{
                    //    transaction.createTransactionLine(card, productId, 0, 1, ref message);
                    //    int returnValue = transaction.SaveTransacation(parafaitDBTrx, ref message);
                    //    transactionId = transaction.Trx_id;
                    //    tempList.ForEach((x) =>
                    //    {
                    //        x.TransactionId = transactionId;
                    //        x.TransactionLineId = transaction.TrxLines[0].DBLineId;
                    //    });

                    //    foreach (var trxLine in transaction.TrxLines)
                    //    {
                    //        transaction.CreateCardGames(parafaitDBTrx, card.card_id, trxLine.ProductID, transactionId, trxLine.DBLineId, trxLine);
                    //    }
                    //    taskProcs.createTask(card.card_id, TaskProcs.LOADBONUS, 0, -1, -1, -1, -1, 0, transactionServiceDTO.GamePlayId, "Games loaded", parafaitDBTrx, -1, -1, -1, -1, -1);
                    //    accountBL = new AccountBL(executionContext, accountBL.AccountDTO);
                    //    accountBL.Save(parafaitDBTrx);
                    //}
                    log.LogMethodExit();
                }
                if (sqlTransaction == null)
                {
                    parafaitDBTrx.Commit();
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                if (sqlTransaction == null)
                {
                    parafaitDBTrx.Rollback();
                }
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
                throw new Exception(ex.Message);
            }
        }

        private int GetGraceTime(SqlTransaction SQLTrx, int TrxId, Transaction.TransactionLine Line)
        {
            log.LogMethodEntry(TrxId, Line);

            if (Line.LineAtb == null || Line.card == null || Line.LineAtb.AttractionBookingDTO.AttractionPlayId == -1)
            {
                log.LogMethodExit(0);
                return 0;
            }

            FacilityMapBL facilityMapBL = new FacilityMapBL(executionContext, Line.LineAtb.AttractionBookingDTO.FacilityMapId, false, true, SQLTrx);
            if (facilityMapBL != null && facilityMapBL.FacilityMapDTO != null)
            {
                //int returnValueNew = (facilityDTOList[0].GraceTime == null ? 0 : (int)facilityDTOList[0].GraceTime);
                int returnValueNew = facilityMapBL.FacilityMapDTO.GraceTime == null ? 0 : (int)facilityMapBL.FacilityMapDTO.GraceTime;
                log.LogMethodEntry(returnValueNew);
                return returnValueNew;
            }
            log.LogMethodExit(0);
            return 0;
        }


        private void SaveProductGames(int productId, SqlTransaction sqlTransaction)
        {
            List<ProductGamesDTO> ProductGamesDTOList = new List<ProductGamesDTO>();
            foreach (AccountGameDTO accountGameDTO in accountBL.AccountDTO.AccountGameDTOList)
            {
                ProductGamesDTO productGamesDTO = new ProductGamesDTO(-1, productId, accountGameDTO.GameId, accountGameDTO.Quantity, null,
                                            accountGameDTO.ExpiryDate, "D", accountGameDTO.GameProfileId, accountGameDTO.Frequency, accountGameDTO.Guid,
                                            accountGameDTO.SiteId, accountGameDTO.SynchStatus, accountGameDTO.MembershipId, accountGameDTO.EntitlementType, accountGameDTO.OptionalAttribute,
                                            null, accountGameDTO.CustomDataSetId, accountGameDTO.TicketAllowed, null, accountGameDTO.FromDate, accountGameDTO.MasterEntityId,
                                            accountGameDTO.Monday, accountGameDTO.Tuesday, accountGameDTO.Wednesday, accountGameDTO.Thursday, accountGameDTO.Friday, accountGameDTO.Saturday,
                                            accountGameDTO.Sunday, executionContext.GetUserId(), DateTime.Now, executionContext.GetUserId(), DateTime.Now, accountGameDTO.IsActive);
                ProductGamesDTOList.Add(productGamesDTO);
            }
            ProductGamesListBL productGamesListBL = new ProductGamesListBL(executionContext, ProductGamesDTOList);
            productGamesListBL.SaveUpdateProductGamesList();

        }
        //public void CreateCardGames(SqlTransaction SQLTrx, int CardId, int ProductId, int TrxId, int LineId, Transaction.TransactionLine Line)
        //{
        //    log.LogMethodEntry(SQLTrx, CardId, ProductId, TrxId, LineId, Line);

        //    int GraceTime = GetGraceTime(SQLTrx, TrxId, Line);

        //    SqlCommand cmd = utilities.getCommand(SQLTrx);

        //    cmd.CommandText = @"insert into CardGames (Card_Id, game_profile_id, Game_Id, quantity, 
        //                                ExpiryDate, FromDate, 
        //                                Frequency, BalanceGames, TrxId, TrxLineId, 
        //                                EntitlementType, OptionalAttribute, @last_update_date, CustomDataSetId, TicketAllowed, site_id,
        //                                Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday, ExpireWithMembership, 
        //                                MembershipId, MembershipRewardsId, CreatedBy, CreationDate, LastUpdatedBy, IsActive, ValidityStatus) 
        //        Values (@cardId, @game_profile_id, @game_Id, @Quantity, 
        //            case when @ExpiryDate is not null then 
        //       case when @ExpiryTime is null 
        //        then  dateadd(ss, @OffsetDuration, dateadd(MI, @graceTime, dateadd(MI, 6, dateadd(dd, 1, expirydate))))
        //        else  dateadd(ss, @OffsetDuration, dateadd(MI, @graceTime, dateadd(mi, (FLOOR(ExpiryTime)*60 + (ExpiryTime - FLOOR(ExpiryTime))*100), expirydate)))
        //       end
        //      end ExpiryDate,
        //            case when @Frequency not in ('B', 'A') then 
        //                    case when @FromDate is not null then dateadd(ss, @OffsetDuration, dateadd(MI, -@graceTime, dateadd(DD, isnull(EffectiveAfterDays, 0), dateadd(HH, 6, FromDate))))
        //                        else case when isnull(EffectiveAfterDays, 0) > 0 then dateadd(ss, @OffsetDuration, dateadd(MI, -@graceTime, dateadd(HH, 6, dateadd(DD, EffectiveAfterDays, DATEADD(D, 0, DATEDIFF(D, 0, case when @IsAttraction = 1 then @schFrom else @refDate end))))))
        //                                else dateadd(ss, @OffsetDuration, dateadd(MI, -@graceTime, case when @IsAttraction = 1 then @schFrom else @refDate end))
        //                            end 
        //                    end 
        //            else case when @Frequency = 'B' then (select CONVERT(DateTime,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@passPhrase,prof.DateOfBirth))) birth_date
        //                                                        from cards c left outer join customers cu on cu.customer_id = c.customer_id left outer join Profile prof on cu.ProfileId = prof.id
        //                                                        where c.card_id = @cardId)
        //                        when @Frequency = 'A' then (select  CONVERT(DateTime,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@passPhrase,prof.Anniversary))) anniversary
        //                                                        from cards c left outer join customers cu on cu.customer_id = c.customer_id left outer join Profile prof on cu.ProfileId = prof.id
        //                                                        where c.card_id = @cardId)
        //                        else NULL
        //                    end
        //            end startDate, 
        //            @Frequency, @balanceGames, @TrxId, @TrxLineId,
        //            @EntitlementType, @OptionalAttribute, getdate(), @CustomDataSetId, @TicketAllowed, @site_id,
        //            CASE WHEN (@Monday IS NULL OR @Monday = 1) then 'Y' else 'N' end,
        //            CASE WHEN (@Tuesday IS NULL OR @Tuesday = 1) then 'Y' else 'N' end,
        //            CASE WHEN (@Wednesday IS NULL OR @Wednesday = 1) then 'Y' else 'N' end,
        //            CASE WHEN (@Thursday IS NULL OR @Thursday = 1) then 'Y' else 'N' end,
        //            CASE WHEN (@Friday IS NULL OR @Friday = 1) then 'Y' else 'N' end,
        //            CASE WHEN (@Saturday IS NULL OR @Saturday = 1) then 'Y' else 'N' end,
        //            CASE WHEN (@Sunday IS NULL OR @Sunday = 1) then 'Y' else 'N' end,
        //            @ExpireWithMembership, @MembershipId, @MembershipRewardsId, @CreatedBy, 
        //            GetDate(), @lastUpdatedBy, @isActive, @ValidityStatus
        //          )
        //    --where product_game_id = @productGameId; select @cardGameId = @@identity;
        //    --insert into cardGameExtended (cardGameId, GameId, GameProfileId, Exclude, site_id, PlayLimitPerGame)
        //    --//    select @cardGameId, GameId, GameProfileId, Exclude, site_id, PlayLimitPerGame 
        //    --//    from productGameExtended
        //    --//    where productGameId = @productGameId;
        //   -- //INSERT INTO EntityOverrideDates (EntityName, EntityGuid, OverrideDate, IncludeExcludeFlag, Day, Remarks, LastUpdatedBy, LastUpdatedDate, site_id)
        //    --//SELECT 'CARDGAMES',(SELECT Guid from CARDGAMES WHERE card_game_id = @cardGameId), OverrideDate, IncludeExcludeFlag, Day, Remarks, @lastUpdatedBy, GetDate(), site_id 
        //    --//FROM EntityOverrideDates
        //    --//WHERE EntityGuid = (SELECT Guid from ProductGames where product_game_id = @productGameId)";

        //    foreach (AccountGameDTO accountGameDTO in accountBL.AccountDTO.AccountGameDTOList)
        //    {
        //        cmd.Parameters.Clear();
        //        cmd.Parameters.AddWithValue("@passPhrase", Semnox.Core.Utilities.ParafaitDefaultContainerList.GetDecryptedParafaitDefault(utilities.ExecutionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE"));
        //        cmd.Parameters.AddWithValue("@cardId", CardId);
        //        cmd.Parameters.AddWithValue("@isActive", 1);
        //        cmd.Parameters.AddWithValue("@balanceGames", accountGameDTO.BalanceGames);

        //        cmd.Parameters.AddWithValue("@Monday", accountGameDTO.Monday);
        //        cmd.Parameters.AddWithValue("@Tuesday", accountGameDTO.Tuesday);
        //        cmd.Parameters.AddWithValue("@Wednesday", accountGameDTO.Wednesday);
        //        cmd.Parameters.AddWithValue("@Thursday", accountGameDTO.Thursday);
        //        cmd.Parameters.AddWithValue("@Friday", accountGameDTO.Friday);
        //        cmd.Parameters.AddWithValue("@Saturday", accountGameDTO.Saturday);
        //        cmd.Parameters.AddWithValue("@Sunday", accountGameDTO.Sunday);
        //        cmd.Parameters.AddWithValue("@EntitlementType", accountGameDTO.EntitlementType);
        //        cmd.Parameters.AddWithValue("@OptionalAttribute", accountGameDTO.OptionalAttribute);
        //        cmd.Parameters.AddWithValue("@CustomDataSetId", accountGameDTO.CustomDataSetId);
        //        cmd.Parameters.AddWithValue("@TicketAllowed", accountGameDTO.TicketAllowed);

        //        cmd.Parameters.AddWithValue("@quantity", accountGameDTO.Quantity);
        //        cmd.Parameters.AddWithValue("@ExpiryDate", accountGameDTO.ExpiryDate);
        //        cmd.Parameters.AddWithValue("@FromDate", accountGameDTO.FromDate);
        //        cmd.Parameters.AddWithValue("@game_Id", accountGameDTO.GameId);
        //        cmd.Parameters.AddWithValue("@site_id", executionContext.GetSiteId());
        //        cmd.Parameters.AddWithValue("@refDate", (Line.LineAtb != null && Line.LineAtb.AttractionBookingDTO != null) ? Line.LineAtb.AttractionBookingDTO.ScheduleFromDate : utilities.getServerTime());//Entitlement Date added to card games
        //        cmd.Parameters.AddWithValue("@game_profile_id", accountGameDTO.GameProfileId);
        //        cmd.Parameters.AddWithValue("@lastUpdatedBy", utilities.ParafaitEnv.LoginID);
        //        cmd.Parameters.AddWithValue("@TrxId", TrxId == -1 ? DBNull.Value : (object)TrxId);
        //        cmd.Parameters.AddWithValue("@TrxLineId", LineId == -1 ? DBNull.Value : (object)LineId);
        //        cmd.Parameters.AddWithValue("@graceTime", GraceTime);
        //        cmd.Parameters.AddWithValue("@CreatedBy", utilities.ParafaitEnv.LoginID);
        //        cmd.Parameters.AddWithValue("@MembershipId", Line.MembershipId == -1 ? DBNull.Value : (object)Line.MembershipId);
        //        cmd.Parameters.AddWithValue("@MembershipRewardsId", Line.MembershipRewardsId == -1 ? DBNull.Value : (object)Line.MembershipRewardsId);
        //        if (Line.ExpireWithMembership == "Y")
        //            cmd.Parameters.AddWithValue("@ExpireWithMembership", Line.ExpireWithMembership);
        //        else
        //            cmd.Parameters.AddWithValue("@ExpireWithMembership", DBNull.Value);

        //        cmd.Parameters.AddWithValue("@IsAttraction", Line.LineAtb != null ? 1 : 0);
        //        cmd.Parameters.AddWithValue("@schFrom", Line.LineAtb == null ? DBNull.Value : (object)Line.LineAtb.AttractionBookingDTO.ScheduleFromDate);
        //        cmd.Parameters.AddWithValue("@schTo", Line.LineAtb == null ? DBNull.Value : (object)Line.LineAtb.AttractionBookingDTO.ScheduleToDate);
        //        cmd.Parameters.AddWithValue("@OffsetDuration", 0);
        //        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "LOAD_CARD_ENTITLEMENT_ON_TRANSACTION_COMPLETE").Equals("Y"))
        //            cmd.Parameters.AddWithValue("@ValidityStatus", 'H');
        //        else
        //            cmd.Parameters.AddWithValue("@ValidityStatus", DBNull.Value);
        //        cmd.ExecuteNonQuery();

        //        log.LogVariableState("LOAD_CARD_ENTITLEMENT_ON_TRANSACTION_COMPLETE: ", ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "LOAD_CARD_ENTITLEMENT_ON_TRANSACTION_COMPLETE"));

        //    }

        //    cmd.CommandText = @"delete cardGameExtended
        //from CardGames cg
        //where cg.card_game_id < (select MAX(card_game_id)
        //							from cardGames
        //							where card_id = @cardId
        //							and Frequency in ('A', 'B'))
        //and cg.card_game_id = CardGameExtended.CardGameId
        //and cg.card_id = @cardId
        //and cg.Frequency in ('A', 'B');

        //                        delete from cardGames 
        //                        where card_game_id < (select MAX(card_game_id)
        //                                            from cardGames pg
        //                                            where pg.Frequency = cardGames.Frequency
        //                                            and pg.card_id = cardGames.card_id)
        //                        and Frequency in ('A', 'B')
        //                        and card_id = @cardId";
        //    cmd.Parameters.Clear();
        //    cmd.Parameters.AddWithValue("@cardId", CardId);
        //    cmd.ExecuteNonQuery();

        //    log.LogVariableState("@cardId", CardId);
        //    log.LogVariableState("@site_id", executionContext.GetSiteId());
        //    log.LogVariableState("@refDate", utilities.getServerTime());//Entitlement Date added to card games
        //    log.LogVariableState("@lastUpdatedBy", utilities.ParafaitEnv.LoginID);
        //    log.LogVariableState("@TrxId", TrxId == -1 ? DBNull.Value : (object)TrxId);
        //    log.LogVariableState("@TrxLineId", LineId == -1 ? DBNull.Value : (object)LineId);
        //    log.LogVariableState("@cardId", CardId);
        //    log.LogMethodExit(null);
        //}
    }
}
