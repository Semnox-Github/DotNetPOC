/********************************************************************************************
 * Project Name - Promotions
 * Description  - Business logic file for  PromotionRule
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80      24-Jun-2019     Girish Kundar           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// Business logic for PromotionRule class.
    /// </summary>
    public class PromotionRuleBL
    {
        private PromotionRuleDTO promotionRuleDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of PromotionRuleBL class
        /// </summary>
        private PromotionRuleBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates PromotionRuleBL object using the PromotionRuleDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="promotionRuleDTO">PromotionRule DTO object</param>
        public PromotionRuleBL(ExecutionContext executionContext, PromotionRuleDTO promotionRuleDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, promotionRuleDTO);
            this.promotionRuleDTO = promotionRuleDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the PromotionRule  id as the parameter
        /// Would fetch the PromotionRule object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="id">id -PromotionRule </param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public PromotionRuleBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            PromotionRuleDataHandler promotionRuleDataHandler = new PromotionRuleDataHandler(sqlTransaction);
            promotionRuleDTO = promotionRuleDataHandler.GetPromotionRuleDTO(id);
            if (promotionRuleDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "PromotionRuleDTO", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the PromotionRule DTO
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (promotionRuleDTO.IsChanged == false
                &&  promotionRuleDTO.Id > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            PromotionRuleDataHandler promotionRuleDataHandler = new PromotionRuleDataHandler(sqlTransaction);
            if (promotionRuleDTO.IsActive == true)
            {
                List<ValidationError> validationErrors = Validate();
                if (validationErrors.Any())
                {
                    string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                    log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                    throw new ValidationException(message, validationErrors);
                }
                if (promotionRuleDTO.Id < 0)
                {
                    log.LogVariableState("PromotionRuleDTO", promotionRuleDTO);
                    promotionRuleDTO = promotionRuleDataHandler.Insert(promotionRuleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    promotionRuleDTO.AcceptChanges();
                }
                else if (promotionRuleDTO.IsChanged)
                {
                    log.LogVariableState("PromotionRuleDTO", promotionRuleDTO);
                    promotionRuleDTO = promotionRuleDataHandler.Update(promotionRuleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    promotionRuleDTO.AcceptChanges();
                }

            }

            else  
            {

                if (promotionRuleDTO.Id >= 0)
                {
                    promotionRuleDataHandler.Delete(promotionRuleDTO);
                }
                promotionRuleDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the PromotionRuleDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public PromotionRuleDTO PromotionRuleDTO
        {
            get
            {
                return promotionRuleDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of PromotionRule
    /// </summary>
    public class PromotionRuleListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<PromotionRuleDTO> promotionRuleDTOList = new List<PromotionRuleDTO>(); // To be initialized
        /// <summary>
        /// Parameterized constructor of PromotionRuleListBL
        /// </summary>
        /// <param name="executionContext">executionContext object</param>
        public PromotionRuleListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="promotionRuleDTOList">PromotionRule DTO List as parameter </param>
        public PromotionRuleListBL(ExecutionContext executionContext,
                                               List<PromotionRuleDTO> promotionRuleDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, promotionRuleDTOList);
            this.promotionRuleDTOList = promotionRuleDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the PromotionRule DTO list based on the search parameter.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>The List of PromotionRuleDTO </returns>
        public List<PromotionRuleDTO> GetPromotionRuleDTOList(List<KeyValuePair<PromotionRuleDTO.SearchByParameters, string>> searchParameters,
                                                              SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            PromotionRuleDataHandler promotionRuleDataHandler = new PromotionRuleDataHandler(sqlTransaction);
            List<PromotionRuleDTO> promotionRuleDTOList = promotionRuleDataHandler.GetPromotionRuleDTOList(searchParameters);
            log.LogMethodExit(promotionRuleDTOList);
            return promotionRuleDTOList;
        }

       

        /// <summary>
        /// Saves the  list of PromotionRule DTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (promotionRuleDTOList == null ||
                promotionRuleDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < promotionRuleDTOList.Count; i++)
            {
                var promotionRuleDTO = promotionRuleDTOList[i];
                if (promotionRuleDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    PromotionRuleBL promotionRuleBL = new PromotionRuleBL(executionContext, promotionRuleDTO);
                    promotionRuleBL.Save(sqlTransaction);
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
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving PromotionRuleDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("PromotionRuleDTO", promotionRuleDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the PromotionRuleDTO List for product Id List
        /// </summary>
        /// <param name="promotionIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductDTO</returns>
        public List<PromotionRuleDTO> GetPromotionRuleDTOListForProducts(List<int> promotionIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(promotionIdList, activeRecords, sqlTransaction);
            PromotionRuleDataHandler promotionRuleDataHandler = new PromotionRuleDataHandler(sqlTransaction);
            List<PromotionRuleDTO> promotionRuleDTOList = promotionRuleDataHandler.GetPromotionRuleDTOList(promotionIdList, activeRecords);
            log.LogMethodExit(promotionRuleDTOList);
            return promotionRuleDTOList;
        }
    }
}
