
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Promotions;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Game;

namespace Semnox.Parafait.Loyalty
{
    class LoyaltyEngine
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        Utilities utilities;

        public LoyaltyEngine(ExecutionContext executionContext, Utilities utilities)
        {
            this.executionContext = executionContext;
            this.utilities = utilities;
        }

        public void ProcessBatchLoyaltyRules()
        {
            log.LogMethodEntry();
            try
            {
                LoyaltyEngineDataHandler loyaltyEngineDataHandler = new LoyaltyEngineDataHandler(null);
                DateTime? lastRunTimeValue = loyaltyEngineDataHandler.GettLoyaltyEngineRunTime(executionContext.GetUserId(), executionContext.GetSiteId());
                if (lastRunTimeValue != null)
                {
                    DateTime lastRunTime = (DateTime)lastRunTimeValue;
                    DateTime currentRunTime = DateTime.Now;
                    //Check batch rules are available or not
                    LoyaltyRuleListBL loyaltyRuleListBL = new LoyaltyRuleListBL(executionContext);
                    List<KeyValuePair<LoyaltyRuleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LoyaltyRuleDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<LoyaltyRuleDTO.SearchByParameters, string>(LoyaltyRuleDTO.SearchByParameters.ACTIVE_FLAG, "Y"));
                    searchParameters.Add(new KeyValuePair<LoyaltyRuleDTO.SearchByParameters, string>(LoyaltyRuleDTO.SearchByParameters.APPLY_IMMEDIATE, "N"));
                    List<LoyaltyRuleDTO> loyaltyRuleDTOList = loyaltyRuleListBL.GetLoyaltyRuleDTOList(searchParameters);
                    if (loyaltyRuleDTOList != null && loyaltyRuleDTOList.Count > 0)
                    {

                        //Transactions
                        TransactionCore transactionCore = new TransactionCore();
                        List<TransactionDetails> transactionDetailsList = transactionCore.GetTransactionsByDateTime(lastRunTime, currentRunTime, executionContext.GetSiteId());
                        if (transactionDetailsList != null && transactionDetailsList.Count > 0)
                        {
                            foreach (TransactionDetails transactionDetails in transactionDetailsList)
                            {
                                try
                                {
                                    Semnox.Parafait.Transaction.Loyalty loyalty = new Semnox.Parafait.Transaction.Loyalty(utilities);
                                    loyalty.LoyaltyOnPurchase(transactionDetails.TransactionId, "N", null);
                                    loyalty.LoyaltyOnProductConsumption(transactionDetails.TransactionId, null, utilities.ParafaitEnv.POSMachine, "N");
                                    //capture data into LoyaltyBatchProcess
                                    LoyaltyBatchProcessDTO loyaltyBatchProcessDTO = new LoyaltyBatchProcessDTO(-1, transactionDetails.TransactionId, -1, "", executionContext.GetUserId(), DateTime.Now, executionContext.GetUserId(), DateTime.Now, executionContext.GetSiteId(), -1, false);
                                    LoyaltyBatchProcessDataHandler loyaltyBatchProcessDataHandler = new LoyaltyBatchProcessDataHandler(null);
                                    loyaltyBatchProcessDataHandler.InsertLoyaltyBatchProcess(loyaltyBatchProcessDTO, executionContext.GetUserId(), executionContext.GetSiteId());

                                }
                                catch (Exception ex)
                                {
                                    log.Error("Loyalty Error for Trx " + transactionDetails.TransactionId.ToString(), ex);
                                    //log error into table along with trx id
                                    LoyaltyBatchErrorLogDataHandler loyaltyBatchErrorLogDataHandler = new LoyaltyBatchErrorLogDataHandler(null);
                                    LoyaltyBatchErrorLogDTO loyaltyBatchErrorLogDTO = new LoyaltyBatchErrorLogDTO(-1, transactionDetails.TransactionId, -1, ex.StackTrace, "", executionContext.GetUserId(), DateTime.Now, executionContext.GetUserId(), DateTime.Now, executionContext.GetSiteId(), -1, false);
                                    loyaltyBatchErrorLogDataHandler.InsertLoyaltyBatchErrorLog(loyaltyBatchErrorLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                                }
                            }
                        }
                        //GameTime
                        GamePlayListBL gamePlayListBL = new GamePlayListBL(executionContext);
                        List<GamePlayDTO> gamePlayDTOList = gamePlayListBL.GetGamePlayByDateTime(lastRunTime.AddMinutes(-15), currentRunTime.AddMinutes(-15), executionContext.GetSiteId());
                        if (gamePlayDTOList != null && gamePlayDTOList.Count > 0)
                        {
                            foreach (GamePlayDTO gamePlayDTO in gamePlayDTOList)
                            {
                                try
                                {
                                    Semnox.Parafait.Transaction.Loyalty loyalty = new Semnox.Parafait.Transaction.Loyalty(utilities);
                                    loyalty.LoyaltyOnGamePlay(gamePlayDTO.GameplayId, "N", null);
                                    //capture data into LoyaltyBatchProcess
                                    LoyaltyBatchProcessDTO loyaltyBatchProcessDTO = new LoyaltyBatchProcessDTO(-1, -1, gamePlayDTO.GameplayId, "", executionContext.GetUserId(), DateTime.Now, executionContext.GetUserId(), DateTime.Now, executionContext.GetSiteId(), -1, false);
                                    LoyaltyBatchProcessDataHandler loyaltyBatchProcessDataHandler = new LoyaltyBatchProcessDataHandler(null);
                                    loyaltyBatchProcessDataHandler.InsertLoyaltyBatchProcess(loyaltyBatchProcessDTO, executionContext.GetUserId(), executionContext.GetSiteId());

                                }
                                catch (Exception ex)
                                {
                                    log.Error("Loyalty Error for Game play " + gamePlayDTO.GameplayId.ToString(), ex);
                                    //log error into table along with game play id
                                    LoyaltyBatchErrorLogDataHandler loyaltyBatchErrorLogDataHandler = new LoyaltyBatchErrorLogDataHandler(null);
                                    LoyaltyBatchErrorLogDTO loyaltyBatchErrorLogDTO = new LoyaltyBatchErrorLogDTO(-1, -1, gamePlayDTO.GameplayId, ex.StackTrace, "", executionContext.GetUserId(), DateTime.Now, executionContext.GetUserId(), DateTime.Now, executionContext.GetSiteId(), -1, false);
                                    loyaltyBatchErrorLogDataHandler.InsertLoyaltyBatchErrorLog(loyaltyBatchErrorLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                                }

                            }
                        }
                    }
                    //set last run time
                    try
                    {
                        loyaltyEngineDataHandler.SetLoyaltyEngineRunTime(currentRunTime, executionContext.GetUserId(), executionContext.GetSiteId());
                    }
                    catch (Exception ex)
                    {
                        log.Error("Errro while setting loyalty engine run time", ex);
                    }
                    try
                    {
                        int purgeDays=0;
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        searchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "LOYALTY_BATCH_PROCESS_PURGE"));
                        searchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "RetentionDays"));

                        List<LookupValuesDTO> lookUpValueDTOList = lookupValuesList.GetAllLookupValues(searchParams);
                        if (lookUpValueDTOList != null && lookUpValueDTOList.Count > 0)
                        {
                            try { purgeDays = Convert.ToInt32(lookUpValueDTOList[0].Description); }
                            catch (Exception ex) { log.Error("Unable to set purgeDays", ex); purgeDays = 15; }
                        }
                        else
                        {   purgeDays = 15;
                            log.Error("Unable to get purgeDays value from lookup");
                        }

                        if(purgeDays > 0)
                        {
                            DateTime purgeDate = DateTime.Now.AddDays(-purgeDays).Date;
                            DateTime purgeDateValidated;
                            if (purgeDate != null && DateTime.TryParse(purgeDate.ToString(), out purgeDateValidated))
                            {
                                 LoyaltyBatchProcessDataHandler loyaltyBatchProcessDataHandler = new LoyaltyBatchProcessDataHandler(null);
                                 loyaltyBatchProcessDataHandler.PurgeBatchProcessLog(purgeDate);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        log.Error("Errro while purging loyaltyBatchProcess data", ex);
                    }
                }
                else
                {
                    log.Error("last run time details are missing for Loyalty Rules Batch engine");
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in Loyalty Rules Batch engine",ex);
            }
            log.LogMethodExit();
        }

    }

    

}
