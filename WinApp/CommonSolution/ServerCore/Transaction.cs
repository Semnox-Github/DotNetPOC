/*******************************************************************************************************************************************
 * Project Name - Transaction
 * Description  - ServerCore Transaction
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ********************************************************************************************************************************************
 *2.70.2.0      04-Oct-2019      Jinto Thomas   Modified insert trx_header query. Added createdby,
 *                                                                          creationdate fields       
 *2.130.3     21-Jan-2022      Mathew Ninan   Load gameplay id to TransactionLineGamePlayMapping instead of Trx_lines 
 *2.130.9     30-Jun-2022      Mathew Ninan   Moved all credits, Bonus, loyalty points to Credit plus than card level 
 *                                            entitlement. Ticket# 43211 
 *2.130.10    19-Jan-2023      Mathew Ninan   Create one creditplus record for the day to improve performance 
 *                                            and reduce load on Upload engine. Ticket# 92681                             
 *2.140.5     20-Feb-2023      Mathew Ninan   Logic to ignore transaction creation based on config "CREATE_TRX_TOKEN_REDEMPTION_MACHINE"                                          
 *********************************************************************************************************************************************/
using System;
using Semnox.Parafait.Device.PaymentGateway;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Parafait.Transaction;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.ServerCore
{
    static class Transaction
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static bool createGamePlayTokenOutTransaction(long gamePlayId, int card_id, string creditBonus, int BonusCount, decimal TokenPrice, string isTicketEater, ServerStatic ServerStatic, ref string message)
        {
            log.LogMethodEntry(gamePlayId, card_id, creditBonus, BonusCount, TokenPrice, isTicketEater, ServerStatic, message);
            SqlCommand cmd = ServerStatic.Utilities.getCommand(ServerStatic.Utilities.createConnection());
            SqlTransaction SQLTrx = cmd.Connection.BeginTransaction();
            cmd.Transaction = SQLTrx;
            decimal Amount;
            string CreditPlusType = "A";
            if (isTicketEater == "Y") // ATM Machine
                Amount = BonusCount * TokenPrice;
            else
                Amount = 0;
            try
            {
                cmd.CommandText = " insert into trx_header (TrxDate, TrxAmount, TrxDiscountPercentage, " +
                                                            "TaxAmount, TrxNetAmount, pos_machine, " +
                                                            "user_id, payment_mode, cashAmount, " +
                                                            "creditCardAmount, gameCardAmount, OtherPaymentModeAmount, Status, CreatedBy, CreationDate, LastUpdatedBy, LastUpdateTime) " +
                                                    "values (getdate(), @Amount, 0, " +
                                                            "0, @Amount, @POSName, " +
                                                            "(select user_id from users where LoginId = 'External POS'), 1, @Amount, " +
                                                            "0, 0, 0, 'CLOSED', (select user_id from users where LoginId = 'External POS'), getdate()," +
                                                            " (select user_id from users where LoginId = 'External POS') , getdate()); select @@identity";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Amount", Amount);
                cmd.Parameters.AddWithValue("@POSName", Environment.MachineName);
                log.LogVariableState("@Amount", Amount);
                log.LogVariableState("@POSName", Environment.MachineName);
                int localTrxId = Convert.ToInt32(cmd.ExecuteScalar());

                if (isTicketEater == "Y")
                {
                    PaymentModeList paymentModeListBL = new PaymentModeList(ServerStatic.Utilities.ExecutionContext);
                    List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
                    List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                    if (paymentModeDTOList != null)
                    {
                        TransactionPaymentsDTO cashTrxPaymentDTO = new TransactionPaymentsDTO();
                        cashTrxPaymentDTO.PaymentModeId = paymentModeDTOList[0].PaymentModeId;
                        cashTrxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                        cashTrxPaymentDTO.Amount = (double)Amount;
                        cashTrxPaymentDTO.CurrencyCode = string.Empty;
                        cashTrxPaymentDTO.CurrencyRate = null;
                        cashTrxPaymentDTO.PosMachine = Environment.MachineName;
                        cashTrxPaymentDTO.TenderedAmount = (double)Amount;
                        cashTrxPaymentDTO.TransactionId = localTrxId;
                        TransactionPaymentsBL cashTrxPaymentBL = new TransactionPaymentsBL(ServerStatic.Utilities.ExecutionContext, cashTrxPaymentDTO);
                        cashTrxPaymentBL.Save(SQLTrx);
                    }
                    //Semnox.Parafait.Transaction.TransactionUtils transactionUtils = new Semnox.Parafait.Transaction.TransactionUtils(ServerStatic.Utilities);
                    //transactionUtils.CreateCashTrxPayment((int)localTrxId, (double)Amount, (double)Amount, SQLTrx);
                }

                cmd.CommandText = "insert into trx_lines (TrxId, LineId, price, amount, quantity, card_id, card_number, product_id, credits, bonus, loyalty_points, gameplayId) " +
                                    "select top 1 @TrxId, 1, @TokenPrice, @Amount, 0, c.card_id, c.card_number, product_id, @credits, @bonus, @loyalty_points, @gameplayId " +
                                    "from products p, cards c " +
                                    "where card_id = @cardId " +
                                    "and valid_flag = 'Y' " +
                                    "and product_type_id = (select product_type_id " +
                                                            "from product_type " +
                                                            "where product_type = 'GAMEPLAYCREDIT')";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Amount", Amount);
                cmd.Parameters.AddWithValue("@TrxId", localTrxId);
                cmd.Parameters.AddWithValue("@cardId", card_id);
                cmd.Parameters.AddWithValue("@gameplayId", DBNull.Value);//gameplayId stored in separate entity
                cmd.Parameters.AddWithValue("@TokenPrice", TokenPrice);
                log.LogVariableState("@Amount", Amount);
                log.LogVariableState("@TrxId", localTrxId);
                log.LogVariableState("@cardId", card_id);
                log.LogVariableState("@gameplayId", DBNull.Value);
                log.LogVariableState("@TokenPrice", TokenPrice);

                if (creditBonus == "C")
                {
                    cmd.Parameters.AddWithValue("@bonus", 0);
                    cmd.Parameters.AddWithValue("@credits", BonusCount * TokenPrice);
                    cmd.Parameters.AddWithValue("@loyalty_points", 0);
                    CreditPlusType = "A";
                    log.LogVariableState("@bonus", 0);
                    log.LogVariableState("@credits", BonusCount * TokenPrice);
                    log.LogVariableState("@loyalty_points", 0);
                }
                else if (creditBonus == "B")
                {
                    cmd.Parameters.AddWithValue("@bonus", BonusCount * TokenPrice);
                    cmd.Parameters.AddWithValue("@credits", 0);
                    cmd.Parameters.AddWithValue("@loyalty_points", 0);
                    CreditPlusType = "B";
                    log.LogVariableState("@bonus", BonusCount * TokenPrice);
                    log.LogVariableState("@credits", 0);
                    log.LogVariableState("@loyalty_points", 0);
                }
                else if (creditBonus == "L")
                {
                    cmd.Parameters.AddWithValue("@bonus", 0);
                    cmd.Parameters.AddWithValue("@credits", 0);
                    cmd.Parameters.AddWithValue("@loyalty_points", BonusCount * TokenPrice);
                    CreditPlusType = "L";
                    log.LogVariableState("@bonus", 0);
                    log.LogVariableState("@credits", 0);
                    log.LogVariableState("@loyalty_points", BonusCount * TokenPrice);
                }
                else if (creditBonus == "RP" || creditBonus == "NP") // non / refundable creditplus
                {
                    cmd.Parameters.AddWithValue("@bonus", 0);
                    cmd.Parameters.AddWithValue("@credits", 0);
                    cmd.Parameters.AddWithValue("@loyalty_points", 0);
                    CreditPlusType = "A";
                    log.LogVariableState("@bonus", 0);
                    log.LogVariableState("@credits", 0);
                    log.LogVariableState("@loyalty_points", 0);
                }

                cmd.ExecuteNonQuery();

                //if (creditBonus == "C" || creditBonus == "B" || creditBonus == "L")
                //{
                //    cmd.CommandText = @"update cards set credits = isnull(credits, 0) + @credits, 
                //                                         bonus = isnull(bonus, 0) + @bonus, 
                //                                         loyalty_points = isnull(loyalty_points, 0) + @loyalty_points, 
                //                                         last_update_time = getdate() " +
                //                        "where card_id = @cardId ";
                //    cmd.ExecuteNonQuery();
                //}
                //else
                //{
                cmd.CommandText = @"insert into CardCreditPlus (Card_Id, CreditPlus, CreditPlusBalance, Refundable, Remarks, TrxId, LineId, CreditPlusType, CreationDate, LastupdatedDate, LastUpdatedBy) 
                                        select card_Id, @CreditPlus, @CreditPlus, @Refundable, @Remarks, @TrxId, 1, @CreditPlusType, getdate(), getdate() , (select user_id from users where LoginId = 'External POS')
                                        from cards 
                                        where card_Id = @cardId";

                cmd.Parameters.AddWithValue("@CreditPlus", BonusCount * TokenPrice);
                cmd.Parameters.AddWithValue("@Refundable", (creditBonus == "NP" ? "N" : "Y"));
                cmd.Parameters.AddWithValue("@Remarks", "Token out Credit Plus");
                cmd.Parameters.AddWithValue("@CreditPlusType", CreditPlusType); //"A"

                cmd.ExecuteNonQuery();

                log.LogVariableState("@CreditPlus", BonusCount * TokenPrice);
                log.LogVariableState("@Refundable", (creditBonus == "NP" ? "N" : "Y"));
                log.LogVariableState("@Remarks", "Token out Credit Plus");
                log.LogVariableState("@CreditPlusType", CreditPlusType);
                //}

                //Create GamePlay transaction mapping entry
                if (!createTransactionGamePlayRecord(gamePlayId, localTrxId, 1, ServerStatic, SQLTrx, ref message))
                    throw new Exception(message);
                SQLTrx.Commit();
                cmd.Connection.Close();
                log.LogVariableState("Message", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                SQLTrx.Rollback();
                cmd.Connection.Close();
                log.Error("Error occured while creating GamePlay Token Out Transaction", ex);
                log.LogVariableState("Message", message);
                log.LogMethodExit(false);
                return false;
            }

        }

        public static bool createTransactionGamePlayRecord(long gamePlayId, int trxId, int lineId, ServerStatic ServerStatic, SqlTransaction sqlTrx, ref string message)
        {
            log.LogMethodEntry(gamePlayId, trxId, lineId, ServerStatic, message);
            log.Debug("Initiating command and connection objects");
            SqlCommand cmd = ServerStatic.Utilities.getCommand(sqlTrx);
            log.Debug("Created command and connection objects");
            try
            {
                cmd.CommandText = @"insert into TransactionLineGamePlayMapping(TrxId, LineId, GamePlayId, IsActive,
                                                       guid, site_id, creationDate, createdBy, LastUpdateDate, LastUpdatedBy) 
                                          values (@TrxId, @LineId, @gameplayId, 1, newid(), @siteId, getdate(), @user, getdate(), @user)";

                cmd.Parameters.AddWithValue("@TrxId", trxId);
                log.LogVariableState("@TrxId", trxId);

                cmd.Parameters.AddWithValue("@LineId", lineId);
                log.LogVariableState("@LineId", lineId);

                cmd.Parameters.AddWithValue("@gameplayId", gamePlayId);
                log.LogVariableState("@gameplayId", gamePlayId);

                cmd.Parameters.AddWithValue("@user", "semnox");
                object site_id = DBNull.Value;
                if (ServerStatic.Utilities.ParafaitEnv.IsCorporate == false || ServerStatic.Utilities.ParafaitEnv.SiteId <= 0)
                    site_id = DBNull.Value;
                else
                    site_id = ServerStatic.Utilities.ParafaitEnv.SiteId;
                cmd.Parameters.AddWithValue("@siteId", site_id);

                log.Debug("Calling ExecuteNonQuery");
                cmd.ExecuteNonQuery();
                log.Debug("Calling ExecuteNonQuery completed");
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                log.Error(ex);
                cmd.Connection.Close();
                log.Error("Error occured while creating GamePlay Token Out Transaction", ex);
                log.LogVariableState("Message", message);
                log.LogMethodExit(false);
                return false;
            }
        }
        public static bool createGamePlayTokenOutTransaction(long gamePlayId, int card_id, string creditBonus, int BonusCount, decimal TokenPrice, string isTicketEater, GameServerEnvironment GameServerEnvironment, ref string message, Utilities Utilities)
        {
            log.LogMethodEntry(gamePlayId, card_id, creditBonus, BonusCount, TokenPrice, isTicketEater, GameServerEnvironment, message, Utilities);
            bool createTrx = false;
            try
            {
                //createTrx = GameServerEnvironment.Utilities.getParafaitDefaults("CREATE_TRX_TOKEN_REDEMPTION_MACHINE").Equals("Y");
                createTrx = ParafaitDefaultContainerList.GetParafaitDefault(GameServerEnvironment.GameEnvExecutionContext, "CREATE_TRX_TOKEN_REDEMPTION_MACHINE").Equals("Y");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                createTrx = false;
            }
            SqlCommand cmd = Utilities.getCommand(Utilities.createConnection());
            SqlTransaction SQLTrx = cmd.Connection.BeginTransaction();
            cmd.Transaction = SQLTrx;
            decimal Amount;
            string CreditPlusType = "A";
            if (isTicketEater == "Y") // ATM Machine
                Amount = BonusCount * TokenPrice;
            else
                Amount = 0;
            try
            {
                cmd.CommandText = " insert into trx_header (TrxDate, TrxAmount, TrxDiscountPercentage, " +
                                                            "TaxAmount, TrxNetAmount, pos_machine, " +
                                                            "user_id, payment_mode, cashAmount, " +
                                                            "creditCardAmount, gameCardAmount, OtherPaymentModeAmount, Status, CreatedBy, CreationDate, LastUpdatedBy, LastUpdateTime) " +
                                                    "values (getdate(), @Amount, 0, " +
                                                            "0, @Amount, @POSName, " +
                                                            "(select user_id from users where LoginId = 'External POS'), 1, @Amount, " +
                                                            "0, 0, 0, 'CLOSED', (select user_id from users where LoginId = 'External POS'), getdate()," +
                                                            " @loginId , getdate()); select @@identity";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Amount", Amount);
                cmd.Parameters.AddWithValue("@POSName", Environment.MachineName);
                cmd.Parameters.AddWithValue("@loginId", Utilities.ParafaitEnv.LoginID);
                log.LogVariableState("@Amount", Amount);
                log.LogVariableState("@POSName", Environment.MachineName);
                log.LogVariableState("@loginId", Utilities.ParafaitEnv.LoginID);
                int localTrxId = -1;
                if (createTrx)
                {
                    localTrxId = Convert.ToInt32(cmd.ExecuteScalar());
                    if (isTicketEater == "Y")
                    {
                        PaymentModeList paymentModeListBL = new PaymentModeList(GameServerEnvironment.GameEnvExecutionContext);
                        List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
                        List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                        if (paymentModeDTOList != null)
                        {
                            TransactionPaymentsDTO cashTrxPaymentDTO = new TransactionPaymentsDTO();
                            cashTrxPaymentDTO.PaymentModeId = paymentModeDTOList[0].PaymentModeId;
                            cashTrxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                            cashTrxPaymentDTO.Amount = (double)Amount;
                            cashTrxPaymentDTO.CurrencyCode = string.Empty;
                            cashTrxPaymentDTO.CurrencyRate = null;
                            cashTrxPaymentDTO.PosMachine = Environment.MachineName;
                            cashTrxPaymentDTO.TenderedAmount = (double)Amount;
                            cashTrxPaymentDTO.TransactionId = localTrxId;
                            TransactionPaymentsBL cashTrxPaymentBL = new TransactionPaymentsBL(GameServerEnvironment.GameEnvExecutionContext, cashTrxPaymentDTO);
                            cashTrxPaymentBL.Save(SQLTrx);
                        }
                        //Semnox.Parafait.Transaction.TransactionUtils transactionUtils = new Semnox.Parafait.Transaction.TransactionUtils(ServerStatic.Utilities);
                        //transactionUtils.CreateCashTrxPayment((int)localTrxId, (double)Amount, (double)Amount, SQLTrx);
                    }
                }

                cmd.CommandText = "insert into trx_lines (TrxId, LineId, price, amount, quantity, card_id, card_number, product_id, credits, bonus, loyalty_points, gameplayId, creationDate, LastUpdateDate) " +
                                    "select top 1 @TrxId, 1, @TokenPrice, @Amount, 0, c.card_id, c.card_number, product_id, @credits, @bonus, @loyalty_points, @gameplayId, getdate(), getdate() " +
                                    "from products p, cards c " +
                                    "where card_id = @cardId " +
                                    "and valid_flag = 'Y' " +
                                    "and product_type_id = (select product_type_id " +
                                                            "from product_type " +
                                                            "where product_type = 'GAMEPLAYCREDIT')";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Amount", Amount);
                cmd.Parameters.AddWithValue("@TrxId", localTrxId == -1 ? (object)DBNull.Value : localTrxId);
                cmd.Parameters.AddWithValue("@cardId", card_id);
                cmd.Parameters.AddWithValue("@gameplayId", DBNull.Value);//gameplayId stored in separate entity
                cmd.Parameters.AddWithValue("@TokenPrice", TokenPrice);
                log.LogVariableState("@Amount", Amount);
                log.LogVariableState("@TrxId", localTrxId);
                log.LogVariableState("@cardId", card_id);
                log.LogVariableState("@gameplayId", DBNull.Value);
                log.LogVariableState("@TokenPrice", TokenPrice);

                if (creditBonus == "C")
                {
                    cmd.Parameters.AddWithValue("@bonus", 0);
                    cmd.Parameters.AddWithValue("@credits", BonusCount * TokenPrice);
                    cmd.Parameters.AddWithValue("@loyalty_points", 0);
                    CreditPlusType = "A";
                    log.LogVariableState("@bonus", 0);
                    log.LogVariableState("@credits", BonusCount * TokenPrice);
                    log.LogVariableState("@loyalty_points", 0);
                }
                else if (creditBonus == "B")
                {
                    cmd.Parameters.AddWithValue("@bonus", BonusCount * TokenPrice);
                    cmd.Parameters.AddWithValue("@credits", 0);
                    cmd.Parameters.AddWithValue("@loyalty_points", 0);
                    CreditPlusType = "B";
                    log.LogVariableState("@bonus", BonusCount * TokenPrice);
                    log.LogVariableState("@credits", 0);
                    log.LogVariableState("@loyalty_points", 0);
                }
                else if (creditBonus == "L")
                {
                    cmd.Parameters.AddWithValue("@bonus", 0);
                    cmd.Parameters.AddWithValue("@credits", 0);
                    cmd.Parameters.AddWithValue("@loyalty_points", BonusCount * TokenPrice);
                    CreditPlusType = "L";
                    log.LogVariableState("@bonus", 0);
                    log.LogVariableState("@credits", 0);
                    log.LogVariableState("@loyalty_points", BonusCount * TokenPrice);
                }
                else if (creditBonus == "RP" || creditBonus == "NP") // non / refundable creditplus
                {
                    cmd.Parameters.AddWithValue("@bonus", 0);
                    cmd.Parameters.AddWithValue("@credits", 0);
                    cmd.Parameters.AddWithValue("@loyalty_points", 0);
                    CreditPlusType = "A";
                    log.LogVariableState("@bonus", 0);
                    log.LogVariableState("@credits", 0);
                    log.LogVariableState("@loyalty_points", 0);
                }

                if (createTrx)
                {
                    cmd.ExecuteNonQuery();//TrxLines
                }

                //if (creditBonus == "C" || creditBonus == "B" || creditBonus == "L")
                //{
                //    cmd.CommandText = @"update cards set credits = isnull(credits, 0) + @credits, 
                //                                         bonus = isnull(bonus, 0) + @bonus, 
                //                                         loyalty_points = isnull(loyalty_points, 0) + @loyalty_points, 
                //                                         last_update_time = getdate() " +
                //                        "where card_id = @cardId ";
                //    cmd.ExecuteNonQuery();
                //}
                //else
                //{

                cmd.Parameters.AddWithValue("@CreditPlus", BonusCount * TokenPrice);
                cmd.Parameters.AddWithValue("@Refundable", (creditBonus == "NP" ? "N" : "Y"));
                cmd.Parameters.AddWithValue("@Remarks", "Token out Credit Plus");
                cmd.Parameters.AddWithValue("@CreditPlusType", CreditPlusType); //"A"

                int updateRecs = 0;
                cmd.CommandText = @"update CardCreditPlus 
                                       set CreditPlus = CreditPlus + @CreditPlus, 
                                           CreditPlusBalance = CreditPlusBalance + @CreditPlus,
                                           LastupdatedDate = getdate()
                                    where CardCreditPlusId = (select top 1 CardCreditPlusId 
                                                                from CardCreditPlus 
                                                                where card_id = @cardId
                                                                and CreditPlusType = @CreditPlusType
                                                                and remarks = 'Token out Credit Plus'
                                                                and (
									                                    (PeriodTo IS NOT NULL 
									                                     AND PeriodTo = DATEADD(HH, 6, DATEADD(dd, DATEDIFF(dd, 0, getdate()), 0))
									                                     )
								                                     OR
									                                    (PeriodTo IS NULL 
									                                     AND CreationDate BETWEEN CASE WHEN datepart(HH, getdate()) between 0 and 5
																                                       THEN DATEADD(HH, 6, DATEADD(dd, DATEDIFF(dd, 0, getdate() - 1), 0))
																                                       ELSE DATEADD(HH, 6, DATEADD(dd, DATEDIFF(dd, 0, getdate()), 0))
																                                    END
														                                       AND CASE WHEN datepart(HH, getdate()) between 0 and 5
																	                                    THEN DATEADD(HH, 6, DATEADD(dd, DATEDIFF(dd, 0, getdate()), 0))
																	                                    ELSE DATEADD(HH, 6, DATEADD(dd, DATEDIFF(dd, 0, getdate() + 1), 0))
																                                    END
									                                    )
								                                    )
                                                                order by CreationDate desc
                                                                )";

                if (createTrx == false)
                {
                    updateRecs = cmd.ExecuteNonQuery();//perform update CCP if CreateTrx is false
                }

                if (updateRecs == 0)
                {
                    cmd.CommandText = @"insert into CardCreditPlus (Card_Id, CreditPlus, CreditPlusBalance, Refundable, TicketAllowed, Remarks, TrxId, LineId, CreditPlusType, CreationDate, LastupdatedDate, LastUpdatedBy) 
                                        select card_Id, @CreditPlus, @CreditPlus, @Refundable, 1, @Remarks, @TrxId, @TrxLineId, @CreditPlusType, getdate(), getdate() , (select user_id from users where LoginId = 'External POS')
                                        from cards 
                                        where card_Id = @cardId";
                    cmd.Parameters.AddWithValue("@TrxLineId", localTrxId == -1 ? (object)DBNull.Value : 1);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    Card updateCard = new Card(card_id, "", Utilities, SQLTrx);
                    updateCard.updateCardTime(SQLTrx);
                    log.LogVariableState("Card Updated for Credit plus update scenario in Tokenout setup: ", updateCard.card_id);
                }

                log.LogVariableState("@CreditPlus", BonusCount * TokenPrice);
                log.LogVariableState("@Refundable", (creditBonus == "NP" ? "N" : "Y"));
                log.LogVariableState("@Remarks", "Token out Credit Plus");
                log.LogVariableState("@CreditPlusType", CreditPlusType);
                //}

                if (createTrx)
                {
                    //Create GamePlay transaction mapping entry
                    if (!createTransactionGamePlayRecord(gamePlayId, localTrxId, 1, GameServerEnvironment, SQLTrx, ref message, Utilities))
                        throw new Exception(message);
                }
                SQLTrx.Commit();
                cmd.Connection.Close();
                log.LogVariableState("Message", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                if (SQLTrx != null && SQLTrx.Connection != null)
                    SQLTrx.Rollback();
                cmd.Connection.Close();
                log.Error("Error occured while creating GamePlay Token Out Transaction", ex);
                log.LogVariableState("Message", message);
                log.LogMethodExit(false);
                return false;
            }

        }

        public static bool createTransactionGamePlayRecord(long gamePlayId, int trxId, int lineId, GameServerEnvironment GameServerEnvironment, SqlTransaction sqlTrx, ref string message, Utilities Utilities)
        {
            log.LogMethodEntry(gamePlayId, trxId, lineId, GameServerEnvironment, message);
            log.Debug("Initiating command and connection objects");
            SqlCommand cmd = Utilities.getCommand(sqlTrx);
            log.Debug("Created command and connection objects");
            try
            {
                cmd.CommandText = @"insert into TransactionLineGamePlayMapping(TrxId, LineId, GamePlayId, IsActive,
                                                       guid, site_id, creationDate, createdBy, LastUpdateDate, LastUpdatedBy) 
                                          values (@TrxId, @LineId, @gameplayId, 1, newid(), @siteId, getdate(), @user, getdate(), @user)";

                cmd.Parameters.AddWithValue("@TrxId", trxId);
                log.LogVariableState("@TrxId", trxId);

                cmd.Parameters.AddWithValue("@LineId", lineId);
                log.LogVariableState("@LineId", lineId);

                cmd.Parameters.AddWithValue("@gameplayId", gamePlayId);
                log.LogVariableState("@gameplayId", gamePlayId);

                cmd.Parameters.AddWithValue("@user", "semnox");
                object site_id = DBNull.Value;
                if (Utilities.ParafaitEnv.IsCorporate == false || Utilities.ParafaitEnv.SiteId <= 0)
                    site_id = DBNull.Value;
                else
                    site_id = Utilities.ParafaitEnv.SiteId;
                cmd.Parameters.AddWithValue("@siteId", site_id);

                log.Debug("Calling ExecuteNonQuery");
                cmd.ExecuteNonQuery();
                log.Debug("Calling ExecuteNonQuery completed");
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                log.Error(ex);
                cmd.Connection.Close();
                log.Error("Error occured while creating GamePlay Token Out Transaction", ex);
                log.LogVariableState("Message", message);
                log.LogMethodExit(false);
                return false;
            }
        }
    }
}

