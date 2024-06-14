/********************************************************************************************
 * Project Name - RedemptionCurrencyRule BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By       Remarks          
 *********************************************************************************************
 *2.70.2        20-Aug-2019      Dakshakh         Created 
 *2.110.0     05-Oct-2020   Mushahid Faizan        Modified as per 3 tier standards, Added methods for Pagination and Excel Sheet functionalities,
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Inventory;


namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// Business logic for RedemptionCurrencyRuleBL class.
    /// </summary>
    public class RedemptionCurrencyRuleBL
    {
        private RedemptionCurrencyRuleDTO redemptionCurrencyRuleDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SqlTransaction sqlTransaction;

        /// <summary>
        /// Default constructor of RedemptionCurrencyRuleBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private RedemptionCurrencyRuleBL(ExecutionContext executionContext, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext);
            this.redemptionCurrencyRuleDTO = new RedemptionCurrencyRuleDTO();
            this.executionContext = executionContext;
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the redemptionCurrencyRule id as the parameter
        /// Would fetch the redemptionCurrencyRule object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="redemptionCurrencyRuleId">redemptionCurrencyRuleId</param>
        /// <param name="loadChildRecords">whether to load the active child records</param>
        /// <param name="activeChildRecords">whether to load the child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public RedemptionCurrencyRuleBL(ExecutionContext executionContext, int redemptionCurrencyRuleId, 
                         bool loadChildRecords = false, bool activeChildRecords = true, 
                         SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, redemptionCurrencyRuleId, loadChildRecords, activeChildRecords, sqlTransaction);
            RedemptionCurrencyRuleDataHandler redemptionCurrencyRuleDataHandler = new RedemptionCurrencyRuleDataHandler(sqlTransaction);
            redemptionCurrencyRuleDTO = redemptionCurrencyRuleDataHandler.GetRedemptionCurrencyRule(redemptionCurrencyRuleId);
            if (redemptionCurrencyRuleDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "RedemptionCurrencyRule", redemptionCurrencyRuleId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (redemptionCurrencyRuleDTO != null && loadChildRecords)
            {
                LoadRuleDetails(sqlTransaction, activeChildRecords);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// LoadRuleDetails
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        private void LoadRuleDetails(SqlTransaction sqlTransaction, bool activeChildRecords)
        {
            log.LogMethodEntry(sqlTransaction, activeChildRecords);
            RedemptionCurrencyRuleDetailListBL redemptionCurrencyRuleDetailListBL = new RedemptionCurrencyRuleDetailListBL(executionContext);
            List<KeyValuePair<RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters, string>> searchParameters = new List<KeyValuePair<RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters, string>>();
            searchParameters.Add(new KeyValuePair<RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters, string>(RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters.REDEMPTION_CURRENCY_RULE_ID, redemptionCurrencyRuleDTO.RedemptionCurrencyRuleId.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters, string>(RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters.IS_ACTIVE, "1"));

            }
            redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList = redemptionCurrencyRuleDetailListBL.GetAllRedemptionCurrencyRuleDetailList(searchParameters, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates RedemptionCurrencyRuleBL object using the RedemptionCurrencyRuleDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="redemptionCurrencyRuleDTO">RedemptionCurrencyRuleDTO object</param>
        public RedemptionCurrencyRuleBL(ExecutionContext executionContext, RedemptionCurrencyRuleDTO redemptionCurrencyRuleDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(redemptionCurrencyRuleDTO, executionContext);
            this.redemptionCurrencyRuleDTO = redemptionCurrencyRuleDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates RedemptionCurrencyRuleBL object using the RedemptionCurrencyRuleDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="redemptionCurrencyRuleDTO">RedemptionCurrencyRuleDTO object</param>
        public RedemptionCurrencyRuleBL(ExecutionContext executionContext, RedemptionCurrencyRuleDTO redemptionCurrencyRuleDTO, bool buildChildDetails = false, bool activeChildRecords = true)
            : this(executionContext)
        {
            log.LogMethodEntry(redemptionCurrencyRuleDTO, executionContext);
            this.redemptionCurrencyRuleDTO = redemptionCurrencyRuleDTO;
            if (buildChildDetails && redemptionCurrencyRuleDTO != null && redemptionCurrencyRuleDTO.RedemptionCurrencyRuleId != -1)
            {
                LoadRuleDetails(sqlTransaction, activeChildRecords);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the RedemptionCurrencyRule
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            Validate(sqlTransaction);
            LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
            RedemptionCurrencyRuleDataHandler redemptionCurrencyRuleDataHandler = new RedemptionCurrencyRuleDataHandler(sqlTransaction);
            if (redemptionCurrencyRuleDTO.RedemptionCurrencyRuleId < 0)
            {
                redemptionCurrencyRuleDTO = redemptionCurrencyRuleDataHandler.InsertRedemptionCurrencyRule(redemptionCurrencyRuleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                redemptionCurrencyRuleDTO.AcceptChanges();
            }
            else
            {
                if (redemptionCurrencyRuleDTO.IsChanged)
                {
                    redemptionCurrencyRuleDTO = redemptionCurrencyRuleDataHandler.UpdateRedemptionCurrencyRule(redemptionCurrencyRuleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    redemptionCurrencyRuleDTO.AcceptChanges();
                }
            }
            if (redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList != null && redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList.Any())
            {
                foreach (var redemptionCurrencyRuleDetailDTO in redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList)
                {
                    if ((redemptionCurrencyRuleDetailDTO.IsChanged || redemptionCurrencyRuleDetailDTO.RedemptionCurrencyRuleDetailId < 0))
                    {
                        redemptionCurrencyRuleDetailDTO.RedemptionCurrencyRuleId = redemptionCurrencyRuleDTO.RedemptionCurrencyRuleId;
                        RedemptionCurrencyRuleDetailBL redemptionCurrencyRuleDetailBL = new RedemptionCurrencyRuleDetailBL(executionContext, redemptionCurrencyRuleDetailDTO);
                        redemptionCurrencyRuleDetailBL.Save(sqlTransaction);
                    }
                }
            }
            if (!string.IsNullOrEmpty(redemptionCurrencyRuleDTO.Guid))
            {
                InventoryActivityLogDTO InventoryActivityLogDTO = new InventoryActivityLogDTO(serverTimeObject.GetServerDateTime(), "RedemptionCurrencyRule Inserted",
                                                         redemptionCurrencyRuleDTO.Guid, false, executionContext.GetSiteId(), "RedemptionCurrencyRule", -1, redemptionCurrencyRuleDTO.RedemptionCurrencyRuleId + ":" + redemptionCurrencyRuleDTO.RedemptionCurrencyRuleName.ToString(), -1, executionContext.GetUserId(),
                                                         serverTimeObject.GetServerDateTime(), executionContext.GetUserId(), serverTimeObject.GetServerDateTime());


                InventoryActivityLogBL inventoryActivityLogBL = new InventoryActivityLogBL(executionContext, InventoryActivityLogDTO);
                inventoryActivityLogBL.Save(sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the redemptionCurrencyRuleDetail. returns validation errors if any of the field values not not valid.
        /// </summary>
        public void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            string errorMessage = string.Empty;
            if (redemptionCurrencyRuleDTO == null)
            {
                log.Error("Redemption currency rules are missing");
                errorMessage = MessageContainerList.GetMessage(executionContext, 2248);//"Redemption currency rules are missing"
                throw new ValidationException(errorMessage);
            }
            if (redemptionCurrencyRuleDTO.RedemptionCurrencyRuleId < 0 || redemptionCurrencyRuleDTO.IsChanged || redemptionCurrencyRuleDTO.IsChangedRecursive)
            {
                if (string.IsNullOrEmpty(redemptionCurrencyRuleDTO.RedemptionCurrencyRuleName))
                {
                    log.Error("Enter valid RedemptionCurrencyRuleName for the redemption currency rule record");
                    errorMessage = MessageContainerList.GetMessage(executionContext, 2249);
                    throw new ValidationException(errorMessage);
                }
                if (string.IsNullOrEmpty(redemptionCurrencyRuleDTO.RedemptionCurrencyRuleName) == false)
                {
                    if (redemptionCurrencyRuleDTO.RedemptionCurrencyRuleName.Length > 50)
                    {
                        log.Debug("Rule name must not exceed 50 characters");
                        errorMessage = MessageContainerList.GetMessage(executionContext, 2295);
                        throw new ValidationException(errorMessage);
                    }
                }
                if (string.IsNullOrEmpty(redemptionCurrencyRuleDTO.Description))
                {
                    log.Error("Enter valid Description for the redemption currency rule record");
                    errorMessage = MessageContainerList.GetMessage(executionContext, 2250);
                    throw new ValidationException(errorMessage);
                }
                if (string.IsNullOrEmpty(redemptionCurrencyRuleDTO.Description) == false)
                {
                    if (redemptionCurrencyRuleDTO.Description.Length > 50)
                    {
                        log.Error("Description must not exceed 50 characters");
                        errorMessage = MessageContainerList.GetMessage(executionContext, 2296);
                        throw new ValidationException(errorMessage);
                    }
                }
                if (redemptionCurrencyRuleDTO.Priority <= 0)
                {
                    log.Error("Enter valid Priority");
                    errorMessage = MessageContainerList.GetMessage(executionContext, 2259);
                    throw new ValidationException(errorMessage);
                }
                if (redemptionCurrencyRuleDTO.Percentage <= 0 && redemptionCurrencyRuleDTO.Amount <= 0)
                {
                    log.Error("Enter valid percentage or amount for the redemption currency rule record");
                    errorMessage = MessageContainerList.GetMessage(executionContext, 2251);
                    throw new ValidationException(errorMessage);
                }
                if (redemptionCurrencyRuleDTO.Percentage > 0 && redemptionCurrencyRuleDTO.Amount > 0)
                {
                    log.Error("Enter valid percentage or amount for the redemption currency rule record");
                    errorMessage = MessageContainerList.GetMessage(executionContext, 2251);
                    throw new ValidationException(errorMessage);
                }
                RedemptionCurrencyRuleDetailListBL redemptionCurrencyRuleDetailListBL = new RedemptionCurrencyRuleDetailListBL(executionContext);
                List<KeyValuePair<RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters, string>> searchParameters = new List<KeyValuePair<RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters, string>>();
                searchParameters.Add(new KeyValuePair<RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters, string>(RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters.REDEMPTION_CURRENCY_RULE_ID, redemptionCurrencyRuleDTO.RedemptionCurrencyRuleId.ToString()));
                searchParameters.Add(new KeyValuePair<RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters, string>(RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters.IS_ACTIVE, "1"));
                List<RedemptionCurrencyRuleDetailDTO> redemptionCurrencyRuleDetailExistDTOList = redemptionCurrencyRuleDetailListBL.GetAllRedemptionCurrencyRuleDetailList(searchParameters);
                if (redemptionCurrencyRuleDTO != null
                   && redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList != null && redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList.Any()
                   && ((redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList.Exists(rline => rline.IsActive == true)) == false) && (redemptionCurrencyRuleDetailExistDTOList.Count == redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList.Count))
                {
                    log.Error("Enter valid percentage or amount for the redemption currency rule record");
                    errorMessage = MessageContainerList.GetMessage(executionContext, 2298);//Minimum one rule detail must be active
                    throw new ValidationException(errorMessage);
                }
                if (redemptionCurrencyRuleDTO != null
                   && redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList != null && redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList.Any())
                {
                    List<int> currencyIdList = redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList.Select(rD => rD.CurrencyId).Distinct().ToList();

                    if (currencyIdList != null && currencyIdList.Any())
                    {
                        for (int i = 0; i < currencyIdList.Count; i++)
                        {
                            if (redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList.Where(rD => rD.CurrencyId == currencyIdList[i]).Count() > 1)
                            {
                                log.Error("Currency " + redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList.Find(rD => rD.CurrencyId == currencyIdList[i]).CurrencyName + " is having more than one entry. Please update existing entry instead of adding new entry");
                                errorMessage = MessageContainerList.GetMessage(executionContext, 2310,
                                                                          redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList.Find(rD => rD.CurrencyId == currencyIdList[i]).CurrencyName);
                                throw new ValidationException(errorMessage);
                            }
                        }
                    }
                }
                if (redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList != null && redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList.Count > 0)
                {
                    foreach (RedemptionCurrencyRuleDetailDTO redemptionCurrencyRuleDetailDTO in redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList)
                    {
                        RedemptionCurrencyRuleDetailBL redemptionCurrencyRuleDetailBL = new RedemptionCurrencyRuleDetailBL(executionContext, redemptionCurrencyRuleDetailDTO);
                        redemptionCurrencyRuleDetailBL.Validate();
                    }
                    Double? Amount = 0;
                    RedemptionCurrencyList redemptionCurrencyList = new RedemptionCurrencyList(executionContext);
                    List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> redemptionCurrencySearchParams = new List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>>();
                    redemptionCurrencySearchParams.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    redemptionCurrencySearchParams.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.ISACTIVE, "1"));
                    List<RedemptionCurrencyDTO> redemptionCurrencyDTOList = redemptionCurrencyList.GetAllRedemptionCurrency(redemptionCurrencySearchParams);
                    foreach (RedemptionCurrencyRuleDetailDTO redemptionCurrencyRuleDetailDTO in redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList)
                    {
                        if (redemptionCurrencyRuleDTO.IsActive == false
                           || (redemptionCurrencyRuleDetailDTO.IsActive == true && ((redemptionCurrencyDTOList.Exists(Rc => Rc.CurrencyId == redemptionCurrencyRuleDetailDTO.CurrencyId)))
                           || redemptionCurrencyRuleDetailDTO.IsActive == false && (redemptionCurrencyDTOList.Find(Rc => Rc.CurrencyId == redemptionCurrencyRuleDetailDTO.CurrencyId) != null
                           && redemptionCurrencyDTOList.Find(Rc => Rc.CurrencyId == redemptionCurrencyRuleDetailDTO.CurrencyId).IsActive == true))
                           || redemptionCurrencyRuleDetailDTO.IsActive == false)
                        {
                            if (redemptionCurrencyRuleDetailDTO.IsActive == true)
                            {
                                if (redemptionCurrencyDTOList.Exists(sD => sD.CurrencyId == redemptionCurrencyRuleDetailDTO.CurrencyId))
                                {
                                    Double ValueInTickets = redemptionCurrencyDTOList.Find(rD => rD.CurrencyId == redemptionCurrencyRuleDetailDTO.CurrencyId).ValueInTickets;
                                    Amount = Amount + (ValueInTickets * redemptionCurrencyRuleDetailDTO.Quantity);
                                }
                            }
                        }
                        else
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2605));//Currency rule cannot have inactive currency record
                        }
                    }
                    if (Amount >= (Double)redemptionCurrencyRuleDTO.Amount && (Double)redemptionCurrencyRuleDTO.Percentage == 0)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2279, redemptionCurrencyRuleDTO.RedemptionCurrencyRuleId.ToString() == "-1" ? String.Empty : redemptionCurrencyRuleDTO.RedemptionCurrencyRuleId.ToString(), redemptionCurrencyRuleDTO.RedemptionCurrencyRuleName));
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Is Rule Applicable
        /// </summary>
        /// <param name="redemptionCurrencyIdList">redemptionCurrencyDTOList</param>
        /// <returns></returns> 
        public List<KeyValuePair<int, List<int>>> IsRuleApplicable(List<int> redemptionCurrencyIdList)
        {
            log.LogMethodEntry(redemptionCurrencyIdList);
            List<KeyValuePair<int, List<int>>> appliedRedemptionCurrencyIdList = new List<KeyValuePair<int, List<int>>>();
            List<int> finalApplicableRedemptionCurrencyIdList = new List<int>();
            if (ValidateRule() == true)
            {
                bool matched = false;
                int qty = 0;
                do
                {
                    List<int> applicableRedemptionCurrencyIdList = new List<int>();
                    matched = false;
                    foreach (RedemptionCurrencyRuleDetailDTO redemptionCurrencyRuleDetailDTO in redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList)
                    {
                        List<int> matchingCurrencyIdList = redemptionCurrencyIdList.Where(rcd => rcd == redemptionCurrencyRuleDetailDTO.CurrencyId).ToList();
                        if (matchingCurrencyIdList != null && matchingCurrencyIdList.Count > 0)
                        {
                            if (matchingCurrencyIdList.Count >= redemptionCurrencyRuleDetailDTO.Quantity)
                            {
                                matched = true;
                                int counter = 1;
                                foreach (int redemptionCurrencyId in matchingCurrencyIdList)
                                {
                                    applicableRedemptionCurrencyIdList.Add(redemptionCurrencyId);
                                    redemptionCurrencyIdList.Remove(redemptionCurrencyId);
                                    if (counter == redemptionCurrencyRuleDetailDTO.Quantity)
                                    {
                                        break;
                                    }
                                    counter++;
                                }
                            }
                            else
                            {
                                matched = false; //if there is partial match add the removed currency ids back the main list
                                foreach (int redemptionCurrencyId in applicableRedemptionCurrencyIdList)
                                {
                                    redemptionCurrencyIdList.Add(redemptionCurrencyId);
                                }
                                applicableRedemptionCurrencyIdList.Clear();
                                break;
                            }
                        }
                        else
                        {
                            matched = false;//if there is partial match add the removed currency ids back the main list
                            foreach (int redemptionCurrencyId in applicableRedemptionCurrencyIdList)
                            {
                                redemptionCurrencyIdList.Add(redemptionCurrencyId);
                            }
                            applicableRedemptionCurrencyIdList.Clear();
                            break;
                        }

                    }
                    if (applicableRedemptionCurrencyIdList != null && applicableRedemptionCurrencyIdList.Any())
                    {
                        qty++;
                        finalApplicableRedemptionCurrencyIdList.AddRange(applicableRedemptionCurrencyIdList);
                    }
                } while (redemptionCurrencyRuleDTO.Cumulative && matched);

                if (finalApplicableRedemptionCurrencyIdList != null && finalApplicableRedemptionCurrencyIdList.Count > 0)
                {
                    appliedRedemptionCurrencyIdList.Add(new KeyValuePair<int, List<int>>(qty, finalApplicableRedemptionCurrencyIdList));
                }
            }
            log.LogMethodExit(appliedRedemptionCurrencyIdList);
            return appliedRedemptionCurrencyIdList;
        }

        /// <summary>
        /// ValidateRule
        /// </summary>
        /// <returns></returns>
        public bool ValidateRule()
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (this.redemptionCurrencyRuleDTO == null)
            {
                errorMessage = MessageContainerList.GetMessage(executionContext, 2261, MessageContainerList.GetMessage(executionContext, "redemptionCurrencyRuleDetailDTO")); //Redemption Currency Rules not available
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Redemption"), MessageContainerList.GetMessage(executionContext, "DTO"), errorMessage));
            }
            if (this.redemptionCurrencyRuleDTO != null && this.redemptionCurrencyRuleDTO.RedemptionCurrencyRuleId == -1)
            {
                errorMessage = MessageContainerList.GetMessage(executionContext, 2262, MessageContainerList.GetMessage(executionContext, "redemptionCurrencyRuleDetailDTO")); //Redemption Currency Rules not saved
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Redemption"), MessageContainerList.GetMessage(executionContext, "DTO"), errorMessage));
            }
            if (this.redemptionCurrencyRuleDTO != null
                    && this.redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList != null
                    && this.redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList.Count == 0)
            {
                errorMessage = MessageContainerList.GetMessage(executionContext, 2263, MessageContainerList.GetMessage(executionContext, "redemptionCurrencyRuleDetailDTO")); //Redemption Currency Rule Details not available
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Redemption"), MessageContainerList.GetMessage(executionContext, "DTO"), errorMessage));
            }
            if (this.redemptionCurrencyRuleDTO != null
                    && this.redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList != null
                    && (this.redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList.Exists(rline => rline.IsActive == true)) == false)
            {
                errorMessage = MessageContainerList.GetMessage(executionContext, 2298, MessageContainerList.GetMessage(executionContext, "redemptionCurrencyRuleDetailDTO"));//Minimum one rule detail must be active
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Redemption"), MessageContainerList.GetMessage(executionContext, "DTO"), errorMessage));
            }
            if (validationErrorList != null && validationErrorList.Count > 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "ValidationError"), validationErrorList);
            }
            log.LogMethodExit(true);
            return true;
        }

        /// <summary>                               
        /// Get priority
        /// </summary>
        /// <returns></returns>
        public int GetPriority()
        {
            log.LogMethodEntry();
            int priority = redemptionCurrencyRuleDTO.Priority;
            log.LogMethodExit(priority);
            return priority;
        }

        /// <summary>
        /// GetRuleTicket
        /// </summary>
        /// <param name="redemptionCurrencyRuleDTO">redemptionCurrencyRuleDTO</param>
        /// <returns>Rule ticket</returns>
        public int GetRuleTicket()
        {
            log.LogMethodEntry(this.redemptionCurrencyRuleDTO);
            int ruleTicketValue = 0;
            try
            {
                if (ValidateRule())
                {
                    if (redemptionCurrencyRuleDTO.Amount > 0)
                    {
                        ruleTicketValue = Convert.ToInt32(redemptionCurrencyRuleDTO.Amount);
                    }
                    else if (redemptionCurrencyRuleDTO.Percentage > 0)
                    {
                        int currencyRuleTicketValue = 0;
                        foreach (RedemptionCurrencyRuleDetailDTO redemptionCurrencyRuleDetailDTO in this.redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList)
                        {
                            currencyRuleTicketValue = currencyRuleTicketValue + Convert.ToInt32(redemptionCurrencyRuleDetailDTO.ValueInTickets);
                        }
                        ruleTicketValue = Convert.ToInt32((redemptionCurrencyRuleDTO.Percentage / 100) * currencyRuleTicketValue);
                        ruleTicketValue = currencyRuleTicketValue + ruleTicketValue;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(ruleTicketValue);
            return ruleTicketValue;
        }

        /// <summary>
        /// Get Rule Delta Ticket
        /// </summary>
        /// <param name="redemptionCurrencyRuleDTO">redemptionCurrencyRuleDTO</param>
        /// <returns></returns>
        public int GetRuleDeltaTicket()
        {
            log.LogMethodEntry();
            int ruleDeltaTicket = 0;
            try
            {
                if (ValidateRule())
                {
                    int totalCurrencyTicketValue = 0;
                    foreach (RedemptionCurrencyRuleDetailDTO redemptionCurrencyRuleDetailDTO in redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList)
                    {
                        totalCurrencyTicketValue = totalCurrencyTicketValue + (Convert.ToInt32(redemptionCurrencyRuleDetailDTO.ValueInTickets) * Convert.ToInt32(redemptionCurrencyRuleDetailDTO.Quantity));
                    }
                    if (redemptionCurrencyRuleDTO.Amount > 0)
                    {
                        ruleDeltaTicket = (Convert.ToInt32(redemptionCurrencyRuleDTO.Amount - totalCurrencyTicketValue));
                    }
                    else if (redemptionCurrencyRuleDTO.Percentage > 0)
                    {
                        ruleDeltaTicket = Convert.ToInt32((redemptionCurrencyRuleDTO.Percentage / 100) * totalCurrencyTicketValue);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(ruleDeltaTicket);
            return ruleDeltaTicket;
        }

        /// <summary>
        /// Get RedemptionCurrencyRuleDTO
        /// </summary>
        public RedemptionCurrencyRuleDTO GetRedemptionCurrencyRuleDTO { get { return redemptionCurrencyRuleDTO; } }
    }

    /// <summary>
    /// Manages the list of RedemptionCurrencyRule
    /// </summary>
    public class RedemptionCurrencyRuleListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<RedemptionCurrencyRuleDTO> redemptionCurrencyRuleDTOList = new List<RedemptionCurrencyRuleDTO>();
        private ExecutionContext executionContext;
        private Dictionary<int, string> keyValuePairs = new Dictionary<int, string>(); // used for InventoryUI to display validation error in the excel

        public RedemptionCurrencyRuleListBL()
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        /// <summary>
        ///  Default constructor of RedemptionCurrencyRule class 
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public RedemptionCurrencyRuleListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// create the RedemptionCurrencyRule object
        /// </summary>
        /// <param name="redemptionCurrencyRuleDTOList">redemptionCurrencyRuleDTOList</param>
        /// <param name="executionContext">executionContext</param>
        public RedemptionCurrencyRuleListBL(ExecutionContext executionContext, List<RedemptionCurrencyRuleDTO> redemptionCurrencyRuleDTOList) : this(executionContext)
        {
            log.LogMethodEntry(redemptionCurrencyRuleDTOList, executionContext);
            this.redemptionCurrencyRuleDTOList = redemptionCurrencyRuleDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// This method is will return Sheet object for RedemptionCurrencyRule.
        /// <returns></returns>
        public Sheet BuildTemplate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            Sheet sheet = new Sheet();
            ///All column Headings are in a headerRow object
            Row headerRow = new Row();

            RedemptionCurrencyRuleDataHandler redemptionCurrencyRuleDataHandler = new RedemptionCurrencyRuleDataHandler(sqlTransaction);
            List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>> searchParameters = new List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>>();
            searchParameters.Add(new KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>(RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            redemptionCurrencyRuleDTOList = redemptionCurrencyRuleDataHandler.GetRedemptionCurrencyRuleDTOList(searchParameters);

            RedemptionCurrencyRuleExcelDTODefinition redemptionCurrencyRuleExcelDTODefinition = new RedemptionCurrencyRuleExcelDTODefinition(executionContext, "");
            ///Building headers from RedemptionCurrencyRuleExcelDTODefinition
            redemptionCurrencyRuleExcelDTODefinition.BuildHeaderRow(headerRow);
            sheet.AddRow(headerRow);

            if (redemptionCurrencyRuleDTOList != null && redemptionCurrencyRuleDTOList.Any())
            {
                foreach (RedemptionCurrencyRuleDTO redemptionCurrencyRuleDTO in redemptionCurrencyRuleDTOList)
                {
                    redemptionCurrencyRuleExcelDTODefinition.Configure(redemptionCurrencyRuleDTO);

                    Row row = new Row();
                    redemptionCurrencyRuleExcelDTODefinition.Serialize(row, redemptionCurrencyRuleDTO);
                    sheet.AddRow(row);
                }
            }
            log.LogMethodExit();
            return sheet;
        }

        public Dictionary<int, string> BulkUpload(Sheet sheet, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sheet, sqlTransaction);
            RedemptionCurrencyRuleExcelDTODefinition redemptionCurrencyRuleExcelDTODefinition = new RedemptionCurrencyRuleExcelDTODefinition(executionContext, "");
            List<RedemptionCurrencyRuleDTO> rowRedemptionCurrencyRuleDTOList = new List<RedemptionCurrencyRuleDTO>();
            List<RedemptionCurrencyRuleDetailDTO> rowRedemptionCurrencyRuleDetailDTOList = new List<RedemptionCurrencyRuleDetailDTO>();

            for (int i = 1; i < sheet.Rows.Count; i++)
            {
                int index = 0;
                try
                {
                    RedemptionCurrencyRuleDTO rowRedemptionCurrencyRuleDTO = (RedemptionCurrencyRuleDTO)redemptionCurrencyRuleExcelDTODefinition.Deserialize(sheet[0], sheet[i], ref index);
                    rowRedemptionCurrencyRuleDTOList.Add(rowRedemptionCurrencyRuleDTO);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                try
                {
                    if (rowRedemptionCurrencyRuleDTOList != null && rowRedemptionCurrencyRuleDTOList.Any())
                    {
                        RedemptionCurrencyRuleListBL redemptionCurrencyRuleListBL = new RedemptionCurrencyRuleListBL(executionContext, rowRedemptionCurrencyRuleDTOList);
                        redemptionCurrencyRuleListBL.Save(sqlTransaction);
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
                    throw ex;
                }
            }
            log.LogMethodExit(keyValuePairs);
            return keyValuePairs;
        }


        /// <summary>
        /// Save and Update the redemptionCurrencyRuleDTOList details
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (redemptionCurrencyRuleDTOList == null ||
                redemptionCurrencyRuleDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < redemptionCurrencyRuleDTOList.Count; i++)
            {
                var redemptionCurrencyRuleDTO = redemptionCurrencyRuleDTOList[i];
                if (redemptionCurrencyRuleDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    RedemptionCurrencyRuleBL redemptionCurrencyRuleBL = new RedemptionCurrencyRuleBL(executionContext, redemptionCurrencyRuleDTO);
                    redemptionCurrencyRuleBL.Save(sqlTransaction);
                }
                catch (SqlException sqlEx)
                {
                    log.Error(sqlEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                    if (sqlEx.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
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
                    log.Error("Error occurred while saving redemptionCurrencyRuleDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("redemptionCurrencyRuleDTO", redemptionCurrencyRuleDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the no of currencyRules matching the search Parameters
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetCurrencyRulesCount(List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            RedemptionCurrencyRuleDataHandler redemptionCurrencyRuleDataHandler = new RedemptionCurrencyRuleDataHandler(sqlTransaction);
            int currencyRulesCount = redemptionCurrencyRuleDataHandler.GetCurrencyRulesCount(searchParameters);
            log.LogMethodExit(currencyRulesCount);
            return currencyRulesCount;
        }


        /// <summary>
        ///  Returns the Get the RedemptionCurrencyRuleDTO list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="loadChildRecords">loadChildRecords</param>
        /// <param name="activeChildRecords">activeChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>RedemptionCurrencyRuleDTOList</returns>
        public List<RedemptionCurrencyRuleDTO> GetAllRedemptionCurrencyRuleList(List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>> searchParameters,
                                                         bool loadChildRecords = false, bool activeChildRecords = true, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            RedemptionCurrencyRuleDataHandler redemptionCurrencyRuleDataHandler = new RedemptionCurrencyRuleDataHandler(sqlTransaction);
            List<RedemptionCurrencyRuleDTO> redemptionCurrencyRuleDTOList = redemptionCurrencyRuleDataHandler.GetRedemptionCurrencyRuleDTOList(searchParameters, currentPage, pageSize);
            if (redemptionCurrencyRuleDTOList != null && redemptionCurrencyRuleDTOList.Any() && loadChildRecords)
            {
                Build(redemptionCurrencyRuleDTOList, activeChildRecords, currentPage, pageSize, sqlTransaction);
            }
            log.LogMethodExit(redemptionCurrencyRuleDTOList);
            return redemptionCurrencyRuleDTOList;
        }

        /// <summary>
        /// Build
        /// </summary>
        /// <param name="redemptionCurrencyRuleDTOList">redemptionCurrencyRuleDTOList</param>
        /// <param name="activeChildRecords">activeChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        private void Build(List<RedemptionCurrencyRuleDTO> redemptionCurrencyRuleDTOList, bool activeChildRecords = true, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(redemptionCurrencyRuleDTOList, activeChildRecords, sqlTransaction);
            if (redemptionCurrencyRuleDTOList != null && redemptionCurrencyRuleDTOList.Count > 0)
            {
                StringBuilder redemptionCurrencyRuleIdListStringBuilder = new StringBuilder("");
                Dictionary<int, RedemptionCurrencyRuleDTO> RedemptionCurrencyRuleIdDictionary = new Dictionary<int, RedemptionCurrencyRuleDTO>();
                string RedemptionCurrencyRuleIdSet;
                for (int i = 0; i < redemptionCurrencyRuleDTOList.Count; i++)
                {
                    if (redemptionCurrencyRuleDTOList[i].RedemptionCurrencyRuleId != -1)
                    {
                        RedemptionCurrencyRuleIdDictionary.Add(redemptionCurrencyRuleDTOList[i].RedemptionCurrencyRuleId, redemptionCurrencyRuleDTOList[i]);
                    }
                    if (i != 0)
                    {
                        redemptionCurrencyRuleIdListStringBuilder.Append(",");
                    }
                    redemptionCurrencyRuleIdListStringBuilder.Append(redemptionCurrencyRuleDTOList[i].RedemptionCurrencyRuleId.ToString());
                }
                RedemptionCurrencyRuleIdSet = redemptionCurrencyRuleIdListStringBuilder.ToString();
                RedemptionCurrencyRuleDetailListBL redemptionCurrencyRuleDetailListBL = new RedemptionCurrencyRuleDetailListBL(executionContext);
                List<KeyValuePair<RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters, string>> searchParameters = new List<KeyValuePair<RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters, string>>();
                searchParameters.Add(new KeyValuePair<RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters, string>(RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters.REDEMPTION_CURRENCY_RULE_ID_LIST, RedemptionCurrencyRuleIdSet));
                if (activeChildRecords)
                {
                    searchParameters.Add(new KeyValuePair<RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters, string>(RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters.IS_ACTIVE, "1"));
                }
                List<RedemptionCurrencyRuleDetailDTO> redemptionCurrencyRuleDetailDTOList = redemptionCurrencyRuleDetailListBL.GetAllRedemptionCurrencyRuleDetailList(searchParameters, sqlTransaction, currentPage, pageSize);
                if (redemptionCurrencyRuleDetailDTOList != null && redemptionCurrencyRuleDetailDTOList.Any())
                {
                    log.LogVariableState("RedemptionCurrencyRuleDetailDTOList", redemptionCurrencyRuleDetailDTOList);
                    foreach (var redemptionCurrencyRuleDetailDTO in redemptionCurrencyRuleDetailDTOList)
                    {
                        if (RedemptionCurrencyRuleIdDictionary.ContainsKey(redemptionCurrencyRuleDetailDTO.RedemptionCurrencyRuleId))
                        {
                            if (RedemptionCurrencyRuleIdDictionary[redemptionCurrencyRuleDetailDTO.RedemptionCurrencyRuleId].RedemptionCurrencyRuleDetailDTOList == null)
                            {
                                RedemptionCurrencyRuleIdDictionary[redemptionCurrencyRuleDetailDTO.RedemptionCurrencyRuleId].RedemptionCurrencyRuleDetailDTOList = new List<RedemptionCurrencyRuleDetailDTO>();
                            }
                            RedemptionCurrencyRuleIdDictionary[redemptionCurrencyRuleDetailDTO.RedemptionCurrencyRuleId].RedemptionCurrencyRuleDetailDTOList.Add(redemptionCurrencyRuleDetailDTO);
                        }
                    }
                }
                log.LogMethodExit();
            }
        }
        public DateTime? GetRedemptionCurrencyRuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            RedemptionCurrencyRuleDataHandler redemptionCurrencyRuleDataHandler = new RedemptionCurrencyRuleDataHandler();
            DateTime? result = redemptionCurrencyRuleDataHandler.GetRedemptionCurrencyRuleModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
