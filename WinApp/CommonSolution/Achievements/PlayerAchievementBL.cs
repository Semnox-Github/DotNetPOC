/********************************************************************************************
* Project Name - playerAchievementBL
* Description  - playerAchievementBL
* 
**************
**Version Log
**************
* Version     Date             Modified By        Remarks          
*********************************************************************************************
* 2.90.0      29-May-2020      Dakashakh raj      Modified :Ability to handle multiple projects. 
* 2.140.0     20-Sep-2021      Mathew Ninan       Added logic to record score log using scoring engine
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Parafait.CardCore;
using Semnox.Parafait.Product;
using Semnox.Parafait.Game;
using System.Data;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Transaction;
using Semnox.Core.Utilities;
using System.Globalization;


namespace Semnox.Parafait.Achievements
{
    public class PlayerAchievementBL
    {
        private PlayerAchievement playerAchievement;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        
        /// <summary>
        /// PlayerAchievementBL Constructor
        /// </summary>
        public PlayerAchievementBL()
        {
            log.Debug("Starts-PlayerAchievementBL() default constructor");
            playerAchievement = null;
            log.Debug("Ends-PlayerAchievementBL() default constructor");
        }

        /// <summary>
        /// PlayerAchievementBL Constructor
        /// </summary>
        public PlayerAchievementBL(ExecutionContext executionContext)
        {
            log.Debug("Starts-PlayerAchievementBL() default constructor");
            this.executionContext = executionContext;
            log.Debug("Ends-PlayerAchievementBL() default constructor");
        }

        /// <summary>
        /// ValidatePlayerCard
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="classExternalSystemReference"></param>
        /// <returns></returns>
        public PlayerAchievement ValidatePlayerCard(string cardNumber, string classExternalSystemReference)
        {
            log.LogMethodEntry(cardNumber, classExternalSystemReference);

            AchievementParams achievementParams = new AchievementParams();
            achievementParams.AchievementClassExternalSystemReference = classExternalSystemReference;
            achievementParams.Score = 0;
            PlayerAchievement playerAchievement = new PlayerAchievement();
            playerAchievement= LoadAchievementScoreLog(achievementParams, cardNumber);
            log.LogMethodExit(playerAchievement);
            return playerAchievement;

        }


        private PlayerAchievement ValidatePlayerCardCheck(string cardNumber, string classExternalSystemReference)
        {
            log.LogMethodEntry(cardNumber, classExternalSystemReference);
            PlayerAchievement pLayerAchievement = new PlayerAchievement();

            CardCoreDTO cardCoreDTO = new CardCoreBL(cardNumber).GetCardCoreDTO;
            if (cardCoreDTO.CardId == -1 || cardCoreDTO.Valid_flag.ToString() == "N" || (cardCoreDTO.ExpiryDate.Date > DateTime.MinValue && cardCoreDTO.ExpiryDate.Date <= DateTime.Now.Date))
            {
                pLayerAchievement.Status = "FAILED";
                pLayerAchievement.ErrorMessage = "Invalid Card! ";
            }

            pLayerAchievement.CardList.Add(cardCoreDTO);
            log.LogMethodExit(pLayerAchievement);
            return pLayerAchievement;
        }


        /// <summary>
        /// Load Achievement ScoreLog
        /// </summary>
        /// <param name="achievementParams"></param>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public PlayerAchievement LoadAchievementScoreLog(AchievementParams achievementParams, string cardNumber, AchievementScoreLogDTO achScoreLogDTO = null)
        {
            log.LogMethodEntry(achievementParams, cardNumber, achScoreLogDTO);
            PlayerAchievement playerAchievement = new PlayerAchievement();

            Semnox.Core.Utilities.ExecutionContext machineUserContext = Semnox.Core.Utilities.ExecutionContext.GetExecutionContext();

            //Validate Player Card()
            CardCoreDTO cardCoreDTO = new CardCoreBL(cardNumber).GetCardCoreDTO;
            if (cardCoreDTO.CardId == -1 || cardCoreDTO.Valid_flag.ToString() == "N" || (cardCoreDTO.ExpiryDate.Date > DateTime.MinValue && cardCoreDTO.ExpiryDate.Date <= DateTime.Now.Date))
            {
                playerAchievement.Status = "FAILED";
                playerAchievement.ErrorMessage = "Invalid Card! ";
                return playerAchievement;
            }

            int machineId = -1;
            int gameId = -1;

            AchievementScoreLogDTO achievementScoreLogDTO = new AchievementScoreLogDTO();
            AchievementClassParams achievementClassParams = new AchievementClassParams();

            if (achScoreLogDTO != null)
            {
                // logic when scorelog is already added and credit plus not updated

                achievementScoreLogDTO = achScoreLogDTO;
                machineId = achievementScoreLogDTO.MachineId;
                achievementClassParams.GameId = achievementParams.GameId;
                log.Info("LoadAchievementScoreLog - Existing Achievement ScoreLog" + achievementScoreLogDTO.Id.ToString() + "Score : " + achievementScoreLogDTO.Score.ToString());
            }
            else
            {

                log.Info("LoadAchievementScoreLog - New Achievement ScoreLog");

                PlayerAchievementDataHandler plAchdh = new PlayerAchievementDataHandler();
                machineId = plAchdh.GetAchievementClassMachine(achievementParams.AchievementClassExternalSystemReference, ref gameId);

                if (gameId == -1)
                    achievementClassParams.ExternalSystemReference = achievementParams.AchievementClassExternalSystemReference;
                else
                    achievementClassParams.GameId = gameId;
            }

            List<AchievementClassDTO> achievementClassDTOList = new AchievementClassesList(machineUserContext).GetAchievementClassList(achievementClassParams);
            if (achievementClassDTOList.Count != 1)
            {
                playerAchievement.Status = "FAILED";
                playerAchievement.ErrorMessage = "Invalid Project/Class ! ";
                log.LogMethodExit(playerAchievement);
                return playerAchievement;
            }

            //Save score Log

            AchievementScoreLog achievementScoreLog;

            ExecutionContext executionContext = Semnox.Core.Utilities.ExecutionContext.GetExecutionContext();
            executionContext.SetSiteId(-1);
            executionContext.SetUserId("Semnox");

            if (achievementParams.Score > 0 && achievementScoreLogDTO.Id == -1)
            {
                achievementScoreLogDTO.AchievementClassId = achievementClassDTOList[0].AchievementClassId;
                achievementScoreLogDTO.Score = achievementParams.Score;
                achievementScoreLogDTO.CardId = cardCoreDTO.CardId;
                achievementScoreLogDTO.LastUpdatedUser = "Semnox";
                achievementScoreLogDTO.Timestamp = DateTime.Now;
                achievementScoreLogDTO.MachineId = machineId;
                achievementScoreLog = new AchievementScoreLog(executionContext, achievementScoreLogDTO);
                achievementScoreLog.Save();
            }

            if (achievementScoreLogDTO.ConvertedToEntitlement == true)
            {
                playerAchievement.Status = "FAILED";
                playerAchievement.ErrorMessage = "Score log for Id :  " + achievementScoreLogDTO.Id + "already converted ";
                log.Info("LoadAchievementScoreLog - " + playerAchievement.ErrorMessage);

                log.LogMethodExit(playerAchievement);
                return playerAchievement;
            }

            // Update Player Progression Level

            AchievementClassParams achClassParams = new AchievementClassParams();
            achClassParams.AchievementProjectId = achievementClassDTOList[0].AchievementProjectId;
            List<AchievementClassDTO> achClassDTOProgressionList = new AchievementClassesList(executionContext).GetAchievementClassList(achClassParams);

            // Get Master Class for the Project
            AchievementClassDTO achievementClassDTO = new AchievementClassDTO();
            foreach (AchievementClassDTO achClassDTO in achClassDTOProgressionList)
            {
                if (achClassDTO.GameId == -1)
                    achievementClassDTO = achClassDTO;
            }

            achClassDTOProgressionList.Clear();
            achClassDTOProgressionList.Add(achievementClassDTOList[0]);
            achClassDTOProgressionList.Add(achievementClassDTO);

            foreach (AchievementClassDTO achClassDTO in achClassDTOProgressionList)
            {
                AchievementParams achParams = new AchievementParams();
                achParams.CardId = cardCoreDTO.CardId;
                achParams.AchievementClassId = achClassDTO.AchievementClassId;
                achParams.GameId = achClassDTO.GameId;
                achParams.CustomerId = cardCoreDTO.Customer_id;

                AchievementLevel achievementLevel = new AchievementLevel(executionContext);

                if (achievementLevel.UpdatePlayerAchievementLevel(achParams))
                {
                    if (achievementParams.Score > 0)
                    {
                        List<AchievementLevelExtended> achLevelExtendedList = achievementLevel.GetAchievementLevelListExtended(achParams);

                        int playerNewAchLevel = -1;

                        if (achLevelExtendedList.Count() == 1)
                        {
                            playerNewAchLevel = achLevelExtendedList[0].AchievementClassLevelId;
                        }

                        decimal playerPoints = achievementParams.Score * GetScoreLogConversionRatio(playerNewAchLevel, achievementScoreLogDTO.Timestamp);

                        // Create Transaction entry 

                        if (playerPoints > 0)
                        {
                            TransactionParams transactionParams = new TransactionParams
                            {
                                LoginId = "External POS",
                                CustomerId = -1,
                                ShouldCommit = true,
                                PosIdentifier = Environment.MachineName,
                                VisitDate = DateTime.Now,
                                PaymentModeId = -1,
                                TrxPaymentReference = "Achievements",
                                CloseTransaction = true,
                                OrderRemarks = "",
                                ApplyOffset = false,
                                ForceIsCorporate = false,
                                ApplySystemVisitDate = true
                            };

                            ProductsFilterParams productsFilterParams = new ProductsFilterParams();
                            productsFilterParams.ProductId = achClassDTO.ProductId;

                            Products products = new Products();
                            List<ProductsDTO> productsDTOList = products.GetProductDTOList(productsFilterParams);

                            if (productsDTOList.Count == 1)
                            {

                                List<LinkedPurchaseProductsStruct> LinkedPurchaseProductsStructList = new List<LinkedPurchaseProductsStruct>();
                                LinkedPurchaseProductsStruct parentProduct = new LinkedPurchaseProductsStruct();
                                parentProduct.ProductId = productsDTOList[0].ProductId;
                                parentProduct.ProductName = productsDTOList[0].ProductName;
                                parentProduct.Price = 0;
                                parentProduct.ProductQuantity = 1;
                                parentProduct.TaxAmount = 0;
                                parentProduct.CardNumber = cardCoreDTO.Card_number;
                                parentProduct.Credits = 0;
                                parentProduct.Guid = Guid.NewGuid().ToString();
                                parentProduct.PurchaseLineId = 1;
                                parentProduct.LinkLineId = -1;
                                parentProduct.Remarks = transactionParams.OrderRemarks;

                                LinkedPurchaseProductsStructList.Add(parentProduct);

                                TransactionCore transactionCore = new TransactionCore();
                                List<TransactionKeyValueStruct> TransactionKeyValueStructList = transactionCore.SaveTransaction(transactionParams, LinkedPurchaseProductsStructList);

                                log.Info("LoadAchievementScoreLog - Transaction Created " + cardCoreDTO.CardId + " Transaction ID : " + TransactionKeyValueStructList[0].Value);

                                int transactionId = Convert.ToInt32(TransactionKeyValueStructList[0].Value);
                                if (transactionId > 0)
                                {
                                    log.Info("LoadAchievementScoreLog - Save Credit Plus" + cardCoreDTO.CardId + " Score : " + Convert.ToDouble(playerPoints).ToString());

                                    CardCreditPlusDTO cardCreditPlusDTO = new CardCreditPlusDTO();
                                    cardCreditPlusDTO.CardId = cardCoreDTO.CardId;
                                    cardCreditPlusDTO.PeriodFrom = achievementScoreLogDTO.Timestamp;
                                    cardCreditPlusDTO.CreditPlus = Convert.ToDouble(playerPoints);
                                    cardCreditPlusDTO.CreditPlusBalance = cardCreditPlusDTO.CreditPlus;
                                    cardCreditPlusDTO.TrxId = transactionId;
                                    cardCreditPlusDTO.CreditPlusType = "T";
                                    cardCreditPlusDTO.Refundable = "N";
                                    cardCreditPlusDTO.Remarks = productsDTOList[0].ProductName;
                                    cardCreditPlusDTO.TicketAllowed = true;
                                    cardCreditPlusDTO.ExtendOnReload = "N";
                                    cardCreditPlusDTO.LineId = 1;

                                    CardCreditPlus cardCreditPlus = new CardCreditPlus(cardCreditPlusDTO);
                                    cardCreditPlus.Save(null);

                                    log.Info("LoadAchievementScoreLog - Save Credit Plus Complete " + cardCoreDTO.CardId + " Score : " + Convert.ToDouble(playerPoints).ToString());


                                    AchievementScoreLogDTO achievementScoreLogDTO1 = new AchievementScoreLog(executionContext, achievementScoreLogDTO.Id).GetAchievementScoreLogDTO;
                                    achievementScoreLogDTO1.CardCreditPlusId = cardCreditPlus.GetCardcreditPlusDTO.CardCreditPlusId;
                                    achievementScoreLogDTO1.ConvertedToEntitlement = true;
                                    achievementScoreLog = new AchievementScoreLog(executionContext, achievementScoreLogDTO1);
                                    achievementScoreLog.Save();

                                    log.Info("LoadAchievementScoreLog - Score Log Status update" + cardCoreDTO.CardId + " Score : " + Convert.ToDouble(playerPoints).ToString());

                                }
                            }

                            playerAchievement.ErrorMessage = "Load Score Success";
                        }
                    }

                }
            }


            AchievementParams achParams1 = new AchievementParams();
            achParams1.CardId = cardCoreDTO.CardId;

            List<PlayerAchievement> playerAchievementList = GetTopNAchievementPlayers(achParams1, (Convert.ToDateTime("1/1/1900", CultureInfo.InvariantCulture)),DateTime.Now.AddDays(1));
            if (playerAchievementList.Count == 1)
            {
                playerAchievement = playerAchievementList[0];
                playerAchievementList[0].ErrorMessage = playerAchievement.ErrorMessage + " | Player Achievement loaded";
            }
            else
            {
                playerAchievement.ErrorMessage += " - Error: Could not load Player Achievement ";
            }

            playerAchievement.Status = "SUCCESS";
            return playerAchievement;

        }

        /// <summary>
        /// Load score and entitlements in achievement
        /// </summary>
        /// <param name="inUtilities">Utilities</param>
        /// <param name="achievementParams">achievementParams</param>
        /// <returns>true</returns>
        public bool LoadAchievementScoreLog(Utilities inUtilities, AchievementParams achievementParams)
        {
            log.LogMethodEntry(inUtilities, achievementParams);
            if (achievementParams.AchievementClassId <= -1)
            {
                log.Error("Achievement Class is not passed. Check set up.");
                return false;
            }
            if (achievementParams.CardId <= -1)
            {
                log.Error("Card Id is not passed. Check set up.");
                return false;
            }
            AccountDTO accountDTO = new AccountBL(inUtilities.ExecutionContext, achievementParams.CardId, false, false).AccountDTO;
            if ((accountDTO.ExpiryDate != null && accountDTO.ExpiryDate < DateTime.Now)
                || accountDTO.ValidFlag == false)
            {
                log.Error("Card is invalid or expired. ");
                return false;
            }

            try
            {
                //create score log
                AchievementScoreLogDTO achievementScoreLogDTO = null;
                if (achievementParams.Score > 0)
                {
                    achievementScoreLogDTO = new AchievementScoreLogDTO(-1, achievementParams.CardId, achievementParams.AchievementClassId, -1,
                                                                                               achievementParams.Score, DateTime.Now, false, -1, true, achievementParams.ScoringEventId);
                    AchievementScoreLog achievementScoreLog = new AchievementScoreLog(inUtilities.ExecutionContext, achievementScoreLogDTO);
                    achievementScoreLog.Save();
                }

                //Create achievementLevel for the card if applicable
                AchievementLevel achievementLevel = new AchievementLevel(inUtilities.ExecutionContext);
                bool isPlayerLevelUpdated = achievementLevel.UpdatePlayerAchievementLevel(achievementParams);

                //Load Entitlements
                if (isPlayerLevelUpdated)
                {
                    AchievementLevelsList achievementLevelsList = new AchievementLevelsList(inUtilities.ExecutionContext);
                    List<KeyValuePair<AchievementLevelDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<AchievementLevelDTO.SearchByParameters, string>>();
                    searchByParameters.Add(new KeyValuePair<AchievementLevelDTO.SearchByParameters, string>(AchievementLevelDTO.SearchByParameters.CARD_ID, achievementParams.CardId.ToString()));

                    List<AchievementLevelDTO> achievementLevelDTOs = achievementLevelsList.GetAllAchievementLevels(searchByParameters);
                    if (achievementLevelDTOs != null && achievementLevelDTOs.Count > 0)
                    {
                        AchievementLevelDTO achievementLevelDTO = achievementLevelDTOs.OrderByDescending(x => x.Id).FirstOrDefault();
                        decimal playerPoints = achievementParams.Score * GetScoreLogConversionRatio(achievementLevelDTO.AchievementClassLevelId, achievementScoreLogDTO.Timestamp);
                        if (playerPoints > 0)
                        {
                            TransactionParams transactionParams = new TransactionParams
                            {
                                LoginId = "External POS",
                                CustomerId = -1,
                                ShouldCommit = true,
                                PosIdentifier = inUtilities.ExecutionContext.POSMachineName,
                                VisitDate = DateTime.Now,
                                PaymentModeId = -1,
                                TrxPaymentReference = "Achievements",
                                CloseTransaction = true,
                                OrderRemarks = "",
                                ApplyOffset = false,
                                ForceIsCorporate = false,
                                ApplySystemVisitDate = true
                            };
                            AchievementClassDTO achievementClassDTO = new AchievementClass(inUtilities.ExecutionContext, achievementParams.AchievementClassId).GetAchievementClassDTO;

                            ProductsDTO productsDTO = new Products(inUtilities.ExecutionContext, achievementClassDTO.ProductId, false, false).GetProductsDTO;
                            if (productsDTO != null)
                            {
                                List<LinkedPurchaseProductsStruct> LinkedPurchaseProductsStructList = new List<LinkedPurchaseProductsStruct>();
                                LinkedPurchaseProductsStruct parentProduct = new LinkedPurchaseProductsStruct();
                                parentProduct.ProductId = productsDTO.ProductId;
                                parentProduct.ProductName = productsDTO.ProductName;
                                parentProduct.Price = 0;
                                parentProduct.ProductQuantity = 1;
                                parentProduct.TaxAmount = 0;
                                parentProduct.CardNumber = accountDTO.TagNumber;
                                parentProduct.Credits = 0;
                                parentProduct.Guid = Guid.NewGuid().ToString();
                                parentProduct.PurchaseLineId = 1;
                                parentProduct.LinkLineId = -1;
                                parentProduct.Remarks = transactionParams.OrderRemarks;

                                LinkedPurchaseProductsStructList.Add(parentProduct);

                                TransactionCore transactionCore = new TransactionCore();
                                List<TransactionKeyValueStruct> TransactionKeyValueStructList = transactionCore.SaveTransaction(transactionParams, LinkedPurchaseProductsStructList);

                                log.Info("LoadAchievementScoreLog - Transaction Created " + accountDTO.AccountId + " Transaction ID : " + TransactionKeyValueStructList[0].Value);

                                int transactionId = Convert.ToInt32(TransactionKeyValueStructList[0].Value);
                                if (transactionId > 0)
                                {
                                    log.Info("LoadAchievementScoreLog - Save Credit Plus" + accountDTO.AccountId + " Score : " + Convert.ToDouble(playerPoints).ToString());

                                    AchievementScoreConversionsList achievementScoreConversionList = new AchievementScoreConversionsList(inUtilities.ExecutionContext);
                                    List<KeyValuePair<AchievementScoreConversionDTO.SearchByParameters, string>> achvSearchParameters = new List<KeyValuePair<AchievementScoreConversionDTO.SearchByParameters, string>>();
                                    achvSearchParameters.Add(new KeyValuePair<AchievementScoreConversionDTO.SearchByParameters, string>(AchievementScoreConversionDTO.SearchByParameters.ACHIEVEMENT_CLASS_LEVEL_ID, achievementLevelDTO.AchievementClassLevelId.ToString()));
                                    List<AchievementScoreConversionDTO> achievementScoreConversionDTOs = achievementScoreConversionList.GetAllAchievementScoreConversions(achvSearchParameters);

                                    if (!string.IsNullOrEmpty(achievementScoreConversionDTOs[0].ConversionEntitlement))
                                    {
                                        Loyalty loyalty = new Loyalty(inUtilities);
                                        loyalty.CreateGenericCreditPlusLine(accountDTO.AccountId, achievementScoreConversionDTOs[0].ConversionEntitlement,
                                            Convert.ToDouble(playerPoints), true, 0, "N", inUtilities.ExecutionContext.UserId, "Achievement Entitlement", null,
                                            DateTime.Now, transactionId, 1, true);
                                    }
                                    log.Info("LoadAchievementScoreLog - Save Credit Plus Complete " + accountDTO.AccountId + " Score : " + Convert.ToDouble(playerPoints).ToString());

                                    AchievementScoreLogDTO updateAchievementScoreLogDTO = new AchievementScoreLog(executionContext, achievementScoreLogDTO.Id).GetAchievementScoreLogDTO;
                                    AccountDTO updAccountDTO = new AccountBL(inUtilities.ExecutionContext, accountDTO.AccountId, true, true).AccountDTO;
                                    updateAchievementScoreLogDTO.CardCreditPlusId = updAccountDTO.AccountCreditPlusDTOList.OrderByDescending(x => x.AccountCreditPlusId).FirstOrDefault().AccountCreditPlusId;
                                    updateAchievementScoreLogDTO.ConvertedToEntitlement = true;
                                    AchievementScoreLog updateAchievementScoreLog = new AchievementScoreLog(executionContext, updateAchievementScoreLogDTO);
                                    updateAchievementScoreLog.Save();
                                    log.Info("LoadAchievementScoreLog - Score Log Status update" + accountDTO.AccountId + " Score : " + Convert.ToDouble(playerPoints).ToString());
                                }
                            }
                        }
                    }
                }
                log.LogMethodExit(true);
                return true;
            }
            catch(Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// GetScoreLogConversionRatio
        /// </summary>
        /// <param name="achievementClassLevelId"></param>
        /// <param name="effDate"></param>
        /// <returns></returns>
        private decimal GetScoreLogConversionRatio(int achievementClassLevelId, DateTime effDate)
        {
            log.LogMethodEntry(achievementClassLevelId, effDate);
            decimal playerConversionRatio = 0;

            AchievementScoreConversionParams achievementScoreConversionParams = new AchievementScoreConversionParams();
            achievementScoreConversionParams.AchievementClassLevelId = achievementClassLevelId;
            achievementScoreConversionParams.IsActive = true;

            AchievementScoreConversionsList achScoreConversionsList = new AchievementScoreConversionsList(executionContext);
            List<AchievementScoreConversionDTO> achScoreConversionDTOList = achScoreConversionsList.GetAllAchievementScoreConversions(achievementScoreConversionParams);

            if (achScoreConversionDTOList.Where(c => c.FromDate == DateTime.MinValue && c.ToDate == DateTime.MinValue).Count() == 1)
            {
                playerConversionRatio = achScoreConversionDTOList.Where(c => c.FromDate == DateTime.MinValue && c.ToDate == DateTime.MinValue).First().Ratio;
            }

            if (achScoreConversionDTOList.Where(c => c.FromDate.Date <= effDate.Date && c.ToDate.Date >= effDate.Date).Count() == 1)
            {
                playerConversionRatio = achScoreConversionDTOList.Where(c => c.FromDate <= effDate.Date && c.ToDate >= effDate.Date).First().Ratio;
            }
            log.LogMethodExit(playerConversionRatio);
            return playerConversionRatio;
        }



        public PlayerAchievement GetPlayerAchievement(string cardNumber, AchievementParams achievementParams)
        {
            log.LogMethodEntry(cardNumber, achievementParams);
            PlayerAchievement pLayerAchievement = new PlayerAchievement();

            try
            {
                pLayerAchievement = ValidatePlayerCardCheck(cardNumber, "");
                if (pLayerAchievement.Status != "FAILED")
                {
                    AchievementParams achParams = new AchievementParams();
                    achParams.CardId = pLayerAchievement.CardList[0].CardId;
                    achParams.ShowValidLevelsOnly = achievementParams.ShowValidLevelsOnly;
                    achParams.ProjectName = achievementParams.ProjectName;
                    List<PlayerAchievement> plAchList = new List<PlayerAchievement>();
                    plAchList = GetTopNAchievementPlayers(achParams);

                    if (plAchList.Count == 1)
                    {
                        pLayerAchievement = plAchList[0];
                    }
                }

            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }

            log.LogMethodExit(pLayerAchievement);
            return pLayerAchievement;


        }


        public List<PlayerAchievement> GetTopNAchievementPlayers(AchievementParams achievementParams, DateTime? fromDate = null, DateTime? todate = null)
        {
            log.LogMethodEntry(achievementParams, fromDate, todate);
            Semnox.Core.Utilities.ExecutionContext machineUserContext = Semnox.Core.Utilities.ExecutionContext.GetExecutionContext();
            List<PlayerAchievement> pLayerAchievementList = new List<PlayerAchievement>();
            PlayerAchievementDataHandler playerAchievementDataHandler = new PlayerAchievementDataHandler();
            DataTable dtPlayers = playerAchievementDataHandler.GetTopNPlayers(achievementParams, fromDate, todate);
            if (dtPlayers.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dtPlayers.Rows)
                {
                    PlayerAchievement pLayerAchievement = new PlayerAchievement();
                    pLayerAchievement.FirstName = "Not Registered";
                    if (dataRow["customer_id"] != DBNull.Value)
                    {
                        CustomerBL customerBL = new CustomerBL(machineUserContext, Convert.ToInt32(dataRow["customer_id"]));
                        pLayerAchievement.Id = customerBL.CustomerDTO.Id;
                        pLayerAchievement.FirstName = customerBL.CustomerDTO.FirstName;
                        pLayerAchievement.LastName = customerBL.CustomerDTO.LastName;
                        pLayerAchievement.PhotoURL = customerBL.CustomerDTO.PhotoURL;
                    }

                    pLayerAchievement.Points = Convert.ToInt32(dataRow["points"]);
                    pLayerAchievement.HistoricalPoints = Convert.ToInt32(dataRow["historicalpoints"]);

                    AchievementParams achParams = new AchievementParams();
                    achParams.CardId = Convert.ToInt32(dataRow["card_id"]);
                    achParams.ShowValidLevelsOnly = achievementParams.ShowValidLevelsOnly;
                    achParams.ProjectName = achievementParams.ProjectName;
                    pLayerAchievement.AchievementLevelList = new AchievementLevelDataHandler().GetAchievementLevelListExtended(achParams);
                    pLayerAchievement.CardList.Add(new CardCoreBL(achParams.CardId).GetCardCoreDTO);
                    pLayerAchievement.Score = playerAchievementDataHandler.GetPlayerAchievementScore(achParams);
                    pLayerAchievement.ProjectName = achievementParams.ProjectName;
                    pLayerAchievementList.Add(pLayerAchievement);

                }
            }

            log.LogMethodExit(pLayerAchievementList);
            return pLayerAchievementList;

        }

        /// <summary>
        /// UpdateAchievementScoreEntitlements
        /// Method used to convert AchievementScore to entitlements for ConvertedToEntitlement = false
        /// </summary>
        public void UpdateAchievementScoreEntitlements()
        {
            log.LogMethodEntry();

            List<KeyValuePair<AchievementScoreLogDTO.SearchByParameters, string>> achScoreLogSearchList = new List<KeyValuePair<AchievementScoreLogDTO.SearchByParameters, string>>();
            achScoreLogSearchList.Add(new KeyValuePair<AchievementScoreLogDTO.SearchByParameters, string>(AchievementScoreLogDTO.SearchByParameters.CONVERTED_TO_ENTITLEMENT, "0"));

            List<AchievementScoreLogDTO> achievementScoreLogDTOList;

            AchievementScoreLogsList achievementScoreLogsList = new AchievementScoreLogsList(executionContext);
            achievementScoreLogDTOList = achievementScoreLogsList.GetAllAchievementScoreLogs(achScoreLogSearchList);

            //log.Debug("UpdateAchievementScoreEntitlements() - Pending Entitlement Conversions" + achievementScoreLogDTOList.Count().ToString());
            log.LogMethodExit();

            foreach (AchievementScoreLogDTO achScoreLogDTO in achievementScoreLogDTOList)
            {
                if (achScoreLogDTO.ConvertedToEntitlement == false && achScoreLogDTO.CardCreditPlusId == -1)
                {
                    log.LogMethodEntry(achievementScoreLogDTOList);

                    CardCoreDTO cardDTO = new CardCoreBL(achScoreLogDTO.CardId).GetCardCoreDTO;
                    if (cardDTO.CardId > 0 && cardDTO.Valid_flag.ToString() == "Y" && (cardDTO.ExpiryDate.Date == DateTime.MinValue || cardDTO.ExpiryDate.Date <= DateTime.Now.Date))
                    {
                        MachineDTO machineDTO = new MachineList().GetMachine(achScoreLogDTO.MachineId);

                        AchievementParams achievementParams = new AchievementParams();
                        achievementParams.GameId = machineDTO.GameId;
                        achievementParams.Score = (int)achScoreLogDTO.Score;

                        log.Debug("UpdateAchievementScoreEntitlements - Send to  LoadAchievementScoreLog" + achScoreLogDTO.Id.ToString());
                        LoadAchievementScoreLog(achievementParams, cardDTO.Card_number, achScoreLogDTO);
                        log.Debug("UpdateAchievementScoreEntitlements - Completed LoadAchievementScoreLog");

                    }
                }
            }


            log.LogMethodExit();
        }

    }
}
