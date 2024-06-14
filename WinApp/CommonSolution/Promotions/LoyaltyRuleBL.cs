/********************************************************************************************
 * Project Name - Promotions
 * Description  - Business logic file for LoyaltyRule
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *****************************************************************************************************************
 *2.70        08-Jul-2019   Dakshakh raj            Modified : Added private access modifiers for data members.
 *2.70.2      06-Feb-2020   Girish Kundar           Modified : As per the 3 tier standard
 *2.80.0      31-Mar-2020   Mushahid Faizan         Modified : 3 tier changes for Rest API
 *****************************************************************************************************************/
//using Semnox.Parafait.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// Business logic for LoyaltyRule class. 
    /// </summary>
    public class LoyaltyRuleBL
    {
        private LoyaltyRuleDTO loyaltyRuleDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of LoyaltyRuleBL class
        /// </summary>
        private LoyaltyRuleBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            loyaltyRuleDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the  loyaltyRule id as the parameter
        /// Would fetch the  loyaltyRule object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public LoyaltyRuleBL(ExecutionContext executionContext, int loyaltyId, bool activeRecords = false, bool loadChildRecords = false, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, loyaltyId, sqlTransaction);
            LoyaltyRuleDataHandler loyaltyRuleDataHandler = new LoyaltyRuleDataHandler(sqlTransaction);
            loyaltyRuleDTO = loyaltyRuleDataHandler.GetLoyaltyRuleDTO(loyaltyId);
            if (loyaltyRuleDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "loyaltyRule", loyaltyId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Build child list
        /// </summary>
        /// <param name="activeChildRecords">Bool for active only records</param>
        /// <param name="sqlTransaction">sql transaction</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            LoyaltyRuleTriggerListBL loyaltyRuleTriggerListBL = new LoyaltyRuleTriggerListBL(executionContext);
            List<KeyValuePair<LoyaltyRuleTriggerDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LoyaltyRuleTriggerDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<LoyaltyRuleTriggerDTO.SearchByParameters, string>(LoyaltyRuleTriggerDTO.SearchByParameters.LOYALTY_RULE_ID, loyaltyRuleDTO.LoyaltyRuleId.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<LoyaltyRuleTriggerDTO.SearchByParameters, string>(LoyaltyRuleTriggerDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            loyaltyRuleDTO.LoyaltyRuleTriggerDTOList = loyaltyRuleTriggerListBL.GetLoyaltyRuleTriggerDTOList(searchParameters, sqlTransaction);
            LoyaltyBonusAttributeListBL loyaltyBonusAttributeListBL = new LoyaltyBonusAttributeListBL(executionContext);
            List<KeyValuePair<LoyaltyBonusAttributeDTO.SearchByParameters, string>> psearchParameters = new List<KeyValuePair<LoyaltyBonusAttributeDTO.SearchByParameters, string>>();
            psearchParameters.Add(new KeyValuePair<LoyaltyBonusAttributeDTO.SearchByParameters, string>(LoyaltyBonusAttributeDTO.SearchByParameters.LOYALTY_RULE_ID, loyaltyRuleDTO.LoyaltyRuleId.ToString()));
            if (activeChildRecords)
            {
                psearchParameters.Add(new KeyValuePair<LoyaltyBonusAttributeDTO.SearchByParameters, string>(LoyaltyBonusAttributeDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            }
            loyaltyRuleDTO.LoyaltyBonusAttributeDTOList = loyaltyBonusAttributeListBL.GetLoyaltyBonusAttributeDTOList(psearchParameters, true, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates LoyaltyRuleBL object using the LoyaltyRuleDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="loyaltyRuleDTO">LoyaltyRuleDTO object</param>
        public LoyaltyRuleBL(ExecutionContext executionContext, LoyaltyRuleDTO loyaltyRuleDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, loyaltyRuleDTO);
            this.loyaltyRuleDTO = loyaltyRuleDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the LoyaltyRule
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            LoyaltyRuleDataHandler loyaltyRuleDataHandler = new LoyaltyRuleDataHandler(sqlTransaction);
            if (loyaltyRuleDTO.IsChangedRecursive == false
                 && loyaltyRuleDTO.LoyaltyRuleId >-1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (loyaltyRuleDTO.LoyaltyRuleId < 0)
            {
                loyaltyRuleDTO = loyaltyRuleDataHandler.Insert(loyaltyRuleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                loyaltyRuleDTO.AcceptChanges();
            }
            else
            {
                if (loyaltyRuleDTO.IsChanged)
                {
                    loyaltyRuleDTO = loyaltyRuleDataHandler.Update(loyaltyRuleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    loyaltyRuleDTO.AcceptChanges();
                }
            }
            SaveChildRecords(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the child records 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveChildRecords(SqlTransaction sqlTransaction)
        {

            // for Child Records : :LoyaltyRuleTriggerDTO
            if (loyaltyRuleDTO.LoyaltyRuleTriggerDTOList != null &&
                loyaltyRuleDTO.LoyaltyRuleTriggerDTOList.Any())
            {
                List<LoyaltyRuleTriggerDTO> updatedLoyaltyRuleTriggerDTOList = new List<LoyaltyRuleTriggerDTO>();
                foreach (LoyaltyRuleTriggerDTO loyaltyRuleTriggerDTO in loyaltyRuleDTO.LoyaltyRuleTriggerDTOList)
                {
                    if (loyaltyRuleTriggerDTO.LoyaltyRuleId != loyaltyRuleDTO.LoyaltyRuleId)
                    {
                        loyaltyRuleTriggerDTO.LoyaltyRuleId = loyaltyRuleDTO.LoyaltyRuleId;
                    }
                    if (loyaltyRuleTriggerDTO.IsChanged)
                    {
                        updatedLoyaltyRuleTriggerDTOList.Add(loyaltyRuleTriggerDTO);
                    }
                }
                if (updatedLoyaltyRuleTriggerDTOList.Any())
                {
                    LoyaltyRuleTriggerListBL loyaltyRuleTriggerListBL = new LoyaltyRuleTriggerListBL(executionContext, loyaltyRuleDTO.LoyaltyRuleTriggerDTOList);
                    loyaltyRuleTriggerListBL.Save(sqlTransaction);
                }
            }

            // For child record :LoyaltyBonusAttributeDTO

            if (loyaltyRuleDTO.LoyaltyBonusAttributeDTOList != null &&
               loyaltyRuleDTO.LoyaltyBonusAttributeDTOList.Any())
            {
                List<LoyaltyBonusAttributeDTO> updatedLoyaltyBonusAttributeDTOList = new List<LoyaltyBonusAttributeDTO>();
                foreach (LoyaltyBonusAttributeDTO loyaltyBonusAttributeDTO in loyaltyRuleDTO.LoyaltyBonusAttributeDTOList)
                {
                    if (loyaltyBonusAttributeDTO.LoyaltyRuleId != loyaltyRuleDTO.LoyaltyRuleId)
                    {
                        loyaltyBonusAttributeDTO.LoyaltyRuleId = loyaltyRuleDTO.LoyaltyRuleId;
                    }
                    if (loyaltyBonusAttributeDTO.IsChanged)
                    {
                        updatedLoyaltyBonusAttributeDTOList.Add(loyaltyBonusAttributeDTO);
                    }
                }
                if (updatedLoyaltyBonusAttributeDTOList.Any())
                {
                    LoyaltyBonusAttributeListBL loyaltyBonusAttributeListBL = new LoyaltyBonusAttributeListBL(executionContext, loyaltyRuleDTO.LoyaltyBonusAttributeDTOList);
                    loyaltyBonusAttributeListBL.Save(sqlTransaction);
                }
            }
        }

        /// <summary>
        /// Delete the loyaltyRule records from database based on loyaltyRuleId
        /// This method is only used for Web Management Studio.
        /// </summary>
        public void Delete(LoyaltyRuleDTO loyaltyRuleDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(loyaltyRuleDTO, sqlTransaction);
            try
            {
                LoyaltyRuleDataHandler loyaltyRuleDataHandler = new LoyaltyRuleDataHandler(sqlTransaction);

                if ((loyaltyRuleDTO.LoyaltyBonusAttributeDTOList != null &&
               loyaltyRuleDTO.LoyaltyBonusAttributeDTOList.Any((x => x.ActiveFlag == true))
                   || loyaltyRuleDTO.LoyaltyRuleTriggerDTOList != null &&
                loyaltyRuleDTO.LoyaltyRuleTriggerDTOList.Any(x => x.IsActive == true)))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1143);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new ForeignKeyException(message);
                }
                log.LogVariableState("LoyaltyRuleDTO", loyaltyRuleDTO);
                SaveChildRecords(sqlTransaction);
                if (loyaltyRuleDTO.LoyaltyRuleId >= 0)
                {
                    loyaltyRuleDataHandler.Delete(loyaltyRuleDTO);
                }
                loyaltyRuleDTO.AcceptChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public LoyaltyRuleDTO LoyaltyRuleDTO
        {
            get
            {
                return loyaltyRuleDTO;
            }
        }

        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (loyaltyRuleDTO == null)
            {
                //Validation to be implemented.
            }

            if (loyaltyRuleDTO.LoyaltyRuleTriggerDTOList != null && loyaltyRuleDTO.LoyaltyRuleTriggerDTOList.Any())
            {
                foreach (var loyaltyRuleTriggerDTO in loyaltyRuleDTO.LoyaltyRuleTriggerDTOList)
                {
                    if (loyaltyRuleTriggerDTO.IsChanged)
                    {
                        LoyaltyRuleTriggerBL loyaltyRuleTriggerBL = new LoyaltyRuleTriggerBL(executionContext, loyaltyRuleTriggerDTO);
                        validationErrorList.AddRange(loyaltyRuleTriggerBL.Validate(sqlTransaction)); //calls child validation method.
                    }
                }
            }
            if (loyaltyRuleDTO.LoyaltyBonusAttributeDTOList != null && loyaltyRuleDTO.LoyaltyBonusAttributeDTOList.Any())
            {
                foreach (var loyaltyBonusAttributeDTO in loyaltyRuleDTO.LoyaltyBonusAttributeDTOList)
                {
                    if (loyaltyBonusAttributeDTO.IsChanged)
                    {
                        LoyaltyBonusAttributeBL loyaltyBonusAttributeBL = new LoyaltyBonusAttributeBL(executionContext, loyaltyBonusAttributeDTO);
                        validationErrorList.AddRange(loyaltyBonusAttributeBL.Validate(sqlTransaction)); //calls child validation method.
                    }
                }
            }

            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

    }

    /// <summary>
    /// Manages the list of LoyaltyRule
    /// </summary>
    public class LoyaltyRuleListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<LoyaltyRuleDTO> loyaltyRuleDTOList = new List<LoyaltyRuleDTO>();
        
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public LoyaltyRuleListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public LoyaltyRuleListBL(ExecutionContext executionContext, List<LoyaltyRuleDTO> loyaltyRuleDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.loyaltyRuleDTOList = loyaltyRuleDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the LoyaltyRule list
        /// </summary>
        public List<LoyaltyRuleDTO> GetLoyaltyRuleDTOList(List<KeyValuePair<LoyaltyRuleDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            LoyaltyRuleDataHandler loyaltyRuleDataHandler = new LoyaltyRuleDataHandler(sqlTransaction);
            List<LoyaltyRuleDTO> returnValue = loyaltyRuleDataHandler.GetLoyaltyRuleDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        /// <summary>
        /// Returns the LoyaltyRule  List
        /// </summary>
        public List<LoyaltyRuleDTO> GetAllLoyaltyRuleDTOList(List<KeyValuePair<LoyaltyRuleDTO.SearchByParameters, string>> searchParameters,
                                          bool loadChildRecords = false, bool activeChildRecords = true,
                                          SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            LoyaltyRuleDataHandler loyaltyRuleDataHandler = new LoyaltyRuleDataHandler(sqlTransaction);
            List<LoyaltyRuleDTO> loyaltyRuleDTOList = loyaltyRuleDataHandler.GetLoyaltyRuleDTOList(searchParameters, sqlTransaction);
            if (loyaltyRuleDTOList != null && loyaltyRuleDTOList.Any() && loadChildRecords)
            {
                Build(loyaltyRuleDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(loyaltyRuleDTOList);
            return loyaltyRuleDTOList;
        }

        private void Build(List<LoyaltyRuleDTO> loyaltyRuleDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(loyaltyRuleDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, LoyaltyRuleDTO> loyaltyRuleIdLoyaltyRuleDTODictionary = new Dictionary<int, LoyaltyRuleDTO>();
            StringBuilder sb = new StringBuilder("");
            string loyaltyRuleIdSet;
            for (int i = 0; i < loyaltyRuleDTOList.Count; i++)
            {
                if (loyaltyRuleDTOList[i].LoyaltyRuleId == -1 ||
                    loyaltyRuleIdLoyaltyRuleDTODictionary.ContainsKey(loyaltyRuleDTOList[i].LoyaltyRuleId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(loyaltyRuleDTOList[i].LoyaltyRuleId.ToString());
                loyaltyRuleIdLoyaltyRuleDTODictionary.Add(loyaltyRuleDTOList[i].LoyaltyRuleId, loyaltyRuleDTOList[i]);
            }
            loyaltyRuleIdSet = sb.ToString();
            // Child 1  LoyaltyRuleTriggerDTO
            LoyaltyRuleTriggerListBL loyaltyRuleTriggerListBL = new LoyaltyRuleTriggerListBL(executionContext);
            List<KeyValuePair<LoyaltyRuleTriggerDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LoyaltyRuleTriggerDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<LoyaltyRuleTriggerDTO.SearchByParameters, string>(LoyaltyRuleTriggerDTO.SearchByParameters.LOYALTY_RULE_ID_LIST, loyaltyRuleIdSet.ToString()));
            List<LoyaltyRuleTriggerDTO> loyaltyRuleTriggerDTOList = loyaltyRuleTriggerListBL.GetLoyaltyRuleTriggerDTOList(searchParameters, sqlTransaction);
            if (loyaltyRuleTriggerDTOList.Any())
            {
                log.LogVariableState("loyaltyRuleTriggerDTOList", loyaltyRuleTriggerDTOList);
                foreach (var loyaltyRuleTriggerDTO in loyaltyRuleTriggerDTOList)
                {
                    if (loyaltyRuleIdLoyaltyRuleDTODictionary.ContainsKey(loyaltyRuleTriggerDTO.LoyaltyRuleId))
                    {
                        if (loyaltyRuleIdLoyaltyRuleDTODictionary[loyaltyRuleTriggerDTO.LoyaltyRuleId].LoyaltyRuleTriggerDTOList == null)
                        {
                            loyaltyRuleIdLoyaltyRuleDTODictionary[loyaltyRuleTriggerDTO.LoyaltyRuleId].LoyaltyRuleTriggerDTOList = new List<LoyaltyRuleTriggerDTO>();
                        }
                        loyaltyRuleIdLoyaltyRuleDTODictionary[loyaltyRuleTriggerDTO.LoyaltyRuleId].LoyaltyRuleTriggerDTOList.Add(loyaltyRuleTriggerDTO);
                    }
                }
            }

            // Child 2  LoyaltyBonusAttributeDTO
            LoyaltyBonusAttributeListBL loyaltyBonusAttributeListBL = new LoyaltyBonusAttributeListBL(executionContext);
            List<KeyValuePair<LoyaltyBonusAttributeDTO.SearchByParameters, string>> psearchParameters = new List<KeyValuePair<LoyaltyBonusAttributeDTO.SearchByParameters, string>>();
            psearchParameters.Add(new KeyValuePair<LoyaltyBonusAttributeDTO.SearchByParameters, string>(LoyaltyBonusAttributeDTO.SearchByParameters.LOYALTY_RULE_ID_LIST, loyaltyRuleIdSet.ToString()));
            List<LoyaltyBonusAttributeDTO> LoyaltyBonusAttributeDTOList = loyaltyBonusAttributeListBL.GetLoyaltyBonusAttributeDTOList(psearchParameters, true, activeChildRecords, sqlTransaction);
            if (LoyaltyBonusAttributeDTOList.Any())
            {
                log.LogVariableState("LoyaltyBonusAttributeDTOList", LoyaltyBonusAttributeDTOList);
                foreach (var loyaltyBonusAttributeDTO in LoyaltyBonusAttributeDTOList)
                {
                    if (loyaltyRuleIdLoyaltyRuleDTODictionary.ContainsKey(loyaltyBonusAttributeDTO.LoyaltyRuleId))
                    {
                        if (loyaltyRuleIdLoyaltyRuleDTODictionary[loyaltyBonusAttributeDTO.LoyaltyRuleId].LoyaltyRuleTriggerDTOList == null)
                        {
                            loyaltyRuleIdLoyaltyRuleDTODictionary[loyaltyBonusAttributeDTO.LoyaltyRuleId].LoyaltyRuleTriggerDTOList = new List<LoyaltyRuleTriggerDTO>();
                        }
                        loyaltyRuleIdLoyaltyRuleDTODictionary[loyaltyBonusAttributeDTO.LoyaltyRuleId].LoyaltyBonusAttributeDTOList.Add(loyaltyBonusAttributeDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Save or update records with inner collections
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (loyaltyRuleDTOList == null ||
                loyaltyRuleDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < loyaltyRuleDTOList.Count; i++)
            {
                var loyaltyRuleDTO = loyaltyRuleDTOList[i];
                if (loyaltyRuleDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    LoyaltyRuleBL loyaltyRuleBL = new LoyaltyRuleBL(executionContext, loyaltyRuleDTO);
                    loyaltyRuleBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving loyaltyRuleDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("loyaltyRuleDTO", loyaltyRuleDTO);
                    throw;
                }
            }
        }

        /// <summary>
        /// Delete the loyaltyRuleDTOList based 
        /// This method is only used for Web Management Studio.
        /// </summary>
        public void Delete()
        {
            log.LogMethodEntry();
            if (loyaltyRuleDTOList != null && loyaltyRuleDTOList.Any())
            {
                foreach (LoyaltyRuleDTO loyaltyRuleDTO in loyaltyRuleDTOList)
                {
                    if (loyaltyRuleDTO.IsChangedRecursive)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                LoyaltyRuleBL loyaltyRuleBL = new LoyaltyRuleBL(executionContext, loyaltyRuleDTO);
                                loyaltyRuleBL.Delete(loyaltyRuleDTO, parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
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
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                                throw;
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw;
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}

