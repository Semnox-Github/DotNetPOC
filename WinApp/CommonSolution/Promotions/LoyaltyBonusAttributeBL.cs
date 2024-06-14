/********************************************************************************************
 * Project Name - Promotions
 * Description  - Business logic file for  LoyaltyBonusAttribute
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80     24-Jun-2019     Girish Kundar           Created 
 ********************************************************************************************/
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
    /// Business logic for LoyaltyBonusAttribute class.
    /// </summary>
    public class LoyaltyBonusAttributeBL
    {
        private LoyaltyBonusAttributeDTO loyaltyBonusAttributeDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of LoyaltyBonusAttributeBL class
        /// </summary>
        private LoyaltyBonusAttributeBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates LoyaltyBonusAttributeBL object using the LoyaltyBonusAttributeDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="loyaltyBonusAttributeDTO">LoyaltyBonusAttributeDTO object</param>
        public LoyaltyBonusAttributeBL(ExecutionContext executionContext, LoyaltyBonusAttributeDTO loyaltyBonusAttributeDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, loyaltyBonusAttributeDTO);
            this.loyaltyBonusAttributeDTO = loyaltyBonusAttributeDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the LoyaltyBonusAttribute id as the parameter
        /// Would fetch the LoyaltyBonusAttribute object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="id">id - LoyaltyBonusAttribute</param>
        /// <param name="loadChildRecords">loadChildRecords either true or false</param>
        /// <param name="activeChildRecords">activeChildRecords either true or false</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public LoyaltyBonusAttributeBL(ExecutionContext executionContext, int id, bool loadChildRecords = true,
            bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            LoyaltyBonusAttributeDataHandler loyaltyBonusAttributeDataHandler = new LoyaltyBonusAttributeDataHandler(sqlTransaction);
            loyaltyBonusAttributeDTO = loyaltyBonusAttributeDataHandler.GetLoyaltyBonusAttributeDTO(id);
            if (loyaltyBonusAttributeDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "LoyaltyBonusAttribute", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the child records for loyaltyBonusAttribute object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            LoyaltyBonusPurchaseCriteriaListBL loyaltyBonusPurchaseCriteriaListBL = new LoyaltyBonusPurchaseCriteriaListBL(executionContext);
            List<KeyValuePair<LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters, string>(LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters.LOYALTY_BONUS_ID, loyaltyBonusAttributeDTO.LoyaltyBonusId.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters, string>(LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            }
            loyaltyBonusAttributeDTO.LoyaltyBonusPurchaseCriteriaDTOList = loyaltyBonusPurchaseCriteriaListBL.GetLoyaltyBonusPurchaseCriteriaDTOList(searchParameters, sqlTransaction);
            // Build second child

            LoyaltyBonusRewardCriteriaListBL loyaltyBonusRewardCriteriaListBL = new LoyaltyBonusRewardCriteriaListBL(executionContext);
            List<KeyValuePair<LoyaltyBonusRewardCriteriaDTO.SearchByParameters, string>> psearchParameters = new List<KeyValuePair<LoyaltyBonusRewardCriteriaDTO.SearchByParameters, string>>();
            psearchParameters.Add(new KeyValuePair<LoyaltyBonusRewardCriteriaDTO.SearchByParameters, string>(LoyaltyBonusRewardCriteriaDTO.SearchByParameters.LOYALTY_BONUS_ID, loyaltyBonusAttributeDTO.LoyaltyBonusId.ToString()));
            if (activeChildRecords)
            {
                psearchParameters.Add(new KeyValuePair<LoyaltyBonusRewardCriteriaDTO.SearchByParameters, string>(LoyaltyBonusRewardCriteriaDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            }
            loyaltyBonusAttributeDTO.LoyaltyBonusRewardCriteriaDTOList = loyaltyBonusRewardCriteriaListBL.GetLoyaltyBonusRewardCriteriaDTOList(psearchParameters, sqlTransaction);

            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the loyaltyBonusAttribute
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            if (loyaltyBonusAttributeDTO.IsChangedRecursive == false)
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
            LoyaltyBonusAttributeDataHandler loyaltyBonusAttributeDataHandler = new LoyaltyBonusAttributeDataHandler(sqlTransaction);
            if (loyaltyBonusAttributeDTO.ActiveFlag == true)
            {
                if (loyaltyBonusAttributeDTO.LoyaltyBonusId < 0)
                {
                    log.LogVariableState("LoyaltyBonusAttributeDTO", loyaltyBonusAttributeDTO);
                    loyaltyBonusAttributeDTO = loyaltyBonusAttributeDataHandler.Insert(loyaltyBonusAttributeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    loyaltyBonusAttributeDTO.AcceptChanges();
                }
                else if (loyaltyBonusAttributeDTO.IsChanged)
                {
                    log.LogVariableState("LoyaltyBonusAttributeDTO", loyaltyBonusAttributeDTO);
                    loyaltyBonusAttributeDTO = loyaltyBonusAttributeDataHandler.Update(loyaltyBonusAttributeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    loyaltyBonusAttributeDTO.AcceptChanges();
                }
                SaveLoyaltyBonusAttributeChild(sqlTransaction);
            }
            else  
            {
                if ((loyaltyBonusAttributeDTO.LoyaltyBonusRewardCriteriaDTOList != null && loyaltyBonusAttributeDTO.LoyaltyBonusPurchaseCriteriaDTOList.Any(x => x.ActiveFlag == true))
                   || loyaltyBonusAttributeDTO.LoyaltyBonusPurchaseCriteriaDTOList != null && (loyaltyBonusAttributeDTO.LoyaltyBonusRewardCriteriaDTOList.Any(x => x.ActiveFlag == true)))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1143);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new ForeignKeyException(message);
                }
                log.LogVariableState("LoyaltyBonusAttributeDTO", loyaltyBonusAttributeDTO);
                SaveLoyaltyBonusAttributeChild(sqlTransaction);
                if (loyaltyBonusAttributeDTO.LoyaltyBonusId >= 0)
                {
                    loyaltyBonusAttributeDataHandler.Delete(loyaltyBonusAttributeDTO);
                }
                loyaltyBonusAttributeDTO.AcceptChanges();
            }
        }

        /// <summary>
        /// Saves the child records : LoyaltyBonusPurchaseCriteriaDTO List and LoyaltyBonusRewardCriteria DTO List
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        private void SaveLoyaltyBonusAttributeChild(SqlTransaction sqlTransaction)
        {

            // For child records :LoyaltyBonusRewardCriteriaDTO
            if (loyaltyBonusAttributeDTO.LoyaltyBonusRewardCriteriaDTOList != null &&
                loyaltyBonusAttributeDTO.LoyaltyBonusRewardCriteriaDTOList.Any())
            {
                List<LoyaltyBonusRewardCriteriaDTO> updatedLoyaltyBonusRewardCriteriaDTOList = new List<LoyaltyBonusRewardCriteriaDTO>();
                foreach (var LoyaltyBonusRewardCriteriaDTO in loyaltyBonusAttributeDTO.LoyaltyBonusRewardCriteriaDTOList)
                {
                    if (LoyaltyBonusRewardCriteriaDTO.LoyaltyBonusId != loyaltyBonusAttributeDTO.LoyaltyBonusId)
                    {
                        LoyaltyBonusRewardCriteriaDTO.LoyaltyBonusId = loyaltyBonusAttributeDTO.LoyaltyBonusId;
                    }
                    if (LoyaltyBonusRewardCriteriaDTO.IsChanged)
                    {
                        updatedLoyaltyBonusRewardCriteriaDTOList.Add(LoyaltyBonusRewardCriteriaDTO);
                    }
                }
                if (updatedLoyaltyBonusRewardCriteriaDTOList.Any())
                {
                    log.LogVariableState("UpdatedLoyaltyBonusRewardCriteriaDTOList", updatedLoyaltyBonusRewardCriteriaDTOList);
                    LoyaltyBonusRewardCriteriaListBL loyaltyBonusRewardCriteriaListBL = new LoyaltyBonusRewardCriteriaListBL(executionContext, updatedLoyaltyBonusRewardCriteriaDTOList);
                    loyaltyBonusRewardCriteriaListBL.Save(sqlTransaction);
                }
            }

            // For child records :LoyaltyBonusPurchaseCriteria DTO

            if (loyaltyBonusAttributeDTO.LoyaltyBonusPurchaseCriteriaDTOList != null &&
                loyaltyBonusAttributeDTO.LoyaltyBonusPurchaseCriteriaDTOList.Any())
            {
                List<LoyaltyBonusPurchaseCriteriaDTO> updatedLoyaltyBonusPurchaseCriteriaDTOList = new List<LoyaltyBonusPurchaseCriteriaDTO>();
                foreach (var loyaltyBonusPurchaseCriteriaDTO in loyaltyBonusAttributeDTO.LoyaltyBonusPurchaseCriteriaDTOList)
                {
                    if (loyaltyBonusPurchaseCriteriaDTO.LoyaltyBonusId != loyaltyBonusAttributeDTO.LoyaltyBonusId)
                    {
                        loyaltyBonusPurchaseCriteriaDTO.LoyaltyBonusId = loyaltyBonusAttributeDTO.LoyaltyBonusId;
                    }
                    if (loyaltyBonusPurchaseCriteriaDTO.IsChanged)
                    {
                        updatedLoyaltyBonusPurchaseCriteriaDTOList.Add(loyaltyBonusPurchaseCriteriaDTO);
                    }
                }
                if (updatedLoyaltyBonusPurchaseCriteriaDTOList.Any())
                {
                    LoyaltyBonusPurchaseCriteriaListBL loyaltyBonusPurchaseCriteriaListBL = new LoyaltyBonusPurchaseCriteriaListBL(executionContext, updatedLoyaltyBonusPurchaseCriteriaDTOList);
                    loyaltyBonusPurchaseCriteriaListBL.Save(sqlTransaction);
                }
            }
            else
            {
                log.Debug("loyaltyBonusAttributeDTO.LoyaltyBonusPurchaseCriteriaDTOList");
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the loyaltyBonusAttribute and LoyaltyBonusPurchaseCriteriaDTOList , LoyaltyBonusRewardCriteriaDTOList - children 
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();

            if (loyaltyBonusAttributeDTO.LoyaltyBonusPurchaseCriteriaDTOList != null)
            {
                foreach (var loyaltyBonusPurchaseCriteriaDTO in loyaltyBonusAttributeDTO.LoyaltyBonusPurchaseCriteriaDTOList)
                {
                    if (loyaltyBonusPurchaseCriteriaDTO.IsChanged)
                    {
                        log.LogVariableState("LoyaltyBonusPurchaseCriteriaDTO", loyaltyBonusPurchaseCriteriaDTO);
                        LoyaltyBonusPurchaseCriteriaBL loyaltyBonusPurchaseCriteriaBL = new LoyaltyBonusPurchaseCriteriaBL(executionContext, loyaltyBonusPurchaseCriteriaDTO);
                        validationErrorList.AddRange(loyaltyBonusPurchaseCriteriaBL.Validate(sqlTransaction));
                    }
                }
            }

            if (loyaltyBonusAttributeDTO.LoyaltyBonusRewardCriteriaDTOList != null)
            {
                foreach (var loyaltyBonusRewardCriteriaDTO in loyaltyBonusAttributeDTO.LoyaltyBonusRewardCriteriaDTOList)
                {
                    if (loyaltyBonusRewardCriteriaDTO.IsChanged)
                    {
                        log.LogVariableState("LoyaltyBonusRewardCriteriaDTO", loyaltyBonusRewardCriteriaDTO);
                        LoyaltyBonusRewardCriteriaBL loyaltyBonusRewardCriteriaBL = new LoyaltyBonusRewardCriteriaBL(executionContext, loyaltyBonusRewardCriteriaDTO);
                        validationErrorList.AddRange(loyaltyBonusRewardCriteriaBL.Validate(sqlTransaction));
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public LoyaltyBonusAttributeDTO LoyaltyBonusAttributeDTO
        {
            get
            {
                return loyaltyBonusAttributeDTO;
            }
        }

    }
    /// <summary>
    /// Manages the list of AppUIPanelElementAttribute
    /// </summary>
    public class LoyaltyBonusAttributeListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<LoyaltyBonusAttributeDTO> loyaltyBonusAttributeDTOList = new List<LoyaltyBonusAttributeDTO>();

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">ExecutionContext object as parameter</param>
        public LoyaltyBonusAttributeListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">ExecutionContext object as parameter</param>
        /// <param name="loyaltyBonusAttributeDTOList">LoyaltyBonusAttributeDTOList object as parameter</param>
        public LoyaltyBonusAttributeListBL(ExecutionContext executionContext,
                                              List<LoyaltyBonusAttributeDTO> loyaltyBonusAttributeDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, loyaltyBonusAttributeDTOList);
            this.loyaltyBonusAttributeDTOList = loyaltyBonusAttributeDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the loyaltyBonusAttributeDTO List based on the search Parameters
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="loadChildRecords">loadChildRecords holds either true or false</param>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <returns>returns the LoyaltyBonusAttributeDTO List</returns>
        public List<LoyaltyBonusAttributeDTO> GetLoyaltyBonusAttributeDTOList(List<KeyValuePair<LoyaltyBonusAttributeDTO.SearchByParameters, string>> searchParameters,
                                                                       bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            LoyaltyBonusAttributeDataHandler loyaltyBonusAttributeDataHandler = new LoyaltyBonusAttributeDataHandler(sqlTransaction);
            List<LoyaltyBonusAttributeDTO> loyaltyBonusAttributeDTOList = loyaltyBonusAttributeDataHandler.GetLoyaltyBonusAttributeDTOList(searchParameters);
            if (loadChildRecords && loyaltyBonusAttributeDTOList.Any())
            {
                Build(loyaltyBonusAttributeDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(loyaltyBonusAttributeDTOList);
            return loyaltyBonusAttributeDTOList;
        }

        /// <summary>
        /// Builds the List of LoyaltyBonusAttribute objects based on the list of LoyaltyBonusAttribute id.
        /// </summary>
        /// <param name="loyaltyBonusAttributeDTOList">LoyaltyBonusAttributeDTO List</param>
        /// <param name="activeChildRecords">activeChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        private void Build(List<LoyaltyBonusAttributeDTO> loyaltyBonusAttributeDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(loyaltyBonusAttributeDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, LoyaltyBonusAttributeDTO> LoyaltyBonusAttributeIdRewardCriteriaDictionary = new Dictionary<int, LoyaltyBonusAttributeDTO>();
            string loyaltyBonusAttributeIdSet = string.Empty;
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < loyaltyBonusAttributeDTOList.Count; i++)
            {
                if (loyaltyBonusAttributeDTOList[i].LoyaltyBonusId == -1 ||
                    LoyaltyBonusAttributeIdRewardCriteriaDictionary.ContainsKey(loyaltyBonusAttributeDTOList[i].LoyaltyBonusId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(loyaltyBonusAttributeDTOList[i].LoyaltyBonusId);
                LoyaltyBonusAttributeIdRewardCriteriaDictionary.Add(loyaltyBonusAttributeDTOList[i].LoyaltyBonusId, loyaltyBonusAttributeDTOList[i]);
            }
            loyaltyBonusAttributeIdSet = sb.ToString();
            LoyaltyBonusRewardCriteriaListBL loyaltyBonusRewardCriteriaListBL = new LoyaltyBonusRewardCriteriaListBL(executionContext);
            List<KeyValuePair<LoyaltyBonusRewardCriteriaDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LoyaltyBonusRewardCriteriaDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<LoyaltyBonusRewardCriteriaDTO.SearchByParameters, string>(LoyaltyBonusRewardCriteriaDTO.SearchByParameters.LOYALTY_BONUS_ID_LIST, loyaltyBonusAttributeIdSet.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<LoyaltyBonusRewardCriteriaDTO.SearchByParameters, string>(LoyaltyBonusRewardCriteriaDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            }
            List<LoyaltyBonusRewardCriteriaDTO> loyaltyBonusRewardCriteriaDTOList = loyaltyBonusRewardCriteriaListBL.GetLoyaltyBonusRewardCriteriaDTOList(searchParameters, sqlTransaction);
            if (loyaltyBonusRewardCriteriaDTOList.Any())
            {
                log.LogVariableState("LoyaltyBonusRewardCriteriaDTOList", loyaltyBonusRewardCriteriaDTOList);
                foreach (var loyaltyBonusRewardCriteriaDTO in loyaltyBonusRewardCriteriaDTOList)
                {
                    if (LoyaltyBonusAttributeIdRewardCriteriaDictionary.ContainsKey(loyaltyBonusRewardCriteriaDTO.LoyaltyBonusId))
                    {
                        if (LoyaltyBonusAttributeIdRewardCriteriaDictionary[loyaltyBonusRewardCriteriaDTO.LoyaltyBonusId].LoyaltyBonusRewardCriteriaDTOList == null)
                        {
                            LoyaltyBonusAttributeIdRewardCriteriaDictionary[loyaltyBonusRewardCriteriaDTO.LoyaltyBonusId].LoyaltyBonusRewardCriteriaDTOList = new List<LoyaltyBonusRewardCriteriaDTO>();
                        }
                        LoyaltyBonusAttributeIdRewardCriteriaDictionary[loyaltyBonusRewardCriteriaDTO.LoyaltyBonusId].LoyaltyBonusRewardCriteriaDTOList.Add(loyaltyBonusRewardCriteriaDTO);
                    }
                }
            }
            // same for loyaltyBonusPurchaseCriteria
            LoyaltyBonusPurchaseCriteriaListBL loyaltyBonusPurchaseCriteriaListBL = new LoyaltyBonusPurchaseCriteriaListBL(executionContext);
            List<KeyValuePair<LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters, string>> psearchParameters = new List<KeyValuePair<LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters, string>>();
            psearchParameters.Add(new KeyValuePair<LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters, string>(LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters.LOYALTY_BONUS_ID_LIST, loyaltyBonusAttributeIdSet.ToString()));
            if (activeChildRecords)
            {
                psearchParameters.Add(new KeyValuePair<LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters, string>(LoyaltyBonusPurchaseCriteriaDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            }
            List<LoyaltyBonusPurchaseCriteriaDTO> loyaltyBonusPurchaseCriteriaDTOList = loyaltyBonusPurchaseCriteriaListBL.GetLoyaltyBonusPurchaseCriteriaDTOList(psearchParameters, sqlTransaction);
            if (loyaltyBonusPurchaseCriteriaDTOList.Any())
            {
                log.LogVariableState("LoyaltyBonusPurchaseCriteriaDTOList", loyaltyBonusPurchaseCriteriaDTOList);
                foreach (var loyaltyBonusPurchaseCriteriaDTO in loyaltyBonusPurchaseCriteriaDTOList)
                {
                    if (LoyaltyBonusAttributeIdRewardCriteriaDictionary.ContainsKey(loyaltyBonusPurchaseCriteriaDTO.LoyaltyBonusId))
                    {
                        if (LoyaltyBonusAttributeIdRewardCriteriaDictionary[loyaltyBonusPurchaseCriteriaDTO.LoyaltyBonusId].LoyaltyBonusPurchaseCriteriaDTOList == null)
                        {
                            LoyaltyBonusAttributeIdRewardCriteriaDictionary[loyaltyBonusPurchaseCriteriaDTO.LoyaltyBonusId].LoyaltyBonusPurchaseCriteriaDTOList = new List<LoyaltyBonusPurchaseCriteriaDTO>();
                        }
                        LoyaltyBonusAttributeIdRewardCriteriaDictionary[loyaltyBonusPurchaseCriteriaDTO.LoyaltyBonusId].LoyaltyBonusPurchaseCriteriaDTOList.Add(loyaltyBonusPurchaseCriteriaDTO);
                    }
                }
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the loyaltyBonusAttribute DTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (loyaltyBonusAttributeDTOList == null ||
                loyaltyBonusAttributeDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < loyaltyBonusAttributeDTOList.Count; i++)
            {
                var loyaltyBonusAttributeDTO = loyaltyBonusAttributeDTOList[i];
                if (loyaltyBonusAttributeDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    LoyaltyBonusAttributeBL loyaltyBonusAttributeBL = new LoyaltyBonusAttributeBL(executionContext, loyaltyBonusAttributeDTO);
                    loyaltyBonusAttributeBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving LoyaltyBonusAttributeDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("LoyaltyBonusAttributeDTO", loyaltyBonusAttributeDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

    }

}
