/********************************************************************************************
 * Project Name - Promotions
 * Description  - Business logic file for  LoyaltyRuleTrigger
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
    /// Business logic for LoyaltyRuleTrigger class.
    /// </summary>
    public class LoyaltyRuleTriggerBL
    {
        private LoyaltyRuleTriggerDTO loyaltyRuleTriggerDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of LoyaltyRuleTriggerBL class
        /// </summary>
        private LoyaltyRuleTriggerBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates LoyaltyRuleTriggerBL object using the LoyaltyRuleTriggerDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="loyaltyRuleTriggerDTO">LoyaltyRuleTrigger DTO object</param>
        public LoyaltyRuleTriggerBL(ExecutionContext executionContext, LoyaltyRuleTriggerDTO loyaltyRuleTriggerDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, loyaltyRuleTriggerDTO);
            this.loyaltyRuleTriggerDTO = loyaltyRuleTriggerDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the loyaltyRuleTrigger  id as the parameter
        /// Would fetch the loyaltyRuleTrigger object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="id">id -loyaltyRuleTrigger </param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public LoyaltyRuleTriggerBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            LoyaltyRuleTriggerDataHandler loyaltyRuleTriggerDataHandler = new LoyaltyRuleTriggerDataHandler(sqlTransaction);
            loyaltyRuleTriggerDTO = loyaltyRuleTriggerDataHandler.GetLoyaltyRuleTriggerDTO(id);
            if (loyaltyRuleTriggerDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "LoyaltyRuleTriggerDTO", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the loyaltyRuleTriggerDTO
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (loyaltyRuleTriggerDTO.IsChanged == false
                && loyaltyRuleTriggerDTO.LoyaltyRuleTriggerId> -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            LoyaltyRuleTriggerDataHandler loyaltyRuleTriggerDataHandler = new LoyaltyRuleTriggerDataHandler(sqlTransaction);
            if (loyaltyRuleTriggerDTO.IsActive == true)
            {
                List<ValidationError> validationErrors = Validate();
                if (validationErrors.Any())
                {
                    string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                    log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                    throw new ValidationException(message, validationErrors);
                }
                if (loyaltyRuleTriggerDTO.LoyaltyRuleTriggerId < 0)
                {
                    log.LogVariableState("LoyaltyRuleTriggerDTO", loyaltyRuleTriggerDTO);
                    loyaltyRuleTriggerDTO = loyaltyRuleTriggerDataHandler.Insert(loyaltyRuleTriggerDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    loyaltyRuleTriggerDTO.AcceptChanges();
                }
                else if (loyaltyRuleTriggerDTO.IsChanged)
                {
                    log.LogVariableState("LoyaltyRuleTriggerDTO", loyaltyRuleTriggerDTO);
                    loyaltyRuleTriggerDTO = loyaltyRuleTriggerDataHandler.Update(loyaltyRuleTriggerDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    loyaltyRuleTriggerDTO.AcceptChanges();
                }

            }

            else   
            {

                if (loyaltyRuleTriggerDTO.LoyaltyRuleTriggerId >= 0)
                {
                    loyaltyRuleTriggerDataHandler.Delete(loyaltyRuleTriggerDTO);
                }
                loyaltyRuleTriggerDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the loyaltyRuleTriggerDTO 
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
        public LoyaltyRuleTriggerDTO LoyaltyRuleTriggerDTO
        {
            get
            {
                return loyaltyRuleTriggerDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of LoyaltyRuleTrigger
    /// </summary>
    public class LoyaltyRuleTriggerListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<LoyaltyRuleTriggerDTO> loyaltyRuleTriggerDTOList = new List<LoyaltyRuleTriggerDTO>(); // To be initialized
        /// <summary>
        /// Parameterized constructor of LoyaltyRuleTriggerListBL
        /// </summary>
        /// <param name="executionContext">executionContext object</param>
        public LoyaltyRuleTriggerListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="loyaltyRuleTriggerDTOList">LoyaltyRuleTrigger DTO List as parameter </param>
        public LoyaltyRuleTriggerListBL(ExecutionContext executionContext,
                                               List<LoyaltyRuleTriggerDTO> loyaltyRuleTriggerDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, loyaltyRuleTriggerDTOList);
            this.loyaltyRuleTriggerDTOList = loyaltyRuleTriggerDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the LoyaltyRuleTrigger DTO list based on the search parameter.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>The List of LoyaltyRuleTriggerDTO </returns>
        public List<LoyaltyRuleTriggerDTO> GetLoyaltyRuleTriggerDTOList(List<KeyValuePair<LoyaltyRuleTriggerDTO.SearchByParameters, string>> searchParameters,
                                                              SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            LoyaltyRuleTriggerDataHandler loyaltyRuleTriggerDataHandler = new LoyaltyRuleTriggerDataHandler(sqlTransaction);
            List<LoyaltyRuleTriggerDTO> loyaltyRuleTriggerDTOList = loyaltyRuleTriggerDataHandler.GetLoyaltyRuleTriggerDTOList(searchParameters);
            log.LogMethodExit(loyaltyRuleTriggerDTOList);
            return loyaltyRuleTriggerDTOList;
        }

        /// <summary>
        /// Saves the  list of LoyaltyRuleTrigger DTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (loyaltyRuleTriggerDTOList == null ||
                loyaltyRuleTriggerDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < loyaltyRuleTriggerDTOList.Count; i++)
            {
                var loyaltyRuleTriggerDTO = loyaltyRuleTriggerDTOList[i];
                if (loyaltyRuleTriggerDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    LoyaltyRuleTriggerBL loyaltyRuleTriggerBL = new LoyaltyRuleTriggerBL(executionContext, loyaltyRuleTriggerDTO);
                    loyaltyRuleTriggerBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving LoyaltyRuleTriggerDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("LoyaltyRuleTriggerDTO", loyaltyRuleTriggerDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
